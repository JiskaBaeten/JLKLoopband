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
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

//here are some examples of how to use the class

/*EXAMPLE 1: Detailed code for relative mouse move event (move xPos 10px, yPos stays) */
/* int resSendInput;
            InputSimulator.INPUT inp = new InputSimulator.INPUT();
           // inp.type = 1;//set to keyboard
            inp.mi.dx = 10;
            inp.mi.dy = 0;
            inp.mi.dwFlags = InputSimulator.MOUSEEVENTF_MOVE;

            //inp.dwExtraInfo = MOUSEEVENTF_RELATIVE;
            resSendInput = InputSimulator.SendInput(1, ref inp,Marshal.SizeOf(inp));
*********************************************************/



/*EXAMPLE 2: CODE FOR KEYPRESS (down and up event) USING PREDEFINED FUNCTION*/
//  InputSimulator.doKeyPress((ushort)InputSimulator.Key.A);
/*********************************************************/

/* EXAMPLE 3: detailed CODE FOR KEY PRESS (UP AND DOWN) IN DETAIL*/
/*            
          // create the structure that contains the details necessary to define input
            InputSimulator.INPUT keyInput = new InputSimulator.INPUT();
            keyInput.type = 1; // key press
            keyInput.ki.wVk = 0x41; //or Key.A

            keyInput.ki.wScan = 0;
            keyInput.ki.time = 0;  // do it now
            // keyInput.ki.dwFlags = InputSimulator.(int)KeyEvent.KeyDown;//this is default so you do not have to set it
            // keyInput.ki.dwExtraInfo = (uint)GetMessageExtraInfo();

            //send the key down event to the OS
            InputSimulator.SendInput(1, ref keyInput, Marshal.SizeOf(keyInput));

            //switch KeyDown flat to Key up .
            keyInput.ki.dwFlags = 2;// (int)KeyEvent.KeyUp;

            //send the key up event to the OS
            InputSimulator.SendInput(1, ref keyInput, Marshal.SizeOf(keyInput));
*/
/*********************************************************/


namespace KdGSerialPortManager
{
    class InputSimulator
    {
        //define Sendinput function
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

        //define all constants that can be used for the INPUT parameter
       public const int INPUT_MOUSE = 0;
       public const int INPUT_KEYBOARD = 1;
       public const int INPUT_HARDWARE = 2;
       public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
       public const uint KEYEVENTF_KEYUP = 0x0002;
       public const uint KEYEVENTF_UNICODE = 0x0004;
       public const uint KEYEVENTF_SCANCODE = 0x0008;
       public const uint XBUTTON1 = 0x0001;
       public const uint XBUTTON2 = 0x0002;
       public const uint MOUSEEVENTF_MOVE = 0x0001;
       public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
       public const uint MOUSEEVENTF_LEFTUP = 0x0004;
       public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
       public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
       public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
       public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
       public const uint MOUSEEVENTF_XDOWN = 0x0080;
       public const uint MOUSEEVENTF_XUP = 0x0100;
       public const uint MOUSEEVENTF_WHEEL = 0x0800;
       public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
       public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        //define all possible keys in the Key struct so we can use key names
        //instead of key codes
        public enum Key : ushort
        {
            SHIFT = 0x10,
            CONTROL = 0x11,
            MENU = 0x12,
            ESCAPE = 0x1B,
            BACK = 0x08,
            TAB = 0x09,
            RETURN = 0x0D,
            PRIOR = 0x21,
            NEXT = 0x22,
            END = 0x23,
            HOME = 0x24,
            LEFT = 0x25,
            UP = 0x26,
            RIGHT = 0x27,
            DOWN = 0x28,
            SELECT = 0x29,
            PRINT = 0x2A,
            EXECUTE = 0x2B,
            SNAPSHOT = 0x2C,
            INSERT = 0x2D,
            DELETE = 0x2E,
            HELP = 0x2F,
            NUMPAD0 = 0x60,
            NUMPAD1 = 0x61,
            NUMPAD2 = 0x62,
            NUMPAD3 = 0x63,
            NUMPAD4 = 0x64,
            NUMPAD5 = 0x65,
            NUMPAD6 = 0x66,
            NUMPAD7 = 0x67,
            NUMPAD8 = 0x68,
            NUMPAD9 = 0x69,
            MULTIPLY = 0x6A,
            ADD = 0x6B,
            SEPARATOR = 0x6C,
            SUBTRACT = 0x6D,
            DECIMAL = 0x6E,
            DIVIDE = 0x6F,
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            OEM_1 = 0xBA,   // ',:' for US
            OEM_PLUS = 0xBB,   // '+' any country
            OEM_COMMA = 0xBC,   // ',' any country
            OEM_MINUS = 0xBD,   // '-' any country
            OEM_PERIOD = 0xBE,   // '.' any country
            OEM_2 = 0xBF,   // '/?' for US
            OEM_3 = 0xC0,   // '`~' for US
            MEDIA_NEXT_TRACK = 0xB0,
            MEDIA_PREV_TRACK = 0xB1,
            MEDIA_STOP = 0xB2,
            MEDIA_PLAY_PAUSE = 0xB3,
            LWIN = 0x5B,
            RWIN = 0x5C,
            A = 0x41,
            B = 0x42,
            C = 0x43,
            D = 0x44,
            E = 0x45,
            F = 0x46,
            G = 0x47,
            H = 0x48,
            I = 0x49,
            J = 0x4A,
            K = 0x4B,
            L = 0x4C,
            M = 0x4D,
            N = 0x4E,
            O = 0x5F,
            P = 0x50,
            Q = 0x51,
            R = 0x52,
            S = 0x53,
            T = 0x54,
            U = 0x55,
            V = 0x56,
            W = 0x57,
            X = 0x58,
            Y = 0x59,
            Z = 0x5A,
        }

        //define KeyEvents so we can use names instead of codes
        private enum KeyEvent
        {
            KeyUp = 0x0002,
            KeyDown = 0x0000,
            ExtendedKey = 0x0001
        }


        // define MOUSEINPUT structure
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        // define KEYBOARDINPUT structure
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        //  define HARDWAREINPUT structure
        public struct HARDWAREINPUT
        {
            int uMsg;
            short wParamL;
            short wParamH;
        }

        //define INPUT structure
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }

        //function that can be used to emulate a single key stroke (down and up again)
        public static void doKeyPress(ushort key)
        {
            InputSimulator.INPUT keyInput = new InputSimulator.INPUT();
            keyInput.type = 1;
            keyInput.ki.wVk = key;

            keyInput.ki.wScan = 0;
            keyInput.ki.time = 0;
            // keyInput.ki.dwFlags = InputSimulator.(int)KeyEvent.KeyDown;
            // keyInput.ki.dwExtraInfo = (uint)GetMessageExtraInfo();

            InputSimulator.SendInput(1, ref keyInput, System.Runtime.InteropServices.Marshal.SizeOf(keyInput));

            //Key up the key.
            keyInput.ki.dwFlags = 2;// (int)KeyEvent.KeyUp;

            InputSimulator.SendInput(1, ref keyInput, System.Runtime.InteropServices.Marshal.SizeOf(keyInput));
        }
    }
}
