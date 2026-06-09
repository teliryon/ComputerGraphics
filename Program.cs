using System;

namespace OpenTK_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка кодировки, чтобы русский текст в консоли не превращался в кракозябры
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Лабораторная работа №1");
                Console.WriteLine("2. Лабораторная работа №2");
                Console.WriteLine("3. Лабораторная работа №3");
                Console.WriteLine("4. Лабораторная работа №4");
                Console.WriteLine("0. Выход из программы");
                Console.Write("Введите номер лабы (0-4): ");

                string? input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine("Выход...");
                    break;
                }

                switch (input)
                {
                    case "1":
                        Console.WriteLine("\nЗапуск Лабораторной №1...");
                        using (GameLab1 game = new GameLab1(1000, 800, "Лабораторная №1"))
                        {
                            game.Run(60.0);
                        }
                        break;

                    case "2":
                        Console.WriteLine("\nЗапуск Лабораторной №2...");
                        using (GameLab2 game = new GameLab2(800, 600, "Лабораторная №2"))
                        {
                            game.Run(60.0);
                        }
                        break;

                    case "3":
                        Console.WriteLine("\nЗапуск Лабораторной №3...");
                        using (GameLab3 game = new GameLab3(800, 600, "Лабораторная работа №3"))
                        {
                            game.Run(60.0);
                        }
                        break;

                    case "4":
                        Console.WriteLine("\nЗапуск Лабораторной №4...");
                        using (GameLab4 game = new GameLab4(800, 600, "Лабораторная работа №4"))
                        {
                            game.Run(60.0);
                        }
                        break;

                    default:
                        Console.WriteLine("\n[Ошибка] Неверный ввод! Нажмите любую клавишу, чтобы повторить...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}