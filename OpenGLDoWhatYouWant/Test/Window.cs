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

        /// <summary>
        /// Called once on initialization...
        /// </summary>
        /// <param name="e">Don't know, don't care.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Enables depth-checks to decide which object is rendered in the foreground
            GL.Enable(EnableCap.DepthTest);
            // Set's the color used to clear the background
            GL.ClearColor(Color.Azure);

            // Creates a link to a program (Used to store shaders
            program = GL.CreateProgram();

            // Creates a standard vertex- and fragment-shader
            int vs = ShaderLoader.CreateShader("./defaultVertex.glsl", ShaderType.VertexShader);
            int fs = ShaderLoader.CreateShader("./defaultFragment.glsl", ShaderType.FragmentShader);

            // Create's a program using the shaders vs and fs in the space linked to by program
            ShaderLoader.CreateProgram(program, true, vs, fs);

            // Creates an Link to an VertexArrayObject
            vao = GL.GenVertexArray();
            // Put's the vao in the ArrayBuffer BufferTarget
            GL.BindVertexArray(vao);
            // Generate's a VertexBufferObject
            vbo = GL.GenBuffer();
            // Assigns the vbo to the thing found in the ArrayBuffer BufferTarget
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            // Some VertexData (Simple Triangle)
            float[] data =
            {
                0.0f,   0.5f,   0.0f,
                0.0f,  -0.5f,   0.5f,
               -0.5f,  -0.5f,  -0.5f,

                0.0f,   0.5f,   0.0f,
                0.5f,  -0.5f,  -0.5f,
                0.0f,  -0.5f,   0.5f,

                0.0f,   0.5f,   0.0f,
               -0.5f,  -0.5f,  -0.5f,
                0.5f,  -0.5f,  -0.5f,

                0.0f,  -0.5f,   0.5f,
                0.5f,  -0.5f,  -0.5f,
               -0.5f,  -0.5f,  -0.5f,
            };

            // Put's some data into the vbo assigned to the vao inside of the ArrayBuffer BufferTarget
            GL.BufferData(BufferTarget.ArrayBuffer,
                // Length of the Array in bits
                data.Length * sizeof(float),
                // The array
                data,
                // Some OpenGL-stuff used for performance-finetuning... Look it up before changing it
                BufferUsageHint.StaticDraw);

            // Telling OpenGL how to read the array
            GL.VertexAttribPointer(
                // Id (Used to get the data in the Vertex-Shader
                0,
                // How long one set of data is
                3,
                // Type of the data
                VertexAttribPointerType.Float,
                // If the data should be normalized
                false,
                // Amount of bites after which the next set of data starts, relative to the start of the current set
                3 * sizeof(float),
                // Amount of bits after which the first set starts
                0);
            // Get's called after every VertexAttribPointer with the Id used in it to make it work
            GL.EnableVertexAttribArray(0);

        }

        /// <summary>
        /// Get's called once every frame-update
        /// </summary>
        /// <param name="e">Don't know, don't care</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Clears data...
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Tells the thing to use the vao...
            GL.BindVertexArray(vao);

            // ... and the program
            GL.UseProgram(program);

            // Sample matrixes used to move the model
            // Loc-Rot-Scale of the model of the model
            Matrix4 model = Matrix4.CreateFromAxisAngle(new Vector3(0f, 1f, 1f), (float)(Environment.TickCount / 10 % 360) * (float)Math.PI / 180);
            model = Matrix4.Mult(Matrix4.CreateScale(1.5f, 1f, 1f), model);
            // Transform of the camera
            Matrix4 view = Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, -3.0f));
            // Apply perspective to everything
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(90f / 180 * (float)Math.PI, Program.sizeX / Program.sizeY, 0.1f, 100f);

            // Send the matrixes to the Graphics-card
            int modelLoc = GL.GetUniformLocation(program, "model");
            GL.UniformMatrix4(modelLoc, false, ref model);

            int viewLoc = GL.GetUniformLocation(program, "view");
            GL.UniformMatrix4(viewLoc, false, ref view);

            int projectionLoc = GL.GetUniformLocation(program, "projection");
            GL.UniformMatrix4(projectionLoc, false, ref projection);


            // Draws things, considering all applied things (You can draw an vao 2 times if you apply different vectors and call this again)
            GL.DrawArrays(PrimitiveType.Triangles, 0, 12);

            // Doublebuffering
            this.SwapBuffers();
        }

        /// <summary>
        /// Get's called when the Window get's disposed
        /// </summary>
        /// <param name="e">Don't know, don't care</param>
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);

            // Delete's everything
            GL.DeleteProgram(program);

            GL.DeleteBuffer(vao);
            GL.DeleteBuffer(vbo);
        }
    }
}
