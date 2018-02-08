using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLDoWhatYouWant
{
    partial class Window
    {
        private bool isWireframe;

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Console.WriteLine("[INFO] Key " + e.Key + " got pressed");

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
    }
}
