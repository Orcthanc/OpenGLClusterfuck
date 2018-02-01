using System;

namespace OpenGLDoWhatYouWant
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var window = new Window(1000, 800))
            {
                window.Run();
            }
        }
    }
}
