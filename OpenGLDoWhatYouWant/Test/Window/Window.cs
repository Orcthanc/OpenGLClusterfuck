using OpenTK;
using System;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace OpenGLDoWhatYouWant
{
    partial class Window : GameWindow
    {
        public float isometricFactor = 0.3f;

        int program, vao, vbo, modelLength, texture0;

        Matrix4 view;

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

            InitKeyboard();

            // Enables depth-checks to decide which object is rendered in the foreground
            GL.Enable(EnableCap.DepthTest);
            // Set's the color used to clear the background
            GL.ClearColor(Color.Gray);

            // Creates a link to a program (Used to store shaders
            program = GL.CreateProgram();

            // Creates a standard vertex- and fragment-shader
            int vs = ShaderLoader.CreateShader("./Shaders/defaultVertex.glsl", ShaderType.VertexShader);
            int fs = ShaderLoader.CreateShader("./Shaders/defaultFragment.glsl", ShaderType.FragmentShader);

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

            // Load the data from an .obj using the ObjectLoader-class
            float[] data = ObjectLoader.LoadOBJTriangles("./Objects/flyingRobot.obj", out modelLength);

            // Put's some data into the vbo assigned to the vao inside of the ArrayBuffer BufferTarget
            GL.BufferData(BufferTarget.ArrayBuffer,
                // Length of the Array in bits
                data.Length * sizeof(float),
                // The array
                data,
                // Some OpenGL-stuff used for performance-finetuning... Look it up before changing it
                BufferUsageHint.StaticDraw);

            ObjectLoader.ConfigureVaoForOBJTriangles();

            //Load the Texture
            GL.ActiveTexture(TextureUnit.Texture0);
            texture0 = TextureLoader.LoadImage("./Textures/FlyingRobot.png");

        }

        /// <summary>
        /// Get's called once every frame-update
        /// </summary>
        /// <param name="e">Don't know, don't care</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // What could this possibly do?
            UpdateCamera();

            // Clears data...
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Tells the thing to use the vao...
            GL.BindVertexArray(vao);

            // ... and the program
            GL.UseProgram(program);

            // Sample matrixes used to move the model
            // Loc-Rot-Scale of the model of the model
            Matrix4 model = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), (float)Math.PI);
            model = Matrix4.Mult(Matrix4.CreateFromAxisAngle(new Vector3(0f, -0.2f, 1f), (float)(Environment.TickCount / 20 % 360) * (float)Math.PI / 180), model);
            model = Matrix4.Mult(Matrix4.CreateScale(0.5f, 0.5f, 0.5f), model);

            // Updates the view Matrix
            view = Matrix4.LookAt(camPos, camPos + camForward + new Vector3(0, isometricFactor, 0), camUp);

            // Apply perspective to everything
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(90f / 180 * (float)Math.PI, Program.sizeX / Program.sizeY, 0.1f, 100f);

            // Send the matrixes to the Graphics-card
            int modelLoc = GL.GetUniformLocation(program, "model");
            GL.UniformMatrix4(modelLoc, false, ref model);

            int viewLoc = GL.GetUniformLocation(program, "view");
            GL.UniformMatrix4(viewLoc, false, ref view);

            int projectionLoc = GL.GetUniformLocation(program, "projection");
            GL.UniformMatrix4(projectionLoc, false, ref projection);


            // Ligthing stuff
            Vector3 lightColor = new Vector3(1f, 1f, 1f);
            Vector3 objectColor = new Vector3(1f, 1f, 1f);
            Vector3 lightPos = new Vector3(0f, 5f, 0f);

            int lightColorLoc = GL.GetUniformLocation(program, "lightColor");
            GL.Uniform3(lightColorLoc, ref lightColor);

            int objectColorLoc = GL.GetUniformLocation(program, "objectColor");
            GL.Uniform3(objectColorLoc, ref objectColor);

            int lightPosLoc = GL.GetUniformLocation(program, "lightPos");
            GL.Uniform3(lightPosLoc, ref lightPos);


            // Texture stuff
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture0);

            GL.Uniform1(GL.GetUniformLocation(program, "texture0"), 0);

            // Draws things, considering all applied things (You can draw an vbo 2 times if you apply different vectors and call this again)
            GL.DrawArrays(PrimitiveType.Triangles, 0, modelLength);


            // Draws a second object
            model = Matrix4.Mult(Matrix4.CreateTranslation(new Vector3(4f, 0, 0)), model);
            
            GL.UniformMatrix4(modelLoc, false, ref model);

            GL.DrawArrays(PrimitiveType.Triangles, 0, modelLength);


            // Draw a third
            model = Matrix4.Mult(Matrix4.CreateTranslation(new Vector3(-8f, 0, 0)), model);

            GL.UniformMatrix4(modelLoc, false, ref model);

            GL.DrawArrays(PrimitiveType.Triangles, 0, modelLength);

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
