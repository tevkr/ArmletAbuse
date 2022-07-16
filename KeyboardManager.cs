using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace ArmletAbuse
{
    internal class KeyboardManager
    {
        private static readonly List<Tuple<string, ScanCodeShort>> _dota2Buttons = new List<Tuple<string, ScanCodeShort>>()
        {
            // NOT ALLOWED: BACKSPACE, CAPSLOCK, SCROLLOCK, LEFT, RIGHT, DOWN, PRINTSCREEN, NUMPAD*, NUMPAD-, NUMPAD/, NUMPADENTER, MWHEELUP, MWHEELDOWN
            new Tuple<string, ScanCodeShort>("MOUSE3", ScanCodeShort.MBUTTON),
            new Tuple<string, ScanCodeShort>("MOUSE4", ScanCodeShort.XBUTTON1),
            new Tuple<string, ScanCodeShort>("MOUSE5", ScanCodeShort.XBUTTON2),
            new Tuple<string, ScanCodeShort>("TAB", ScanCodeShort.TAB),
            new Tuple<string, ScanCodeShort>("ENTER", ScanCodeShort.RETURN),
            new Tuple<string, ScanCodeShort>("LWIN", ScanCodeShort.LWIN),
            new Tuple<string, ScanCodeShort>("RWIN", ScanCodeShort.RWIN),
            new Tuple<string, ScanCodeShort>("NUMLOCK", ScanCodeShort.NUMLOCK),
            new Tuple<string, ScanCodeShort>("PAUSE", ScanCodeShort.PAUSE),
            new Tuple<string, ScanCodeShort>("END", ScanCodeShort.END),
            new Tuple<string, ScanCodeShort>("HOME", ScanCodeShort.HOME),
            new Tuple<string, ScanCodeShort>("SPACE", ScanCodeShort.SPACE),
            new Tuple<string, ScanCodeShort>("PGUP", ScanCodeShort.PRIOR),
            new Tuple<string, ScanCodeShort>("PGDN", ScanCodeShort.NEXT),
            new Tuple<string, ScanCodeShort>("UPARROW", ScanCodeShort.UP),
            new Tuple<string, ScanCodeShort>("INS", ScanCodeShort.INSERT),
            new Tuple<string, ScanCodeShort>("DEL", ScanCodeShort.DELETE),
            new Tuple<string, ScanCodeShort>("0", ScanCodeShort.KEY_0),
            new Tuple<string, ScanCodeShort>("1", ScanCodeShort.KEY_1),
            new Tuple<string, ScanCodeShort>("2", ScanCodeShort.KEY_2),
            new Tuple<string, ScanCodeShort>("3", ScanCodeShort.KEY_3),
            new Tuple<string, ScanCodeShort>("4", ScanCodeShort.KEY_4),
            new Tuple<string, ScanCodeShort>("5", ScanCodeShort.KEY_5),
            new Tuple<string, ScanCodeShort>("6", ScanCodeShort.KEY_6),
            new Tuple<string, ScanCodeShort>("7", ScanCodeShort.KEY_7),
            new Tuple<string, ScanCodeShort>("8", ScanCodeShort.KEY_8),
            new Tuple<string, ScanCodeShort>("9", ScanCodeShort.KEY_9),
            new Tuple<string, ScanCodeShort>("a", ScanCodeShort.KEY_A),
            new Tuple<string, ScanCodeShort>("b", ScanCodeShort.KEY_B),
            new Tuple<string, ScanCodeShort>("c", ScanCodeShort.KEY_C),
            new Tuple<string, ScanCodeShort>("d", ScanCodeShort.KEY_D),
            new Tuple<string, ScanCodeShort>("e", ScanCodeShort.KEY_E),
            new Tuple<string, ScanCodeShort>("f", ScanCodeShort.KEY_F),
            new Tuple<string, ScanCodeShort>("g", ScanCodeShort.KEY_G),
            new Tuple<string, ScanCodeShort>("h", ScanCodeShort.KEY_H),
            new Tuple<string, ScanCodeShort>("i", ScanCodeShort.KEY_I),
            new Tuple<string, ScanCodeShort>("j", ScanCodeShort.KEY_J),
            new Tuple<string, ScanCodeShort>("k", ScanCodeShort.KEY_K),
            new Tuple<string, ScanCodeShort>("l", ScanCodeShort.KEY_L),
            new Tuple<string, ScanCodeShort>("m", ScanCodeShort.KEY_M),
            new Tuple<string, ScanCodeShort>("n", ScanCodeShort.KEY_N),
            new Tuple<string, ScanCodeShort>("o", ScanCodeShort.KEY_O),
            new Tuple<string, ScanCodeShort>("p", ScanCodeShort.KEY_P),
            new Tuple<string, ScanCodeShort>("q", ScanCodeShort.KEY_Q),
            new Tuple<string, ScanCodeShort>("r", ScanCodeShort.KEY_R),
            new Tuple<string, ScanCodeShort>("s", ScanCodeShort.KEY_S),
            new Tuple<string, ScanCodeShort>("t", ScanCodeShort.KEY_T),
            new Tuple<string, ScanCodeShort>("u", ScanCodeShort.KEY_U),
            new Tuple<string, ScanCodeShort>("v", ScanCodeShort.KEY_V),
            new Tuple<string, ScanCodeShort>("w", ScanCodeShort.KEY_W),
            new Tuple<string, ScanCodeShort>("x", ScanCodeShort.KEY_X),
            new Tuple<string, ScanCodeShort>("y", ScanCodeShort.KEY_Y),
            new Tuple<string, ScanCodeShort>("z", ScanCodeShort.KEY_Z),
            new Tuple<string, ScanCodeShort>("KP_0", ScanCodeShort.NUMPAD0),
            new Tuple<string, ScanCodeShort>("KP_1", ScanCodeShort.NUMPAD1),
            new Tuple<string, ScanCodeShort>("KP_2", ScanCodeShort.NUMPAD2),
            new Tuple<string, ScanCodeShort>("KP_3", ScanCodeShort.NUMPAD3),
            new Tuple<string, ScanCodeShort>("KP_4", ScanCodeShort.NUMPAD4),
            new Tuple<string, ScanCodeShort>("KP_5", ScanCodeShort.NUMPAD5),
            new Tuple<string, ScanCodeShort>("KP_6", ScanCodeShort.NUMPAD6),
            new Tuple<string, ScanCodeShort>("KP_7", ScanCodeShort.NUMPAD7),
            new Tuple<string, ScanCodeShort>("KP_8", ScanCodeShort.NUMPAD8),
            new Tuple<string, ScanCodeShort>("KP_9", ScanCodeShort.NUMPAD9),
            new Tuple<string, ScanCodeShort>("KP_PLUS", ScanCodeShort.ADD),
            new Tuple<string, ScanCodeShort>("KP_DEL", ScanCodeShort.DIVIDE),
            new Tuple<string, ScanCodeShort>("F1", ScanCodeShort.F1),
            new Tuple<string, ScanCodeShort>("F2", ScanCodeShort.F2),
            new Tuple<string, ScanCodeShort>("F3", ScanCodeShort.F3),
            new Tuple<string, ScanCodeShort>("F4", ScanCodeShort.F4),
            new Tuple<string, ScanCodeShort>("F5", ScanCodeShort.F5),
            new Tuple<string, ScanCodeShort>("F6", ScanCodeShort.F6),
            new Tuple<string, ScanCodeShort>("F7", ScanCodeShort.F7),
            new Tuple<string, ScanCodeShort>("F8", ScanCodeShort.F8),
            new Tuple<string, ScanCodeShort>("F9", ScanCodeShort.F9),
            new Tuple<string, ScanCodeShort>("F10", ScanCodeShort.F10),
            new Tuple<string, ScanCodeShort>("F11", ScanCodeShort.F11),
            new Tuple<string, ScanCodeShort>("F12", ScanCodeShort.F12),
            new Tuple<string, ScanCodeShort>("=", ScanCodeShort.OEM_PLUS),
            new Tuple<string, ScanCodeShort>(",", ScanCodeShort.OEM_COMMA),
            new Tuple<string, ScanCodeShort>("-", ScanCodeShort.OEM_MINUS),
            new Tuple<string, ScanCodeShort>(".", ScanCodeShort.OEM_PERIOD),
            new Tuple<string, ScanCodeShort>("/", ScanCodeShort.OEM_2),
            new Tuple<string, ScanCodeShort>("`", ScanCodeShort.OEM_3),
            new Tuple<string, ScanCodeShort>("[", ScanCodeShort.OEM_4),
            new Tuple<string, ScanCodeShort>("\\", ScanCodeShort.OEM_5),
            new Tuple<string, ScanCodeShort>("]", ScanCodeShort.OEM_6),
            new Tuple<string, ScanCodeShort>("'", ScanCodeShort.OEM_7)
        };
        private static ScanCodeShort FindButtonScanCodeShort(string dota2Button)
        {
            Tuple<string, ScanCodeShort> keyCode = _dota2Buttons.Find(button => button.Item1 == dota2Button);
            if (keyCode == null) return ScanCodeShort.ERROR;
            return keyCode.Item2;
        }
        public static void PressButton(string dota2Button, int count)
        {
            ScanCodeShort buttonScanCodeShort = FindButtonScanCodeShort(dota2Button);
            if (buttonScanCodeShort == ScanCodeShort.ERROR) return;
            List<INPUT> inputs = new List<INPUT>();
            INPUT input = new INPUT();
            if (buttonScanCodeShort == 0)
            {
                if (dota2Button.Contains("3"))
                {
                    input.type = 0;
                    input.U.mi.dwFlags = MOUSEEVENTF.MIDDLEDOWN;
                    inputs.Add(input);
                    input.type = 0;
                    input.U.mi.dwFlags = MOUSEEVENTF.MIDDLEUP;
                    inputs.Add(input);
                }
                else if (dota2Button.Contains("4"))
                {
                    input.type = 0;
                    input.U.mi.mouseData = MouseEventDataXButtons.XBUTTON1;
                    input.U.mi.dwFlags = MOUSEEVENTF.XDOWN;
                    inputs.Add(input);
                    input.type = 0;
                    input.U.mi.mouseData = MouseEventDataXButtons.XBUTTON1;
                    input.U.mi.dwFlags = MOUSEEVENTF.XUP;
                    inputs.Add(input);
                }
                else if (dota2Button.Contains("5"))
                {
                    input.type = 0;
                    input.U.mi.mouseData = MouseEventDataXButtons.XBUTTON2;
                    input.U.mi.dwFlags = MOUSEEVENTF.XDOWN;
                    inputs.Add(input);
                    input.type = 0;
                    input.U.mi.mouseData = MouseEventDataXButtons.XBUTTON2;
                    input.U.mi.dwFlags = MOUSEEVENTF.XUP;
                    inputs.Add(input);
                }
            }
            else
            {
                input.type = 1;
                input.U.ki.wScan = buttonScanCodeShort;
                inputs.Add(input);
                input.type = 1;
                input.U.ki.wScan = buttonScanCodeShort;
                input.U.ki.dwFlags = KEYEVENTF.KEYUP;
                inputs.Add(input);
            }
            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                    inputs.Add(inputs[0]);
                else
                    inputs.Add(inputs[1]);
            }
            SendInput(2 * (uint)count, inputs.ToArray(), INPUT.Size);
        }
        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs,[MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,int cbSize);
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            internal uint type;
            internal InputUnion U;
            internal static int Size
            {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }
        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion
        {
            [FieldOffset(0)]
            internal MOUSEINPUT mi;
            [FieldOffset(0)]
            internal KEYBDINPUT ki;
            [FieldOffset(0)]
            internal HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            internal int dx;
            internal int dy;
            internal MouseEventDataXButtons mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }
        [Flags]
        internal enum MouseEventDataXButtons : uint
        {
            Nothing = 0x00000000,
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }
        [Flags]
        internal enum MOUSEEVENTF : uint
        {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            internal VirtualKeyShort wVk;
            internal ScanCodeShort wScan;
            internal KEYEVENTF dwFlags;
            internal int time;
            internal UIntPtr dwExtraInfo;
        }
        [Flags]
        internal enum KEYEVENTF : uint
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }
        internal enum VirtualKeyShort : short
        {
            LBUTTON = 0x01,
            RBUTTON = 0x02,
            CANCEL = 0x03,
            MBUTTON = 0x04,
            XBUTTON1 = 0x05,
            XBUTTON2 = 0x06,
            BACK = 0x08,
            TAB = 0x09,
            CLEAR = 0x0C,
            RETURN = 0x0D,
            SHIFT = 0x10,
            CONTROL = 0x11,
            MENU = 0x12,
            PAUSE = 0x13,
            CAPITAL = 0x14,
            KANA = 0x15,
            HANGUL = 0x15,
            JUNJA = 0x17,
            FINAL = 0x18,
            HANJA = 0x19,
            KANJI = 0x19,
            ESCAPE = 0x1B,
            CONVERT = 0x1C,
            NONCONVERT = 0x1D,
            ACCEPT = 0x1E,
            MODECHANGE = 0x1F,
            SPACE = 0x20,
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
            KEY_0 = 0x30,
            KEY_1 = 0x31,
            KEY_2 = 0x32,
            KEY_3 = 0x33,
            KEY_4 = 0x34,
            KEY_5 = 0x35,
            KEY_6 = 0x36,
            KEY_7 = 0x37,
            KEY_8 = 0x38,
            KEY_9 = 0x39,
            KEY_A = 0x41,
            KEY_B = 0x42,
            KEY_C = 0x43,
            KEY_D = 0x44,
            KEY_E = 0x45,
            KEY_F = 0x46,
            KEY_G = 0x47,
            KEY_H = 0x48,
            KEY_I = 0x49,
            KEY_J = 0x4A,
            KEY_K = 0x4B,
            KEY_L = 0x4C,
            KEY_M = 0x4D,
            KEY_N = 0x4E,
            KEY_O = 0x4F,
            KEY_P = 0x50,
            KEY_Q = 0x51,
            KEY_R = 0x52,
            KEY_S = 0x53,
            KEY_T = 0x54,
            KEY_U = 0x55,
            KEY_V = 0x56,
            KEY_W = 0x57,
            KEY_X = 0x58,
            KEY_Y = 0x59,
            KEY_Z = 0x5A,
            LWIN = 0x5B,
            RWIN = 0x5C,
            APPS = 0x5D,
            SLEEP = 0x5F,
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
            F13 = 0x7C,
            F14 = 0x7D,
            F15 = 0x7E,
            F16 = 0x7F,
            F17 = 0x80,
            F18 = 0x81,
            F19 = 0x82,
            F20 = 0x83,
            F21 = 0x84,
            F22 = 0x85,
            F23 = 0x86,
            F24 = 0x87,
            NUMLOCK = 0x90,
            SCROLL = 0x91,
            LSHIFT = 0xA0,
            RSHIFT = 0xA1,
            LCONTROL = 0xA2,
            RCONTROL = 0xA3,
            LMENU = 0xA4,
            RMENU = 0xA5,
            BROWSER_BACK = 0xA6,
            BROWSER_FORWARD = 0xA7,
            BROWSER_REFRESH = 0xA8,
            BROWSER_STOP = 0xA9,
            BROWSER_SEARCH = 0xAA,
            BROWSER_FAVORITES = 0xAB,
            BROWSER_HOME = 0xAC,
            VOLUME_MUTE = 0xAD,
            VOLUME_DOWN = 0xAE,
            VOLUME_UP = 0xAF,
            MEDIA_NEXT_TRACK = 0xB0,
            MEDIA_PREV_TRACK = 0xB1,
            MEDIA_STOP = 0xB2,
            MEDIA_PLAY_PAUSE = 0xB3,
            LAUNCH_MAIL = 0xB4,
            LAUNCH_MEDIA_SELECT = 0xB5,
            LAUNCH_APP1 = 0xB6,
            LAUNCH_APP2 = 0xB7,
            OEM_1 = 0xBA,
            OEM_PLUS = 0xBB,
            OEM_COMMA = 0xBC,
            OEM_MINUS = 0xBD,
            OEM_PERIOD = 0xBE,
            OEM_2 = 0xBF,
            OEM_3 = 0xC0,
            OEM_4 = 0xDB,
            OEM_5 = 0xDC,
            OEM_6 = 0xDD,
            OEM_7 = 0xDE,
            OEM_8 = 0xDF,
            OEM_102 = 0xE2,
            PROCESSKEY = 0xE5,
            PACKET = 0xE7,
            ATTN = 0xF6,
            CRSEL = 0xF7,
            EXSEL = 0xF8,
            EREOF = 0xF9,
            PLAY = 0xFA,
            ZOOM = 0xFB,
            NONAME = 0xFC,
            PA1 = 0xFD,
            OEM_CLEAR = 0xFE
        }

        internal enum ScanCodeShort : short
        {
            ERROR = -1,
            LBUTTON = 0,
            RBUTTON = 0,
            CANCEL = 70,
            MBUTTON = 0,
            XBUTTON1 = 0,
            XBUTTON2 = 0,
            BACK = 14,
            TAB = 15,
            CLEAR = 76,
            RETURN = 28,
            SHIFT = 42,
            CONTROL = 29,
            MENU = 56,
            PAUSE = 0,
            CAPITAL = 58,
            KANA = 0,
            HANGUL = 0,
            JUNJA = 0,
            FINAL = 0,
            HANJA = 0,
            KANJI = 0,
            ESCAPE = 1,
            CONVERT = 0,
            NONCONVERT = 0,
            ACCEPT = 0,
            MODECHANGE = 0,
            SPACE = 57,
            PRIOR = 73,
            NEXT = 81,
            END = 79,
            HOME = 71,
            LEFT = 75,
            UP = 72,
            RIGHT = 77,
            DOWN = 80,
            SELECT = 0,
            PRINT = 0,
            EXECUTE = 0,
            SNAPSHOT = 84,
            INSERT = 82,
            DELETE = 83,
            HELP = 99,
            KEY_0 = 11,
            KEY_1 = 2,
            KEY_2 = 3,
            KEY_3 = 4,
            KEY_4 = 5,
            KEY_5 = 6,
            KEY_6 = 7,
            KEY_7 = 8,
            KEY_8 = 9,
            KEY_9 = 10,
            KEY_A = 30,
            KEY_B = 48,
            KEY_C = 46,
            KEY_D = 32,
            KEY_E = 18,
            KEY_F = 33,
            KEY_G = 34,
            KEY_H = 35,
            KEY_I = 23,
            KEY_J = 36,
            KEY_K = 37,
            KEY_L = 38,
            KEY_M = 50,
            KEY_N = 49,
            KEY_O = 24,
            KEY_P = 25,
            KEY_Q = 16,
            KEY_R = 19,
            KEY_S = 31,
            KEY_T = 20,
            KEY_U = 22,
            KEY_V = 47,
            KEY_W = 17,
            KEY_X = 45,
            KEY_Y = 21,
            KEY_Z = 44,
            LWIN = 91,
            RWIN = 92,
            APPS = 93,
            SLEEP = 95,
            NUMPAD0 = 82,
            NUMPAD1 = 79,
            NUMPAD2 = 80,
            NUMPAD3 = 81,
            NUMPAD4 = 75,
            NUMPAD5 = 76,
            NUMPAD6 = 77,
            NUMPAD7 = 71,
            NUMPAD8 = 72,
            NUMPAD9 = 73,
            MULTIPLY = 55,
            ADD = 78,
            SEPARATOR = 0,
            SUBTRACT = 74,
            DECIMAL = 83,
            DIVIDE = 53,
            F1 = 59,
            F2 = 60,
            F3 = 61,
            F4 = 62,
            F5 = 63,
            F6 = 64,
            F7 = 65,
            F8 = 66,
            F9 = 67,
            F10 = 68,
            F11 = 87,
            F12 = 88,
            F13 = 100,
            F14 = 101,
            F15 = 102,
            F16 = 103,
            F17 = 104,
            F18 = 105,
            F19 = 106,
            F20 = 107,
            F21 = 108,
            F22 = 109,
            F23 = 110,
            F24 = 118,
            NUMLOCK = 69,
            SCROLL = 70,
            LSHIFT = 42,
            RSHIFT = 54,
            LCONTROL = 29,
            RCONTROL = 29,
            LMENU = 56,
            RMENU = 56,
            BROWSER_BACK = 106,
            BROWSER_FORWARD = 105,
            BROWSER_REFRESH = 103,
            BROWSER_STOP = 104,
            BROWSER_SEARCH = 101,
            BROWSER_FAVORITES = 102,
            BROWSER_HOME = 50,
            VOLUME_MUTE = 32,
            VOLUME_DOWN = 46,
            VOLUME_UP = 48,
            MEDIA_NEXT_TRACK = 25,
            MEDIA_PREV_TRACK = 16,
            MEDIA_STOP = 36,
            MEDIA_PLAY_PAUSE = 34,
            LAUNCH_MAIL = 108,
            LAUNCH_MEDIA_SELECT = 109,
            LAUNCH_APP1 = 107,
            LAUNCH_APP2 = 33,
            OEM_1 = 39,
            OEM_PLUS = 13,
            OEM_COMMA = 51,
            OEM_MINUS = 12,
            OEM_PERIOD = 52,
            OEM_2 = 53,
            OEM_3 = 41,
            OEM_4 = 26,
            OEM_5 = 43,
            OEM_6 = 27,
            OEM_7 = 40,
            OEM_8 = 0,
            OEM_102 = 86,
            PROCESSKEY = 0,
            PACKET = 0,
            ATTN = 0,
            CRSEL = 0,
            EXSEL = 0,
            EREOF = 93,
            PLAY = 0,
            ZOOM = 98,
            NONAME = 0,
            PA1 = 0,
            OEM_CLEAR = 0,
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }
    }
}
