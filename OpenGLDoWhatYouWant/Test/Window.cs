using OpenTK;
using System;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace OpenGLDoWhatYouWant
{
    class Window : GameWindow
    {
        int program, vao, vbo;

        public Window(int width, int height) : base(width, height)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color.Azure);

            program = GL.CreateProgram();

            int vs = ShaderLoader.CreateShader("./defaultVertex.glsl", ShaderType.VertexShader);
            int fs = ShaderLoader.CreateShader("./defaultFragment.glsl", ShaderType.FragmentShader);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            vbo = GL.GenBuffer();

            ShaderLoader.CreateProgram(program, true, vs, fs);

            float[] data =
            {
                -0.5f,  -0.5f,  0.0f,
                0.0f,   0.5f,   0.0f,
                0.5f,   -0.5f,  0.0f
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(vao);

            GL.UseProgram(program);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            GL.DeleteProgram(program);

            GL.DeleteBuffer(vao);
            GL.DeleteBuffer(vbo);
        }
    }
}
