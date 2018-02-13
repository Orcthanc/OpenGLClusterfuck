using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace OpenGLDoWhatYouWant
{
    struct Triangle
    {
        public float x, y, z;

        public Triangle(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    struct Face
    {
        public int[] v, vt, vn;

        public int Length
        {
            get
            {
                return v.Length;
            }
            set
            {
                v = new int[value];
                vt = new int[value];
                vn = new int[value];
            }
        }

        public Face(int[] v, int[] vt, int[] vn)
        {
            this.v = v;
            this.vt = vt;
            this.vn = vn;
        }
    }

    class ObjectLoader
    {
        /// <summary>
        /// Configures the currently bound vao to draw things inputed via the LoadOBJTriangles Method
        /// </summary>
        public static void ConfigureVaoForOBJTriangles()
        {
            // Telling OpenGL how to read the array
            GL.VertexAttribPointer(
                // Id (Used to get the data in the Vertex-Shader)
                0,
                // How long one set of data is
                3,
                // Type of the data
                VertexAttribPointerType.Float,
                // If the data should be normalized
                false,
                // Amount of bites after which the next set of data starts, relative to the start of the current set (stride)
                8 * sizeof(float),
                // Amount of bits after which the first set starts (offset)
                0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

            // Get's called after every VertexAttribPointer with the Id used in it to make it work
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            //x Direction
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            //y Direction
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            //minifying
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            //magnifying
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        /// <summary>
        /// Used to generate VertexData from a .obj-File
        /// </summary>
        /// <param name="fileName">Name of the .obj file</param>
        /// <returns>The Vertexdata as a float-array</returns>
        public static float[] LoadOBJTriangles(String fileName, out int length)
        {
            ReadOBJ(fileName, out Vector3[] verts, out Vector2[] texCoords, out Vector3[] normals, out Face[] faces);

            List<float> verticies = new List<float>();

            foreach (Face f in faces)
            {
                for(int i = 1; i < f.Length - 1; i++)
                {
                    verticies.AddRange(new float[] { verts[f.v[0]].X, verts[f.v[0]].Y, verts[f.v[0]].Z });
                    verticies.AddRange(new float[] { texCoords[f.vt[0]].X, texCoords[f.vt[0]].Y });
                    verticies.AddRange(new float[] { normals[f.vn[0]].X, normals[f.vn[0]].Y, normals[f.vn[0]].Z });

                    verticies.AddRange(new float[] { verts[f.v[i]].X, verts[f.v[i]].Y, verts[f.v[i]].Z });
                    verticies.AddRange(new float[] { texCoords[f.vt[i]].X, texCoords[f.vt[i]].Y });
                    verticies.AddRange(new float[] { normals[f.vn[i]].X, normals[f.vn[i]].Y, normals[f.vn[i]].Z });

                    verticies.AddRange(new float[] { verts[f.v[i + 1]].X, verts[f.v[i + 1]].Y, verts[f.v[i + 1]].Z });
                    verticies.AddRange(new float[] { texCoords[f.vt[i + 1]].X, texCoords[f.vt[i + 1]].Y });
                    verticies.AddRange(new float[] { normals[f.vn[i + 1]].X, normals[f.vn[i + 1]].Y, normals[f.vn[i + 1]].Z });
                }
            }

            length = verticies.Count;

            return verticies.ToArray();
        }

        /// <summary>
        /// Just reads all the data from an .obj-File
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="verts">An Vector3-Array where all the coordinates of the verticies get stored</param>
        /// <param name="texCoords">An Vector2-Array where all the Texture-Coordinates get stored</param>
        /// <param name="normals">An Vector3-Array where all the normal-Vecs are stored</param>
        /// <param name="faces">An Face-Array, where all faces are stored in the form of references to the other Arrays</param>
        public static void ReadOBJ(String fileName, out Vector3[] verts, out Vector2[] texCoords, out Vector3[] normals, out Face[] faces)
        {
            List<Vector3> vertsL = new List<Vector3>();
            List<Vector2> texCoordsL = new List<Vector2>();
            List<Vector3> vertNormsL = new List<Vector3>();
            List<Face> facesL = new List<Face>();

            // Dummy coords because indexes in .objs start at one and not 0
            vertsL.Add(new Vector3());
            texCoordsL.Add(new Vector2());
            vertNormsL.Add(new Vector3());

            using (StreamReader sr = File.OpenText(fileName))
            {
                while (!sr.EndOfStream)
                {
                    String tempStr = sr.ReadLine().Trim();
                    String[] temp = tempStr.Split(' ');

                    if (temp.Length == 0 || temp[0].Equals("#"))
                        continue;
                    else if (temp[0].Equals("mtllib"))
                    {
                        Console.WriteLine("[WARNING] Materials have not yet been implemented, so the model might look different than it should");
                    }
                    else if (temp[0].Equals("o"))
                    {
                        if (temp.Length == 1)
                        {
                            Console.WriteLine("[FATAL] Error occured while loading " + fileName + ". Perhaps the file is corrupt...");
                            break;
                        }
                        Console.WriteLine("[INFO] Loading object " + temp[1]);
                    }
                    else if (temp[0].Equals("v"))
                    {
                        if (temp.Length < 4)
                        {
                            Console.WriteLine("[FATAL] Error occured while loading " + fileName + ". Perhaps the file is corrupt...");
                            break;
                        }
                        vertsL.Add(new Vector3(float.Parse(temp[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(temp[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(temp[3], CultureInfo.InvariantCulture.NumberFormat)));
                    }
                    else if (temp[0].Equals("vt"))
                    {
                        if (temp.Length != 3)
                        {
                            Console.WriteLine("[FATAL] Error occured while loading " + fileName + ". Perhaps the file is corrupt...");
                            break;
                        }
                        texCoordsL.Add(new Vector2(float.Parse(temp[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(temp[2], CultureInfo.InvariantCulture.NumberFormat)));
                    }
                    else if (temp[0].Equals("vn"))
                    {
                        if(temp.Length != 4)
                        {
                            Console.WriteLine("[FATAL] Error occured while loading " + fileName + ". Perhaps the file is corrupt...");
                            break;
                        }
                        vertNormsL.Add(new Vector3(float.Parse(temp[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(temp[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(temp[3], CultureInfo.InvariantCulture.NumberFormat)));
                    }
                    else if (temp[0].Equals("g"))
                    {
                        Console.WriteLine("[INFO] Started loading the facegroup " + temp[1] + ".");
                    }
                    else if (temp[0].Equals("usemtl"))
                    {
                        //TODO
                    }
                    else if (temp[0].Equals("s"))
                    {
                        if (!temp[1].Equals("off"))
                            Console.WriteLine("[WARNING] Smooth shading hasn't been implemented till now...");
                        else
                            Console.WriteLine("[INFO] Turned smooth shading off...");
                    }
                    else if (temp[0].Equals("f"))
                    {
                        Face f = new Face
                        {
                            Length = temp.Length - 1
                        };
                        
                        for(int i = 1; i < temp.Length; i++)
                        {
                            String[] tempSplit = temp[i].Split('/');

                            f.v[i - 1] = int.Parse(tempSplit[0]);

                            if (tempSplit.Length > 1 && !tempSplit[1].Equals(""))
                            {
                                f.vt[i - 1] = int.Parse(tempSplit[1]);
                            }

                            if(tempSplit.Length > 2)
                            {
                                f.vn[i - 1] = int.Parse(tempSplit[2]);
                            }
                        }

                        facesL.Add(f);
                    }
                }
            }

            verts = vertsL.ToArray();
            texCoords = texCoordsL.ToArray();
            normals = vertNormsL.ToArray();
            faces = facesL.ToArray();
        }

    }
}
