using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System;
using System.IO;

namespace OpenGLDoWhatYouWant
{
    class TextureLoader
    {

        /// <summary>
        /// Loads a image into OpenGL
        /// </summary>
        /// <param name="path">Where the file is saved</param>
        /// <returns>An link to where the image is stored within OpenGL</returns>
        public static int LoadImage(string path)
        {
            Console.WriteLine(Path.GetFullPath(path));
            Bitmap bitmap = new Bitmap(Image.FromFile(path));

            return LoadImage(bitmap);
        }

        /// <summary>
        /// Loads a image into OpenGL
        /// </summary>
        /// <param name="image">The bitmap to be loaded into OpenGL</param>
        /// <returns>An link to where the image is stored within OpenGL</returns>
        public static int LoadImage(Bitmap image)
        {
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            int texID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            image.UnlockBits(data);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }

    }
}
