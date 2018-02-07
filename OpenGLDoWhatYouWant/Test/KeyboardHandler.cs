using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace OpenGLDoWhatYouWant
{


    partial class Window
    {
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Console.WriteLine("[INFO] Key " + e.Key + " got pressed...");

        }
    }
}
