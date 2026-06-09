using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_Lab1
{
    public class Texture : IDisposable
    {
        public int Handle { get; private set; }
        private bool disposedValue = false;

        public Texture()
        {
            Handle = GL.GenTexture();
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public int LoadTexture(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            // Настройка фильтрации текстуры
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Загрузка изображения
            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
                bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
            
            return Handle;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteTexture(Handle);
                disposedValue = true;
            }
        }

        ~Texture() => Dispose(false);
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
    }
}