using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLDoWhatYouWant
{
    class ShaderLoader
    {

        public static int createShader(String file, ShaderType shadertype)
        {
            int shader = GL.CreateShader(shadertype);
            using (StreamReader sr = File.OpenText(file))
            {
                GL.ShaderSource(shader, sr.ReadToEnd());
            }
            GL.CompileShader(shader);
            int status;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                string info;
                GL.GetShaderInfoLog(shader, out info);
                Console.WriteLine(info);
            }

            return shader;
        }

        public static void createProgram(int program, bool deleteShaders, params int[] shaders)
        {
            foreach (int i in shaders)
            {
                GL.AttachShader(program, i);
            }

            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
            if (status != 1)
            {
                string info;
                GL.GetProgramInfoLog(program, out info);
                Console.WriteLine(info);
            }

            if (deleteShaders)
                foreach (int i in shaders)
                {
                    GL.DeleteShader(i);
                }
        }

    }
}
