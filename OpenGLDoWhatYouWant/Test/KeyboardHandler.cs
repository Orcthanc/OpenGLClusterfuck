using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace OpenGLDoWhatYouWant
{
    partial class Window
    {
        bool[] keyDown;

        protected void InitKeyboard()
        {
            keyDown = new bool[Key.LastKey.GetHashCode()];
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Console.WriteLine("[INFO] Key " + e.Key + " got pressed...");

            keyDown[e.Key.GetHashCode()] = true;

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
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
