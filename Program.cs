using System;

namespace OpenTK_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (GameLab4 game = new GameLab4(800, 600, "Лабораторная работа №4: Анимация"))
            {
                game.Run(60.0);
            }

            // Для запуска Lab 3:
            //using (GameLab3 game = new GameLab3(800, 600, "Лабораторная работа №3"))
            //{
            //    game.Run(60.0);
            //}
            
            // Для Lab 2 раскомментируй:
            // using (GameLab2 game = new GameLab2(800, 600, "Лабораторная №2"))
            //{
            //     game.Run(60.0);
            //}
            
            // Для Lab 1 раскомментируй:
            // using (GameLab1 game = new GameLab1(1000, 800, "Лабораторная №1"))
            // {
            //     game.Run(60.0);
            // }
        }
    }
}