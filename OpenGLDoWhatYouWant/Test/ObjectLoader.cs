using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace Test
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

        public static float[] LoadOBJTriangles(String fileName)
        {
            ReadOBJ(fileName, out Vector3[] verts, out Vector2[] texCoords, out Vector3[] normals, out Face[] faces);

            List<float> verticies = new List<float>();

            foreach (Face f in faces)
            {
                //TODO
            }

            return verticies.ToArray();
        }

        public static void ReadOBJ(String fileName, out Vector3[] verts, out Vector2[] texCoords, out Vector3[] normals, out Face[] faces)
        {
            List<Vector3> vertsL = new List<Vector3>();
            List<Vector2> texCoordsL = new List<Vector2>();
            List<Vector3> vertNormsL = new List<Vector3>();
            List<Face> facesL = new List<Face>();

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
                        vertsL.Add(new Vector3(float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])));
                    }
                    else if (temp[0].Equals("vt"))
                    {
                        if (temp.Length != 3)
                        {
                            Console.WriteLine("[FATAL] Error occured while loading " + fileName + ". Perhaps the file is corrupt...");
                            break;
                        }
                        texCoordsL.Add(new Vector2(float.Parse(temp[1]), float.Parse(temp[2])));
                    }
                    else if (temp[0].Equals("vn"))
                    {
                        if(temp.Length != 4)
                        {
                            Console.WriteLine("[FATAL] Error occured while loading " + fileName + ". Perhaps the file is corrupt...");
                            break;
                        }
                        vertNormsL.Add(new Vector3(float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])));
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
