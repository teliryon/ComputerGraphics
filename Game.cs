using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_Lab1
{
    public class Game : GameWindow
    {
        public Game(int width, int height, string title) 
            : base(width, height, GraphicsMode.Default, title) 
        { 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color4.MidnightBlue); // Красивый глубокий синий фон
            GL.PointSize(4.0f); // Делаем точки чуть крупнее для наглядности (для Lines)
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
            {
                Exit();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Width, Height);

            // 1. Исследование примитивов (Задание 2)
            DrawPrimitivesResearch();

            // 2. Правильные многоугольники через математику (Задание 3)
            DrawRegularShapes();

            this.SwapBuffers();
        }

        #region Задание 2: Исследование примитивов
        private void DrawPrimitivesResearch()
        {
            // --- ЛЕВАЯ ЧАСТЬ ЭКРАНА: Линейные примитивы ---
            
            // Lines (независимые отрезки)
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1.0f, 0.5f, 0.5f); // Светло-красный
            GL.Vertex3(-0.9, 0.8, 0); GL.Vertex3(-0.6, 0.5, 0);
            GL.Vertex3(-0.9, 0.5, 0); GL.Vertex3(-0.6, 0.2, 0);
            GL.End();

            // LineStrip (ломаная линия)
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(0.5f, 1.0f, 0.5f); // Светло-зеленый
            GL.Vertex3(-0.9, -0.2, 0); GL.Vertex3(-0.6, 0.1, 0);
            GL.Vertex3(-0.3, -0.2, 0); GL.Vertex3(-0.6, -0.5, 0);
            GL.End();

            // LineLoop (замкнутая ломаная)
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(1.0f, 1.0f, 0.5f); // Желтый
            GL.Vertex3(-0.9, -0.7, 0); GL.Vertex3(-0.6, -0.7, 0);
            GL.Vertex3(-0.6, -0.9, 0); GL.Vertex3(-0.9, -0.9, 0);
            GL.End();

            // --- ПРАВАЯ ЧАСТЬ ЭКРАНА: Заполняющие примитивы ---

            // Triangles (два независимых треугольника, как в методичке)
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(0.5f, 0.5f, 1.0f); // Голубой
            // 1 четверть
            GL.Vertex3(0.3, 0.3, 0); GL.Vertex3(0.3, 0.8, 0); GL.Vertex3(0.8, 0.8, 0);
            // 3 четверть
            GL.Vertex3(-0.3, -0.3, 0); GL.Vertex3(-0.3, -0.8, 0); GL.Vertex3(-0.8, -0.8, 0);
            GL.End();

            // TriangleStrip (лента треугольников)
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(1.0f, 0.5f, 1.0f); // Розовый
            GL.Vertex3(0.3, 0.1, 0); GL.Vertex3(0.3, 0.6, 0); 
            GL.Vertex3(0.6, 0.1, 0); GL.Vertex3(0.6, 0.6, 0);
            GL.End();

            // TriangleFan (веер треугольников, в методичке опечатка TriangleFun)
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(0.5f, 1.0f, 1.0f); // Бирюзовый
            GL.Vertex3(0.8, -0.5, 0); // Центральная вершина
            GL.Vertex3(0.5, -0.2, 0); GL.Vertex3(0.5, -0.8, 0); 
            GL.Vertex3(0.8, -0.9, 0); GL.Vertex3(0.9, -0.5, 0);
            GL.End();

            // Quads (четырехугольники)
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(1.0f, 0.8f, 0.2f); // Оранжевый
            GL.Vertex3(0.3, -0.3, 0); GL.Vertex3(0.6, -0.3, 0); 
            GL.Vertex3(0.6, -0.6, 0); GL.Vertex3(0.3, -0.6, 0);
            GL.End();

            // QuadStrip (лента четырехугольников)
            GL.Begin(PrimitiveType.QuadStrip);
            GL.Color3(0.2f, 0.8f, 0.2f); // Темно-зеленый
            GL.Vertex3(0.7, -0.3, 0); GL.Vertex3(0.9, -0.3, 0);
            GL.Vertex3(0.7, -0.6, 0); GL.Vertex3(0.9, -0.6, 0);
            GL.Vertex3(0.7, -0.9, 0); GL.Vertex3(0.9, -0.9, 0);
            GL.End();

            // Polygon (выпуклый многоугольник, например, пятиугольник)
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(0.8f, 0.2f, 0.2f); // Красный
            GL.Vertex3(0.5, 0.0, 0); GL.Vertex3(0.7, 0.2, 0); 
            GL.Vertex3(0.6, 0.5, 0); GL.Vertex3(0.4, 0.5, 0); 
            GL.Vertex3(0.3, 0.2, 0);
            GL.End();
        }
        #endregion

        #region Задание 3: Правильные многоугольники (Математический подход)
        
        /// <summary>
        /// Универсальный метод для рисования любого правильного многоугольника
        /// </summary>
        /// <param name="sides">Количество сторон (3 - треугольник, 4 - квадрат, 6 - шестиугольник)</param>
        /// <param name="centerX">Координата X центра</param>
        /// <param name="centerY">Координата Y центра</param>
        /// <param name="radius">Радиус описанной окружности</param>
        /// <param name="color">Цвет фигуры</param>
        private void DrawRegularPolygon(int sides, double centerX, double centerY, double radius, Color4 color)
        {
            GL.Color4(color);
            GL.Begin(PrimitiveType.LineLoop); // Рисуем красивый контур. Можно заменить на PrimitiveType.Polygon для заливки
            
            for (int i = 0; i < sides; i++)
            {
                // Математика: вычисляем угол для каждой вершины. 
                // Вычитаем PI/2, чтобы первая вершина была строго сверху (для красоты)
                double angle = 2.0 * Math.PI * i / sides - Math.PI / 2.0;
                
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);
                
                GL.Vertex3(x, y, 0);
            }
            
            GL.End();
        }

        private void DrawRegularShapes()
        {
            // Рисуем рамку-подложку для зоны 3-го задания (для красоты)
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.3f, 0.3f, 0.3f);
            GL.Vertex3(-0.25, 0.95, 0); GL.Vertex3(0.25, 0.95, 0);
            GL.Vertex3(0.25, -0.95, 0); GL.Vertex3(-0.25, -0.95, 0);
            GL.End();

            // 1. Равносторонний треугольник (3 стороны)
            DrawRegularPolygon(3, 0.0, 0.6, 0.25, Color4.Gold);
            
            // 2. Квадрат (4 стороны)
            DrawRegularPolygon(4, 0.0, 0.0, 0.25, Color4.Cyan);
            
            // 3. Правильный шестиугольник (6 сторон)
            DrawRegularPolygon(6, 0.0, -0.6, 0.25, Color4.Magenta);
        }
        #endregion
    }
}