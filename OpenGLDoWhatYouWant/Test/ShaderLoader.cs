using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLDoWhatYouWant
{
    class ShaderLoader
    {
        /// <summary>
        /// Creates an OpenGL-Shader
        /// </summary>
        /// <param name="file">Source of the shader</param>
        /// <param name="shadertype">Type of the shader</param>
        /// <returns>A link to the shader</returns>
        public static int CreateShader(String file, ShaderType shadertype)
        {
            int shader = GL.CreateShader(shadertype);
            using (StreamReader sr = File.OpenText(file))
            {
                GL.ShaderSource(shader, sr.ReadToEnd());
            }
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status != 1)
            {
                GL.GetShaderInfoLog(shader, out string info);
                Console.WriteLine(info);
            }

            return shader;
        }

        /// <summary>
        /// Writes shaders into a program
        /// </summary>
        /// <param name="program">Where the Program is stored in OpenGL</param>
        /// <param name="deleteShaders">If the shaders are to be deleted upon completion of the task</param>
        /// <param name="shaders">All the shaders supposed to be packed into the program</param>
        public static void CreateProgram(int program, bool deleteShaders, params int[] shaders)
        {
            foreach (int i in shaders)
            {
                GL.AttachShader(program, i);
            }

            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
            if (status != 1)
            {
                GL.GetProgramInfoLog(program, out string info);
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
