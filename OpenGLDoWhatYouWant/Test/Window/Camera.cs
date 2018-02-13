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
        Vector3 camPos = new Vector3(0, 0, 3);
        Vector3 camForward = new Vector3(0, 0, -1);
        Vector3 camUp = new Vector3(0, 1, 0);

        float camSpeed = 0.05f;

        public void UpdateCamera()
        {
            Vector3 move = new Vector3();

            if (keyDown[Key.A.GetHashCode()])
                move.X -= 1;
            if (keyDown[Key.D.GetHashCode()])
                move.X += 1;
            if (keyDown[Key.S.GetHashCode()])
                move.Z -= 1;
            if (keyDown[Key.W.GetHashCode()])
                move.Z += 1;

            MoveCamera(move);
        }

        public void MoveCamera(Vector3 dir)
        {
            if(dir.Z > 0)
            {
                camPos += camSpeed * camForward;
            }
            else if(dir.Z < 0)
            {
                camPos -= camSpeed * camForward;
            }

            if (dir.Y > 0)
            {
                camPos += camSpeed * camUp;
            }
            else if (dir.Y < 0)
            {
                camPos -= camSpeed * camUp;
            }

            if (dir.X > 0)
            {
                camPos += camSpeed * Vector3.Normalize(Vector3.Cross(camForward, camUp));
            }
            else if (dir.X < 0)
            {
                camPos -= camSpeed * Vector3.Normalize(Vector3.Cross(camForward, camUp));
            }
        }
    }
}
