using System;

namespace OpenGLDoWhatYouWant
{
    class Program
    {
        public static int sizeX = 1000, sizeY = 800;

        static void Main(string[] args)
        {

            using (var window = new Window(sizeX, sizeY))
            {
                window.Run();
            }
        }
    }
}
