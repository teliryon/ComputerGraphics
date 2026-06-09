using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_Lab1
{
    public class Shader : IDisposable
    {
        public int Handle { get; private set; }
        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            // 1. Загрузка исходного кода из файлов
            string vertexShaderSource = File.ReadAllText(vertexPath, Encoding.UTF8);
            string fragmentShaderSource = File.ReadAllText(fragmentPath, Encoding.UTF8);

            // 2. Создание и компиляция вершинного шейдера
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            CheckShaderCompileErrors(vertexShader, "VERTEX");

            // 3. Создание и компиляция фрагментного шейдера
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            CheckShaderCompileErrors(fragmentShader, "FRAGMENT");

            // 4. Сборка шейдерной программы
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);
            CheckProgramLinkErrors(Handle);

            // 5. Очистка временных шейдеров (они уже вшиты в программу)
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        private void CheckShaderCompileErrors(int shader, string type)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Ошибка компиляции {type}-шейдера:\n{infoLog}");
            }
        }

        private void CheckProgramLinkErrors(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
            if (status == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Ошибка линковки шейдерной программы:\n{infoLog}");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }
        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.Uniform1(location, value);
        }

        ~Shader() => Dispose(false);
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
    }
}