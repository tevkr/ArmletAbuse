using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ArmletAbuse
{
    internal class ConsoleInfo
    {
        public ConsoleInfo()
        {
            Config.dota2OpenStateChanged += Dota2OpenStateChangedHandler;
            Config.enabledChanged += EnabledChangedHandler;
            Config.minHPChanged += MinHPChangedHandler;
            CursorPositions.console = new CursorPositions.Position(0, 9);
            CursorPositions.enabled = new CursorPositions.Position(11, 7);
            CursorPositions.minHP = new CursorPositions.Position(14, 8);
        }
        public Task Start()
        {
            return Task.Run(() =>
            {
                FormDota2Closed();
                UserInputReader();
            });
        }
        private void Dota2OpenStateChangedHandler(bool openState)
        {
            if (openState)
            {
                CursorPositions.console = new CursorPositions.Position(0, 21);
                FormDota2Opened();
            }
            else
            {
                CursorPositions.console = new CursorPositions.Position(0, 10);
                FormDota2Closed();
            }
        }
        private void EnabledChangedHandler(bool enabledState)
        {
            Console.SetCursorPosition(CursorPositions.enabled.left, CursorPositions.enabled.top);
            Console.Write(new string(' ', _blockWidth - "Enabled:   ##".Length));
            Console.SetCursorPosition(CursorPositions.enabled.left, CursorPositions.enabled.top);
            Console.Write(enabledState ? "True" : "False");
            Console.SetCursorPosition(CursorPositions.console.left, CursorPositions.console.top);
        }
        private void MinHPChangedHandler(int minHPState)
        {
            Console.SetCursorPosition(CursorPositions.minHP.left, CursorPositions.minHP.top);
            Console.Write(new string(' ', _blockWidth - "Minimal HP:   ##".Length));
            Console.SetCursorPosition(CursorPositions.minHP.left, CursorPositions.minHP.top);
            Console.Write(minHPState);
            Console.SetCursorPosition(CursorPositions.console.left, CursorPositions.console.top);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void FormDota2Closed()
        {
            Console.Clear();
            CreateBlock(0, new List<string> {
                "CREDENTIALS",
                "ArmletAbuse v1.0.0.0",
                "Github URL: github.com/tevkr/ArmletAbuse",
                "nom prod." }, true, true, ConsoleColor.Red);
            CreateBlock(5, new List<string> {
                "STATE INFO",
                "Waiting for Dota2 process", }, true, true, ConsoleColor.Yellow);
            Console.SetCursorPosition(CursorPositions.console.left, CursorPositions.console.top);
            Console.ForegroundColor = ConsoleColor.White;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void FormDota2Opened()
        {
            Console.Clear();
            CreateBlock(0, new List<string> {
                "CREDENTIALS",
                "ArmletAbuse v1.0.0.0",
                "Github URL: github.com/tevkr/ArmletAbuse",
                "nom prod." }, true, true, ConsoleColor.Red);
            CreateBlock(5, new List<string> {
                "STATE INFO",
                "Enabled: " + (Config.enabled ? "True" : "False"),
                "Minimal HP: " + Config.minHP }, false, true, ConsoleColor.White);
            CreateBlock(9, new List<string> {
                "CONSOLE COMMANDS",
                "Supported commands:",
                "1) /enable - enables abuser",
                "",
                "2) /disable - disables abuser",
                "",
                "3) /setminhp [Integer] - sets the",
                "minimum HP for starting abuse",
                "Example /setminhp 350" }, false, true, ConsoleColor.White);
            CreateBlock(19, new List<string> {
                "CONSOLE" }, false, false, ConsoleColor.White);
            Console.SetCursorPosition(CursorPositions.console.left, CursorPositions.console.top);
            Console.ForegroundColor = ConsoleColor.White;
        }
        private void UserInputReader()
        {
            while(true)
            {
                string input = Console.ReadLine();
                if (input.StartsWith("/") && Config.isDota2Opened)
                {
                    List<string> inputValues = input.Split('/', ' ').ToList();
                    inputValues.RemoveAll(val => val.Length == 0);
                    if (inputValues.FirstOrDefault() == "enable" || 
                        inputValues.FirstOrDefault() == "en" || 
                        inputValues.FirstOrDefault() == "e")
                        Config.enabled = true;
                    else if (inputValues.FirstOrDefault() == "disable" || 
                             inputValues.FirstOrDefault() == "dis" || 
                             inputValues.FirstOrDefault() == "d")
                        Config.enabled = false;
                    else if ((inputValues.FirstOrDefault() == "setminhp" || 
                              inputValues.FirstOrDefault() == "set" || 
                              inputValues.FirstOrDefault() == "hp") && int.TryParse(inputValues.ElementAtOrDefault(1), out _))
                        Config.minHP = int.Parse(inputValues.ElementAtOrDefault(1));
                }
                for (int i = 0, row = CursorPositions.console.top; i < Console.WindowHeight - CursorPositions.console.top - 1; i++, row++)
                {
                    Console.SetCursorPosition(0, row);
                    Console.Write(new string(' ', Console.WindowWidth));
                }
                Console.SetCursorPosition(CursorPositions.console.left, CursorPositions.console.top);
            }
        }
        private static class CursorPositions
        {
            public class Position
            {
                public Position(int left, int top)
                {
                    this.left = left;
                    this.top = top;
                }
                public int left { get; set; }
                public int top { get; set; }
            }
            public static Position console { get; set; }
            public static Position minHP { get; set; }
            public static Position enabled { get; set; }
        }
        private static readonly int _blockWidth = 44;
        private static void CreateCorner(int startRow, int height)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            int endRow = startRow + height - 1;
            for (int row = startRow; row <= endRow; row++)
            {
                Console.SetCursorPosition(0, row);
                for (int col = 0; col < _blockWidth; col++)
                {
                    if (row > startRow && row < endRow && col > 0 && col < _blockWidth - 1)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("#");
                    }
                }
            }
        }
        private static void FillBlock(int startRow, List<string> rows, bool centered, ConsoleColor color)
        {
            int maxRowLen = _blockWidth - 4;
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0, row = startRow + 1; i < rows.Count; i++, row++)
            {
                if (rows[i].Length > maxRowLen)
                {
                    rows[i] = rows[i].Substring(0, maxRowLen);
                }
                int col = 2;
                if (centered || i == 0)
                {
                    col = (int)Math.Ceiling((_blockWidth - rows[i].Length) / (double)2);
                }
                Console.SetCursorPosition(col, row);
                Console.Write(rows[i]);
                Console.ForegroundColor = color;
            }

        }
        private static void CreateBlock(int startRow, List<string> rows, bool centered, bool closed, ConsoleColor color)
        {
            if (closed) CreateCorner(startRow, rows.Count + 2);
            FillBlock(startRow, rows, centered, color);
        }
    }
}
