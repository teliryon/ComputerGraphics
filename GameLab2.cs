using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_Lab1
{
    public class GameLab2 : GameWindow
    {
        // Дескрипторы буферов
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        private Shader _shader = null!; // Инициализируем позже в OnLoad, чтобы избежать проблем с порядком инициализации

        // Данные вершин (Прямоугольник из 2-х треугольников, как в методичке)
        private readonly float[] _vertices = {
            0.5f,  0.5f, 0.0f,  // Верхний правый (0)
            0.5f, -0.5f, 0.0f,  // Нижний правый (1)
           -0.5f, -0.5f, 0.0f,  // Нижний левый (2)
           -0.5f,  0.5f, 0.0f   // Верхний левый (3)
        };

        // Индексы для EBO (порядок обхода вершин)
        private readonly uint[] _indices = {
            0, 1, 3, // Первый треугольник
            1, 2, 3  // Второй треугольник
        };

        public GameLab2(int width, int height, string title) 
            : base(width, height, GraphicsMode.Default, title) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color4.MidnightBlue);

            // 1. Инициализация VAO (Обязательно для Core Profile!)
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // 2. Инициализация VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // 3. Инициализация EBO
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // 4. Загрузка шейдеров (СНАЧАЛА СОЗДАЕМ ШЕЙДЕР!)
            _shader = new Shader("shader_lab_2.vert", "shader_lab_2.frag");
            _shader.Use();

            // 5. Настройка указателя атрибутов вершин (ТЕПЕРЬ ШЕЙДЕР СУЩЕСТВУЕТ)
            int location = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(location, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(location);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Отрисовка
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);
            
            // Рисуем по индексам (EBO)
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            // Если нужно показать каркас (как в методичке Рисунок 2.6), раскомментируй строку ниже:
            // GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            this.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard.GetState().IsKeyDown(Key.Escape)) Exit();
        }

        protected override void OnUnload(EventArgs e)
        {
            // Очистка памяти
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteBuffer(_elementBufferObject);
            _shader.Dispose();

            base.OnUnload(e);
        }
    }
}