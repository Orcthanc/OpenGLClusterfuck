using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLDoWhatYouWant
{
    partial class Window
    {
        bool[] keyDown;
        private bool isWireframe;

        protected void InitKeyboard()
        {
            keyDown = new bool[Key.LastKey.GetHashCode()];
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Console.WriteLine("[INFO] Key " + e.Key + " got pressed");

            keyDown[e.Key.GetHashCode()] = true;

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Home:
                    if (isWireframe)
                    {
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                        isWireframe = false;
                    }
                    else
                    {
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        isWireframe = true;
                    }
                    break;
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            
            keyDown[e.Key.GetHashCode()] = false;

            //switch (e.Key)
            //{
            //}
        }
    }
}
