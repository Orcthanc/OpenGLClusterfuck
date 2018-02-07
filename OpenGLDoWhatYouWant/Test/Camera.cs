using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGLDoWhatYouWant
{
    partial class Window
    {
        Vector3 camPos = new Vector3(0, 0, 3);
        Vector3 camForward = new Vector3(0, 0, -1);
        Vector3 camUp = new Vector3(0, 1, 0);

        public void MoveCamera(Vector3 dir)
        {

        }
    }
}
