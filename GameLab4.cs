using System;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_Lab
{
    public class GameLab4 : GameWindow
    {
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        
        // Фикс CS8618: ставим знак ?, разрешая шейдеру инициализироваться позже в OnLoad
        private Shader? _shader; 
        private int _textureHandle;
        
        private double _time;

        // Массив вершин: Координаты (X,Y,Z) | Цвета (R,G,B) | Текстурные координаты (U,V)
        private readonly float[] _vertices = {
             // Координаты         // Цвета (Задание 3)   // Текстура (Задание 4)
             0.5f,  0.5f, 0.0f,    1.0f, 1.0f, 0.0f,      1.0f, 1.0f, // Точка 0
             0.5f, -0.5f, 0.0f,    1.0f, 1.0f, 0.0f,      1.0f, 0.0f, // Точка 1
            -0.5f, -0.5f, 0.0f,    1.0f, 1.0f, 0.0f,      0.0f, 0.0f, // Точка 2
            -0.5f,  0.5f, 0.0f,    1.0f, 1.0f, 0.0f,      0.0f, 1.0f  // Точка 3
        };

        private readonly uint[] _indices = {
            0, 1, 3, 
            1, 2, 3  
        };

        public GameLab4(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color4.MidnightBlue);

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _shader = new Shader(Path.Combine(baseDir, "shader_lab_4.vert"), Path.Combine(baseDir, "shader_lab_4.frag"));

            _vertexArrayObject = GL.GenVertexArray();
            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(_vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            int positionLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            int colorLocation = _shader.GetAttribLocation("aColor");
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(colorLocation);

            int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);

            _textureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _textureHandle);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            string texturePath = Path.Combine(baseDir, "texture_cats.png");
            if (File.Exists(texturePath))
            {
#pragma warning disable CA1416 // Глушим виндовый ворнинг для картинок
                using (Bitmap image = new Bitmap(texturePath))
                {
                    // Фикс CS0104: Явно пишем System.Drawing.Imaging для LockBits
                    System.Drawing.Imaging.BitmapData data = image.LockBits(
                        new Rectangle(0, 0, image.Width, image.Height), 
                        System.Drawing.Imaging.ImageLockMode.ReadOnly, 
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    );

                    // И явно пишем OpenTK.Graphics.OpenGL для отправки в видеокарту
                    GL.TexImage2D(
                        TextureTarget.Texture2D, 
                        0, 
                        PixelInternalFormat.Rgba, 
                        data.Width, 
                        data.Height, 
                        0, 
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                        PixelType.UnsignedByte, 
                        data.Scan0
                    );

                    image.UnlockBits(data);
                }
#pragma warning restore CA1416
                Console.WriteLine("Текстура для Лабы №4 успешно загружена!");
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Keyboard.GetState().IsKeyDown(Key.Escape))
            {
                Exit();
            }

            _time += 4.0 * e.Time;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, Width, Height);

            // Используем оператор ? на случай если шейдер null
            _shader?.Use();

            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Convert.ToSingle(_time)));
            Matrix4 scale = Matrix4.CreateScale(1.2f, 1.2f, 1.2f);
            Matrix4 transform = scale * rotation;

            if (_shader != null)
            {
                int location = GL.GetUniformLocation(_shader.Handle, "transform");
                GL.UniformMatrix4(location, false, ref transform);
            }

            GL.BindTexture(TextureTarget.Texture2D, _textureHandle);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteTexture(_textureHandle);
            _shader?.Dispose();
            base.OnUnload(e);
        }
    }
}