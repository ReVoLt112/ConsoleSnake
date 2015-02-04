using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace ConsoleSnake
{
    public static class GlobaleVariablen
    {
        public static bool eaten { get; set; }
        public static int player { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            GlobaleVariablen.eaten = true;
            char[,] Spielfeld = new char[80, 25];
error:
            Console.Clear();
            Console.Write("Wieviele Spieler? ([1] / 2): ");
            try
            {
                GlobaleVariablen.player = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                GlobaleVariablen.player = 1;
            }
            switch (GlobaleVariablen.player)
            {
                default:
                    Console.WriteLine("Ungültige Auswahl!");
                    Thread.Sleep(1000);
                    goto error;
                case 1:
                    GlobaleVariablen.player = 1;
                    Snake mySnake = new Snake(Spielfeld, 2, 1, 2, ConsoleColor.DarkGreen);

                    mySnake.stopremoving = 0;
                    mySnake.Pause = 100;
                    mySnake.Punkte = 0;

                    mySnake.Richtung = 'E';
                    mySnake.Gezeichnet = false;

                    SetNameAndKeys(mySnake);

                    Console.CursorVisible = false;
                    initSpielfeld(Spielfeld);
                    drawSpielfeld(Spielfeld, GlobaleVariablen.player);
                    drawSnake(Spielfeld, mySnake);
                    bool Terminate = false;
                    while (!Terminate)
                    {
                        setPoints(mySnake.Punkte, mySnake);
                        spawnFood(Spielfeld);
                        if (Console.KeyAvailable)
                        {
                            KeyBoardListener(Console.ReadKey(true).Key, mySnake);
                        }
                        Terminate = move(Spielfeld, mySnake);
                        Thread.Sleep(mySnake.Pause);
                    }
                    //End of game / Game Over
                    Console.SetCursorPosition(35, 12);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("GAME OVER...");
                    Console.SetCursorPosition(30, 13);
                    Console.Write(String.Format("{0} reached: {1} points!", mySnake.name, mySnake.Punkte));
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.ReadLine();
                    break;
                case 2:
                    Snake myS1 = new Snake(Spielfeld, 2, 1, 2, ConsoleColor.DarkGreen);

                    myS1.stopremoving = 0;
                    myS1.Pause = 100;
                    myS1.Punkte = 0;

                    myS1.Richtung = 'E';
                    myS1.Gezeichnet = false;

                    SetNameAndKeys(myS1);

                    Snake myS2 = new Snake(Spielfeld, 2, 75, 20, ConsoleColor.Blue);

                    myS2.stopremoving = 0;
                    myS2.Pause = 100;
                    myS2.Punkte = 0;

                    myS2.Richtung = 'W';
                    myS2.Gezeichnet = false;

                    SetNameAndKeys(myS2);

                    Console.CursorVisible = false;
                    initSpielfeld(Spielfeld);
                    drawSpielfeld(Spielfeld, GlobaleVariablen.player);
                    drawSnake(Spielfeld, myS1);
                    bool Terminate1 = false;
                    bool Terminate2 = false;
                    while (!Terminate1 && !Terminate2)
                    {
                        //setPoints(myS1.Punkte, myS1);
                        spawnFood(Spielfeld);
                        if (Console.KeyAvailable)
                        {
                            ConsoleKey key = Console.ReadKey(true).Key;
                            if (key == myS1.up || key == myS1.down || key == myS1.left || key == myS1.right)
                            {
                                KeyBoardListener(key, myS1);
                            }
                            if (key == myS2.up || key == myS2.down || key == myS2.left || key == myS2.right)
                            {
                                KeyBoardListener(key, myS2);
                            }

                        }
                        Terminate1 = move(Spielfeld, myS1);
                        if (!Terminate1)
                        {
                            Terminate2 = move(Spielfeld, myS2);
                        }
                        Thread.Sleep(100);
                    }
                    //End of game / Game Over
                    if (Terminate1)
                    {
                        Console.SetCursorPosition(35, 12);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("GAME OVER...");
                        Console.SetCursorPosition(35, 13);
                        Console.Write(String.Format("{0} died!", myS1.name));
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.SetCursorPosition(35, 12);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("GAME OVER...");
                        Console.SetCursorPosition(35, 13);
                        Console.Write(String.Format("{0} died!", myS2.name));
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.ReadLine();
                    break;
            }


        }

        private static void SetNameAndKeys(Snake mySnake)
        {
            Console.Clear();
            Console.Write("Bitte geben Sie ihren Namen ein: ");
            mySnake.name = Console.ReadLine();

            Console.Write("Bitte drücken Sie die Taste für hoch: ");
            mySnake.up = Console.ReadKey(true).Key;
            Console.WriteLine(mySnake.up.ToString());
            Console.Write("Bitte drücken Sie die Taste für runter: ");
            mySnake.down = Console.ReadKey(true).Key;
            Console.WriteLine(mySnake.down.ToString());
            Console.Write("Bitte drücken Sie die Taste für links: ");
            mySnake.left = Console.ReadKey(true).Key;
            Console.WriteLine(mySnake.left.ToString());
            Console.Write("Bitte drücken Sie die Taste für rechts: ");
            mySnake.right = Console.ReadKey(true).Key;
            Console.WriteLine(mySnake.right.ToString());

            Thread.Sleep(1000);
        }

        private static bool move(char[,] Spielfeld, Snake snake)
        {
            bool returnvalue = false;
            string[] pos = snake.snakequeue[0].Split('|');
            switch (snake.Richtung)
            {
                case 'N':
                    if (Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) - 1] != '*' && Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) - 1] != 'O' && Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) - 1] != 'X')
                    {
                        if (Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) - 1] == '#')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 1;
                            snake.Punkte++;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        if (Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) - 1] == '+')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 5;
                            snake.Punkte += 5;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        snake.snakequeue.Insert(0, String.Format("{0}|{1}", Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) - 1));
                        Debug.WriteLine(String.Format("Snake insert @ {0}|{1}", Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) - 1));
                        if (snake.stopremoving == 0)
                        {
                            string[] pos2 = snake.snakequeue[snake.snakequeue.Count - 1].Split('|');
                            Spielfeld[Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1])] = '\0';
                            Console.SetCursorPosition(Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1]));
                            Console.Write(" ");
                            snake.snakequeue.RemoveAt(snake.snakequeue.Count - 1);
                        }
                        else
                        {
                            snake.stopremoving--;
                        }
                    }
                    else
                    {
                        returnvalue = true;
                    }
                    break;
                case 'E':
                    if (Spielfeld[Convert.ToInt32(pos[0]) + 1, Convert.ToInt32(pos[1])] != '*' && Spielfeld[Convert.ToInt32(pos[0]) + 1, Convert.ToInt32(pos[1])] != 'O' && Spielfeld[Convert.ToInt32(pos[0]) + 1, Convert.ToInt32(pos[1])] != 'X')
                    {
                        if (Spielfeld[Convert.ToInt32(pos[0]) + 1, Convert.ToInt32(pos[1])] == '#')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 1;
                            snake.Punkte++;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        if (Spielfeld[Convert.ToInt32(pos[0]) + 1, Convert.ToInt32(pos[1])] == '+')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 5;
                            snake.Punkte += 5;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        snake.snakequeue.Insert(0, String.Format("{0}|{1}", Convert.ToInt32(pos[0]) + 1, Convert.ToInt32(pos[1])));
                        Debug.WriteLine(String.Format("Snake insert @ {0}|{1}", Convert.ToInt32(pos[0]) + 1, Convert.ToInt32(pos[1])));
                        if (snake.stopremoving == 0)
                        {
                            string[] pos2 = snake.snakequeue[snake.snakequeue.Count - 1].Split('|');
                            Spielfeld[Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1])] = '\0';
                            Console.SetCursorPosition(Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1]));
                            Console.Write(" ");
                            snake.snakequeue.RemoveAt(snake.snakequeue.Count - 1);
                        }
                        else
                        {
                            snake.stopremoving--;
                        }
                    }
                    else
                    {
                        returnvalue = true;
                    }
                    break;
                case 'S':
                    if (Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) + 1] != '*' && Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) + 1] != 'O' && Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) + 1] != 'X')
                    {
                        if (Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) + 1] == '#')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 1;
                            snake.Punkte++;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        if (Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) + 1] == '+')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 5;
                            snake.Punkte += 5;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        snake.snakequeue.Insert(0, String.Format("{0}|{1}", Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) + 1));
                        Debug.WriteLine(String.Format("Snake insert @ {0}|{1}", Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]) + 1));
                        if (snake.stopremoving == 0)
                        {
                            string[] pos2 = snake.snakequeue[snake.snakequeue.Count - 1].Split('|');
                            Spielfeld[Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1])] = '\0';
                            Console.SetCursorPosition(Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1]));
                            Console.Write(" ");
                            snake.snakequeue.RemoveAt(snake.snakequeue.Count - 1);
                        }
                        else
                        {
                            snake.stopremoving--;
                        }
                    }
                    else
                    {
                        returnvalue = true;
                    }
                    break;
                case 'W':
                    if (Spielfeld[Convert.ToInt32(pos[0]) - 1, Convert.ToInt32(pos[1])] != '*' && Spielfeld[Convert.ToInt32(pos[0]) - 1, Convert.ToInt32(pos[1])] != 'O' && Spielfeld[Convert.ToInt32(pos[0]) - 1, Convert.ToInt32(pos[1])] != 'X')
                    {
                        if (Spielfeld[Convert.ToInt32(pos[0]) - 1, Convert.ToInt32(pos[1])] == '#')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 1;
                            snake.Punkte++;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        if (Spielfeld[Convert.ToInt32(pos[0]) - 1, Convert.ToInt32(pos[1])] == '+')
                        {
                            GlobaleVariablen.eaten = true;
                            snake.stopremoving = 5;
                            snake.Punkte += 5;
                            snake.Pause = Convert.ToInt32(Math.Round(Convert.ToDouble(snake.Pause) * 0.98, 0));
                        }
                        snake.snakequeue.Insert(0, String.Format("{0}|{1}", Convert.ToInt32(pos[0]) - 1, Convert.ToInt32(pos[1])));
                        Debug.WriteLine(String.Format("Snake insert @ {0}|{1}", Convert.ToInt32(pos[0]) - 1, Convert.ToInt32(pos[1])));
                        if (snake.stopremoving == 0)
                        {
                            string[] pos2 = snake.snakequeue[snake.snakequeue.Count - 1].Split('|');
                            Spielfeld[Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1])] = '\0';
                            Console.SetCursorPosition(Convert.ToInt32(pos2[0]), Convert.ToInt32(pos2[1]));
                            Console.Write(" ");
                            snake.snakequeue.RemoveAt(snake.snakequeue.Count - 1);
                        }
                        else
                        {
                            snake.stopremoving--;
                        }
                    }
                    else
                    {
                        returnvalue = true;
                    }
                    break;
            }
            if (!returnvalue) drawSnake(Spielfeld, snake);
            return returnvalue;

        }

        private static void setPoints(int punkte, Snake snake)
        {
            Console.SetCursorPosition(9, 0);
            Console.Write(punkte.ToString());
            Console.SetCursorPosition(26, 0);
            Console.Write(snake.Richtung);
            Console.SetCursorPosition(36, 0);
            Console.Write(100 - snake.Pause);
            Console.SetCursorPosition(0, 0);
        }

        private static void drawSnake(char[,] Spielfeld, Snake snake)
        {
            Console.ForegroundColor = snake.color;
            for (int i = snake.snakequeue.Count - 1; i >= 0; i--)
            {
                string[] pos = snake.snakequeue[i].Split('|');
                Console.SetCursorPosition(Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]));
                if (i == 0)
                {
                    Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1])] = 'X';
                    Console.Write("X");
                }
                else if (i == snake.snakequeue.Count - 1)
                {
                    Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1])] = 'O';
                    Console.Write("o");
                }
                else
                {
                    Spielfeld[Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1])] = 'O';
                    Console.Write("O");
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 0);
            snake.Gezeichnet = true;
            Debug.WriteLine(String.Format("Drawn: {0}", snake.Gezeichnet));
        }

        private static void drawSpielfeld(char[,] Spielfeld, int player)
        {
            Console.Clear();
            for (int y = 0; y <= Spielfeld.GetLength(1) - 1; y++)
            {
                for (int x = 0; x <= Spielfeld.GetLength(0) - 1; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(Spielfeld[x, y]);
                }
            }

            switch (player)
            {
                case 1:
                    Console.SetCursorPosition(1, 0);
                    Console.Write("Points:");
                    Console.SetCursorPosition(15, 0);
                    Console.Write("Direction:");
                    Console.SetCursorPosition(29, 0);
                    Console.Write("Speed:");
                    Console.SetCursorPosition(0, 0);
                    break;
                case 2:
                    Console.SetCursorPosition(1, 0);
                    Console.Write("MULTIPLAYER GAME");
                    break;
                default:
                    break;
            }

        }

        private static void initSpielfeld(char[,] Spielfeld)
        {
            for (int y = 0; y <= Spielfeld.GetLength(1) - 1; y++)
            {
                for (int x = 0; x <= Spielfeld.GetLength(0) - 1; x++)
                {
                    if (y == 1 || y == Spielfeld.GetLength(1) - 1)
                    {
                        Spielfeld[x, y] = '*';
                    }
                    else if (y > 0 && y < Spielfeld.GetLength(1) && (x == 0 || x == Spielfeld.GetLength(0) - 1))
                    {
                        Spielfeld[x, y] = '*';
                    }
                }
            }
        }


        private static void KeyBoardListener(ConsoleKey keypressed, Snake snake)
        {
            int i = 0;


            if (!snake.Gezeichnet) goto hit;
            if (keypressed == snake.up && snake.Richtung != 'S')
            {
            north:
                if (snake.Gezeichnet)
                {
                    snake.Gezeichnet = false;
                    snake.Richtung = 'N';
                    Debug.WriteLine(snake.Richtung);
                    goto hit;
                }
                else { i = 0; goto north; }
            }
            else if (keypressed == snake.right && snake.Richtung != 'W')
            {
            east:
                if (snake.Gezeichnet)
                {
                    snake.Gezeichnet = false;
                    snake.Richtung = 'E';
                    Debug.WriteLine(snake.Richtung);
                    goto hit;
                }
                else { i = 0; goto east; }
            }
            else if (keypressed == snake.down && snake.Richtung != 'N')
            {
            south:
                if (snake.Gezeichnet)
                {
                    snake.Gezeichnet = false;
                    snake.Richtung = 'S';
                    Debug.WriteLine(snake.Richtung);
                    goto hit;
                }
                else { i = 0; goto south; }
            }
            else if (keypressed == snake.left && snake.Richtung != 'E')
            {
            west:
                if (snake.Gezeichnet)
                {
                    snake.Gezeichnet = false;
                    snake.Richtung = 'W';
                    Debug.WriteLine(snake.Richtung);
                    goto hit;
                }
                else { i = 0; goto west; }
            }
        hit:

            i++;
        }


        private static void spawnFood(char[,] Spielfeld)
        {
            Random myRNDNumber = new Random();
            if (GlobaleVariablen.eaten)
            {
                int x = myRNDNumber.Next(1, Spielfeld.GetLength(0) - 1);
                int y = myRNDNumber.Next(2, Spielfeld.GetLength(1) - 1);
                if (Spielfeld[x, y] == '\0')
                {
                    Random myRNDItem = new Random();
                    int i = myRNDItem.Next(0, 11);
                    switch (i)
                    {
                        case 1:
                            Spielfeld[x, y] = '+';
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.SetCursorPosition(x, y);
                            Console.Write("+");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        default:
                            Spielfeld[x, y] = '#';
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.SetCursorPosition(x, y);
                            Console.Write("#");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }
                    Console.SetCursorPosition(0, 0);
                    GlobaleVariablen.eaten = false;
                }
            }
        }
    }
}
