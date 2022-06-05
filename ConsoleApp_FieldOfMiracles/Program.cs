using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp_FieldOfMiracles
{
    internal class Program
    {
        static int ReadMoveButtons(char up, char down)
        {
            ConsoleKeyInfo symb = Console.ReadKey();
            char symbChar = symb.KeyChar;

            if (symbChar == up || symb.Key == ConsoleKey.UpArrow)
            {
                return -1;
            }
            else if (symbChar == down || symb.Key == ConsoleKey.DownArrow)
            {
                return 1;
            }
            else if (symb.Key == ConsoleKey.Enter)
            {
                return 0;
            }
            else
            {
                return (int)symb.Key;
            }
        }

        static void ShowMainMenu(int cursorPos, string[] menuPoints, string cursor)
        {
            Console.Clear();
            Console.WriteLine("\n" + new String(' ', 21) + "Поле чудес\n\n");

            for (int i = 0; i < menuPoints.Length; i++)
            {
                Console.WriteLine("");
                if (cursorPos != i)
                {
                    Console.WriteLine(new String(' ', 22) + menuPoints[i]);
                }
                else
                {
                    Console.WriteLine(new String(' ', 20) + cursor + menuPoints[i]);
                }
            }
        }

        static public void MainProgram(string[] args)
        {
            int cursorPos = 0;
            string[] menuPoints = { "Играть", "Вопросы", "Выход" };
            string cursor = "-> ";
            bool isMenuShowed = false;

            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            Task task = new Task(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (cursor == "->")
                    {
                        cursor = "=> ";
                    }
                    else
                    {
                        cursor = "->";
                    }
                    
                    if (!isMenuShowed)
                    {
                        isMenuShowed = true;
                        ShowMainMenu(cursorPos, menuPoints, cursor);
                        isMenuShowed = false;
                    }
                    await Task.Delay(500);
                }
                return;
            }, token);
            task.Start();

            int res;
            do
            {
                if (!isMenuShowed)
                {
                    isMenuShowed = true;
                    ShowMainMenu(cursorPos, menuPoints, cursor);
                    isMenuShowed = false;
                }
                res = ReadMoveButtons('w', 's');
                if (SystemInput.isBetween(res, -1, 1))
                {
                    if (SystemInput.isBetween(cursorPos + res, 0, 2))
                    {
                        cursorPos += res;
                    }
                }
            } while (res != 0);

            cancelTokenSource.Cancel();

            switch (cursorPos)
            {
                case 0:
                    {
                        Console.WriteLine("Load Game Script");
                        if (File.Exists(QuestionsManager.infoFile))
                        {
                            var gameScript = new Game();
                            gameScript.MainGame(args).Wait();
                        }
                        else
                        {
                            Console.WriteLine($"\nНе найдены вопросы. Пожалуйста добавьте вопросы в меню Вопросы или положите файл {QuestionsManager.infoFile} в папку(ConsoleApp_FieldOfMiracles/ConsoleApp_FieldOfMiracles/bin/Debug)");
                        }
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("Load Manager Script");
                        QuestionsManager.MainQuestions(args);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Quit Game");
                        Environment.Exit(0);
                        break;
                    }
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Поле чудес";

            Console.BufferHeight = 35;
            Console.WindowHeight = 35;
            Console.WindowWidth = 60;
            Console.BufferWidth = 60;

            MainProgram(args);
        }
    }
}
