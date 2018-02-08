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
        bool keyDownA = false, keyDownD = false, keyDownW = false, keyDownS = false;

        protected void InitKeyboard()
        {

        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Console.WriteLine("[INFO] Key " + e.Key + " got pressed...");

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.A:
                    keyDownA = true;
                    break;
                case Key.D:
                    keyDownD = true;
                    break;
                case Key.W:
                    keyDownW = true;
                    break;
                case Key.S:
                    keyDownS = true;
                    break;
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    keyDownA = false;
                    break;
                case Key.D:
                    keyDownD = false;
                    break;
                case Key.W:
                    keyDownW = false;
                    break;
                case Key.S:
                    keyDownS = false;
                    break;
            }
        }
    }
}
