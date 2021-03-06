﻿using System;
using System.Threading;
using MindSqueezer.Utilities;

namespace MindSqueezer
{
    class Timer
    {
        private static Thread _inputThread;
        private static string _input;
        private static bool _gotInput;

        private static void Reader()
        {
            while (!_gotInput)
            {
                _input = Console.ReadLine();
                _gotInput = true;            
            }
        }
      
        public static bool TryReadLine(out string line, int timeOutMillisecs = Timeout.Infinite)
        {         
            _inputThread = new Thread(Reader);
            _gotInput = false;
            _inputThread.IsBackground = true;
            _inputThread.Start();
          
            for (int i = timeOutMillisecs; i >= 0 ; i -= 500)
            {
                if (_gotInput)
                {
                    break;
                }

                var cursorTop = Console.CursorTop;
                var cursorLeft = Console.CursorLeft;
                Console.SetCursorPosition(0, 0);
               
                ColorChanger.ChangeColor(ConsoleColor.Gray, ConsoleColor.Black);
                Writer.WriteMessage("You have ");
                ColorChanger.ChangeColor(ConsoleColor.Red, ConsoleColor.Black);
                Writer.WriteMessage($"{i / 1000:D2}");
                ColorChanger.ChangeColor(ConsoleColor.Gray, ConsoleColor.Black);
                Writer.WriteMessage(" seconds to answer.");
                ColorChanger.DefaultColor();

                Console.SetCursorPosition(cursorLeft, cursorTop);
                Thread.Sleep(500);
            }

            if (_gotInput)
            {
                line = _input;
                _inputThread.Abort();
            }
            else
            {
                line = null;
                _inputThread.Abort();
            }               
            return _gotInput;
        }

    }
}
