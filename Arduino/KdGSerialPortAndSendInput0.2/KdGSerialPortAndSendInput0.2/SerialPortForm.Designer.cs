namespace KdGSerialPortManager
{
    partial class SerialPortForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label6 = new System.Windows.Forms.Label();
            this.radioText = new System.Windows.Forms.RadioButton();
            this.RadioBinary = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.dataBitsList = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReceiveBytes = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.stopBitsSelection = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.paritySelection = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.baudRateSelection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.portNameSelection = new System.Windows.Forms.ComboBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.mSerialPort = new System.IO.Ports.SerialPort(this.components);
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Mode:";
            // 
            // radioText
            // 
            this.radioText.AutoSize = true;
            this.radioText.Location = new System.Drawing.Point(47, 175);
            this.radioText.Name = "radioText";
            this.radioText.Size = new System.Drawing.Size(46, 17);
            this.radioText.TabIndex = 12;
            this.radioText.TabStop = true;
            this.radioText.Text = "Text";
            this.radioText.UseVisualStyleBackColor = true;
            // 
            // RadioBinary
            // 
            this.RadioBinary.AutoSize = true;
            this.RadioBinary.Location = new System.Drawing.Point(47, 159);
            this.RadioBinary.Name = "RadioBinary";
            this.RadioBinary.Size = new System.Drawing.Size(54, 17);
            this.RadioBinary.TabIndex = 11;
            this.RadioBinary.TabStop = true;
            this.RadioBinary.Text = "Binary";
            this.RadioBinary.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Databits:";
            // 
            // dataBitsList
            // 
            this.dataBitsList.FormattingEnabled = true;
            this.dataBitsList.Location = new System.Drawing.Point(70, 127);
            this.dataBitsList.Name = "dataBitsList";
            this.dataBitsList.Size = new System.Drawing.Size(91, 21);
            this.dataBitsList.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtReceiveBytes);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.radioText);
            this.groupBox1.Controls.Add(this.RadioBinary);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dataBitsList);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.stopBitsSelection);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.paritySelection);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.baudRateSelection);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.portNameSelection);
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 263);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "COM Port settings";
            // 
            // txtReceiveBytes
            // 
            this.txtReceiveBytes.Location = new System.Drawing.Point(90, 199);
            this.txtReceiveBytes.Mask = "00000";
            this.txtReceiveBytes.Name = "txtReceiveBytes";
            this.txtReceiveBytes.Size = new System.Drawing.Size(71, 20);
            this.txtReceiveBytes.TabIndex = 16;
            this.txtReceiveBytes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtReceiveBytes.ValidatingType = typeof(int);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 208);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "receiveBytes:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "bytes to trigger";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Stop bits:";
            // 
            // stopBitsSelection
            // 
            this.stopBitsSelection.FormattingEnabled = true;
            this.stopBitsSelection.Location = new System.Drawing.Point(70, 100);
            this.stopBitsSelection.Name = "stopBitsSelection";
            this.stopBitsSelection.Size = new System.Drawing.Size(91, 21);
            this.stopBitsSelection.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Parity:";
            // 
            // paritySelection
            // 
            this.paritySelection.FormattingEnabled = true;
            this.paritySelection.Location = new System.Drawing.Point(49, 73);
            this.paritySelection.Name = "paritySelection";
            this.paritySelection.Size = new System.Drawing.Size(112, 21);
            this.paritySelection.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baud rate:";
            // 
            // baudRateSelection
            // 
            this.baudRateSelection.FormattingEnabled = true;
            this.baudRateSelection.Location = new System.Drawing.Point(70, 46);
            this.baudRateSelection.Name = "baudRateSelection";
            this.baudRateSelection.Size = new System.Drawing.Size(91, 21);
            this.baudRateSelection.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port:";
            // 
            // portNameSelection
            // 
            this.portNameSelection.FormattingEnabled = true;
            this.portNameSelection.Location = new System.Drawing.Point(40, 19);
            this.portNameSelection.Name = "portNameSelection";
            this.portNameSelection.Size = new System.Drawing.Size(121, 21);
            this.portNameSelection.TabIndex = 1;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(12, 272);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(151, 23);
            this.buttonOpen.TabIndex = 2;
            this.buttonOpen.Text = "Open COM port";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // mSerialPort
            // 
            this.mSerialPort.PortName = "COM4";
            this.mSerialPort.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.mSerialPort_ErrorReceived);
            this.mSerialPort.PinChanged += new System.IO.Ports.SerialPinChangedEventHandler(this.mSerialPort_PinChanged);
            this.mSerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.mSerialPort_DataReceived);
            // 
            // outputTextBox
            // 
            this.outputTextBox.Location = new System.Drawing.Point(179, 3);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(363, 292);
            this.outputTextBox.TabIndex = 4;
            this.outputTextBox.Text = "";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(12, 301);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(151, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close COM port";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(179, 301);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(52, 23);
            this.buttonSend.TabIndex = 6;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(238, 304);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(304, 20);
            this.inputBox.TabIndex = 7;
            // 
            // SerialPortForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 332);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SerialPortForm";
            this.Text = "@";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioText;
        private System.Windows.Forms.RadioButton RadioBinary;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox dataBitsList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox stopBitsSelection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox paritySelection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox baudRateSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox portNameSelection;
        private System.Windows.Forms.Button buttonOpen;
        private System.IO.Ports.SerialPort mSerialPort;
        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.MaskedTextBox txtReceiveBytes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;

    }
}

