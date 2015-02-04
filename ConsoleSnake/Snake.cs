using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSnake
{
    class Snake
    {
        public char Richtung { get; set; }
        public bool Gezeichnet { get; set; }
        public int Pause { get; set; }
        public int Punkte { get; set; }
        public int stopremoving { get; set; }
        public string name { get; set; }
        public ConsoleColor color { get; set; }

        public ConsoleKey up { get; set; }
        public ConsoleKey down { get; set; }
        public ConsoleKey left { get; set; }
        public ConsoleKey right { get; set; }
        private List<string> SnakeQueue = new List<string>();
        public List<string> snakequeue
        {
            get { return SnakeQueue; }
            set { SnakeQueue = value; }
        }
        public Snake(char[,] Spielfeld, int initLaenge, int initPosX, int initPosY, ConsoleColor snakecolor)
        {
            for (int i = 0; i < initLaenge; i++)
            {
                snakequeue.Insert(0, String.Format("{0}|{1}", initPosX + i, initPosY));
            }
            color = snakecolor;
        }

    }
}
