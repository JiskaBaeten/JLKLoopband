/*   PROGRAM BY PIETER JORISSEN*/
/*
 * This C# class can be used to emulate the WINAPI SendInput behavior in C#
 * This function synthesizes keystrokes, stylus and mouse motions, and button clicks.
 * This is necessary as it is not part of the .NET framework
 * Therefore we use the  System.Runtime.InteropServices; and 
 * explicit dllimport of the user32.dll and redefine all necessary functions and
 * structs, variables, ...
 * This file starts with some examples in comment to how the class can be used
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KdGSerialPortManager
{
    public partial class SerialPortForm : Form
    {
        //class members

        /*This buffer variable will contain the received messages 
         from the COM port 
         it is filled and emptied in the data_received callback function 
         the default behavior is writing it to the form and emptying it again
         */
        private String mBuffer = "";

        //main form function
        public SerialPortForm()
        {
            InitializeComponent();
            initializeForm();
        }

        //initialize the form with all possible values
        protected void initializeForm()
        {
            //fill in the possible form values
            portNameSelection.Items.Clear();//clear the list first
            for (byte i = 0; i < 100; ++i)
            {
                portNameSelection.Items.Add("COM" + i.ToString());
            }
            portNameSelection.SelectedIndex = 4;

            //fill the possible baud rate values
            baudRateSelection.Items.Clear();
            baudRateSelection.Items.Add("300");
            baudRateSelection.Items.Add("600");
            baudRateSelection.Items.Add("1200");
            baudRateSelection.Items.Add("2400");
            baudRateSelection.Items.Add("4800");
            baudRateSelection.Items.Add("9600");
            baudRateSelection.Items.Add("14400");
            baudRateSelection.Items.Add("28800");
            baudRateSelection.Items.Add("36000");
            baudRateSelection.Items.Add("115000");
            baudRateSelection.SelectedIndex = 5;

            //Fill the parities
            paritySelection.Items.Clear();
            paritySelection.Items.Add("None");
            paritySelection.Items.Add("Odd");
            paritySelection.Items.Add("Even");
            paritySelection.Items.Add("Mark");
            paritySelection.Items.Add("Space");
            paritySelection.SelectedIndex = 0;

            //Fill the Stop bits
            stopBitsSelection.Items.Clear();
            stopBitsSelection.Items.Add("None");
            stopBitsSelection.Items.Add("1");
            stopBitsSelection.Items.Add("1.5");
            stopBitsSelection.Items.Add("2");
            stopBitsSelection.SelectedIndex = 1;

            //Fill the Data bits
            dataBitsList.Items.Clear();
            dataBitsList.Items.Add("7");
            dataBitsList.Items.Add("8");
            dataBitsList.Items.Add("9");
            dataBitsList.SelectedIndex = 1;

            //set text mode default
            radioText.Select();

            //set the buttons enabled/disabled
            buttonOpen.Enabled = true;
            buttonClose.Enabled = false;

           //set the standard received bytes necessary to call data received
            txtReceiveBytes.Text = "1";

        }

        //opens the COM port with the selected values from the Form
        protected void buttonOpen_Click(object sender, EventArgs e)
        {
       
            if (!mSerialPort.IsOpen)
            {
                mSerialPort.PortName = portNameSelection.SelectedItem.ToString();
                mSerialPort.BaudRate = Convert.ToInt32(baudRateSelection.SelectedItem.ToString());

                switch (paritySelection.SelectedItem.ToString())
                {
                    case "None": mSerialPort.Parity = System.IO.Ports.Parity.None; break;
                    case "Odd": mSerialPort.Parity = System.IO.Ports.Parity.Odd; break;
                    case "Even": mSerialPort.Parity = System.IO.Ports.Parity.Even; break;
                    case "Mark": mSerialPort.Parity = System.IO.Ports.Parity.Mark; break;
                    case "Space": mSerialPort.Parity = System.IO.Ports.Parity.Space; break;
                    default: mSerialPort.Parity = System.IO.Ports.Parity.None; break;
                }

                switch (stopBitsSelection.SelectedItem.ToString())
                {
                        //first line gives an error???
                    case "None": mSerialPort.StopBits = System.IO.Ports.StopBits.None; break;
                    case "1": mSerialPort.StopBits = System.IO.Ports.StopBits.One; break;
                    case "1.5": mSerialPort.StopBits = System.IO.Ports.StopBits.OnePointFive; break;
                    case "2": mSerialPort.StopBits = System.IO.Ports.StopBits.Two; break;
                    default: mSerialPort.StopBits = System.IO.Ports.StopBits.One; break;
                }

                mSerialPort.DataBits = Convert.ToInt32(dataBitsList.SelectedItem.ToString());
               // mSerialPort.WriteTimeout = 500;
               // mSerialPort.ReadTimeout = 500;

                mSerialPort.ReadBufferSize = 4096;//test
                mSerialPort.Handshake = System.IO.Ports.Handshake.None;
                mSerialPort.RtsEnable = true;
                

                //set the number of bytes that are needed to call the datareceived 
                //method
                mSerialPort.ReceivedBytesThreshold = Convert.ToInt32(txtReceiveBytes.Text);
               

                //now try to open the serial port
                openPort();

                //check if the port is now open and change the available buttons
                if (mSerialPort.IsOpen)
                {
                    buttonOpen.Enabled = false;
                    buttonClose.Enabled = true;
                    OutputMessage("PORT IS NOW OPENED\n");
                    mSerialPort.DiscardInBuffer();
                }
            }
            else
            {
                OutputMessage("ERROR: Cannot open port, port is already opened\n");
            }
        }


        // close the COM port
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            if (mSerialPort.IsOpen)
            {
                try
                {
                    mSerialPort.Close();
                }
                catch (Exception ex)
                {
                    String mess = "ERROR: " + ex.ToString();
                    OutputMessage(mess + "\n");
                }
            }
            else
            {
                OutputMessage("WARNING: trying to close a port that is not opened\n");
            }
            //set the button state
            if (!mSerialPort.IsOpen)
            {
                buttonOpen.Enabled = true;
                buttonClose.Enabled = false;
                OutputMessage("PORT IS NOW CLOSED\n");
            }

        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);
        //writes a message to the textbox
        protected void OutputMessage(String message)
        {
            if (outputTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(OutputMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                outputTextBox.AppendText(message);
                //scroll to the bottom
                outputTextBox.ScrollToCaret();
            }
        }

        //this function just adds bytes received from the serialport to the buffer
        private void mSerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            mBuffer += mSerialPort.ReadExisting();
            if (mBuffer.Length > 0)
            {
                /*TODO: add your own COM received message handling code here*/
                OutputMessage(mBuffer);

                //empty the buffer
                //TODO: in case of your own buffer handling
                //make sure you only clear the buffer or part of it 
                // if it has been handled
                mBuffer = "";
            }
            
        }

        //output the error message
        private void mSerialPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            OutputMessage("ERROR: " + e.ToString() + "\n");
        }

        //output pin changed message
        private void mSerialPort_PinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            OutputMessage("PIN CHANGED: " + e.ToString() + "\n");
        }


   
    

        //load the form, make sure all COM ports are closed 
        private void Form1_Load(object sender, EventArgs e)
        {
            closePort();
        }

        //use this method if anything needs to be done when the application closes
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        //send message to COM port followed by "\n"
        //TODO: clear writing to textbox if not necessary
        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (mSerialPort.IsOpen && inputBox.Text.Length > 0)
            {
                try
                {
                    mSerialPort.WriteLine(inputBox.Text);
                }
                catch (Exception ex)
                {
                    String mess = "ERROR: " + ex.ToString();
                    OutputMessage(mess + "\n");
                }

                OutputMessage("Message: \"" + inputBox.Text + "\" was sent to the serial port" + "\n");
                inputBox.Text = "";
            }
           
        }

        //open the com port
        private void openPort()
        {
            try
            {
                mSerialPort.Open();
            }
            catch (Exception ex)
            {
                String mess = "ERROR: " + ex.ToString();
                OutputMessage(mess + "\n");
            }
        }

        //close the port
        private void closePort()
        {
             mSerialPort.Close();
            mSerialPort.Dispose();
            mBuffer = "";
        }

    }
}
