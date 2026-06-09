using System;

namespace OpenTK_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем окно 800x600. Можно сделать 1000x800 для еще лучшей читаемости
            using (Game game = new Game(1000, 800, "Лабораторная работа №1: OpenTK Primitives"))
            {
                game.Run(60.0);
            }
        }
    }
}