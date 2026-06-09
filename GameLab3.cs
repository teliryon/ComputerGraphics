using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_Lab
{
    public class GameLab3 : GameWindow
    {
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        private Shader _shader = null!;
        private Texture _texture = null!;

        // Прямоугольник из 2 треугольников с координатами, цветами и текстурными координатами
        // Format: Position (3) + Color (3) + TexCoord (2) = 8 floats per vertex
        private readonly float[] _vertices = {
            // Position          Color           TexCoord
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f,  // Top-right (желтый)
             0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,  // Bottom-right (желтый)
            -0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f,  // Bottom-left (желтый)
            -0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f   // Top-left (желтый)
        };

        private readonly uint[] _indices = {
            0, 1, 3,  // First triangle
            1, 2, 3   // Second triangle
        };

        public GameLab3(int width, int height, string title) 
            : base(width, height, GraphicsMode.Default, title) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color4.MidnightBlue);

            // Инициализация VAO
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Инициализация VBO
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // Инициализация EBO
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Загрузка шейдеров (для задания 2-3 используем shader.vert и shader.frag)
            _shader = new Shader("shader_lab_3.vert", "shader_lab_3.frag");
            _shader.Use();

            // Настройка атрибутов вершин (Position + Color + TexCoord)
            SetupVertexAttributes();

            // Загрузка текстуры (создай файл texture.png в корневой папке)
            _texture = new Texture();
            try 
            {
                _texture.LoadTexture("texture_cats.png");
                Console.WriteLine("Текстура загружена успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки текстуры: {ex.Message}");
                Console.WriteLine("Продолжаем без текстуры...");
            }
        }

        private void SetupVertexAttributes()
        {
            // Position attribute (3 floats)
            int vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexLocation);

            // Color attribute (3 floats)
            int colorLocation = _shader.GetAttribLocation("aColor");
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(colorLocation);

            // TexCoord attribute (2 floats)
            int texCoordLocation = _shader.GetAttribLocation("vTex");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            
            // Активируем текстуру (если есть)
            _texture?.Use(TextureUnit.Texture0);

            // Передаем номер текстурного блока в шейдер
            _shader.SetInt("texture1", 0);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            this.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard.GetState().IsKeyDown(Key.Escape)) Exit();
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteBuffer(_elementBufferObject);

            _shader.Dispose();
            _texture?.Dispose();

            base.OnUnload(e);
        }
    }
}