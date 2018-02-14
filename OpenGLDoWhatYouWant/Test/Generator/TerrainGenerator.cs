using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Generator
{
    class TerrainGenerator
    {
        Random random = new Random();

        
        /// <summary>
        /// Generates a completely random noisePattern
        /// </summary>
        /// <param name="array"></param>
        public void GenerateRandomNoise(ref float[,] array)
        {
            for(int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    array[x, y] = (float)random.NextDouble();
                }
            }
        }

        /// <summary>
        /// Used to generate a ugly worse variant of the ValueNoise...
        /// It also only eats uneven arraydimensions...
        /// </summary>
        /// <param name="array">The array where the noise should be stored</param>
        public void GenerateBadValueNoise(ref float[,] array)
        {
            if(array.GetLength(0) % 2 == 0 || array.GetLength(1) % 2 == 0)
            {
                throw new Exception("Array dimensions must be uneven");
            }

            for (int x = 0; x < array.GetLength(0); x += 2)
            {
                for (int y = 0; y < array.GetLength(1); y += 2)
                {
                    array[x, y] = (float)random.NextDouble();
                }
            }

            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    if(array[x, y] != 0)
                    {
                        float counter = 0;
                        float value = 0;

                        if(array[x - 1, y] != 0)
                        {
                            counter++;
                            value += array[x - 1, y];
                        }
                        if (array[x + 1, y] != 0)
                        {
                            counter++;
                            value += array[x + 1, y];
                        }
                        if (array[x, y - 1] != 0)
                        {
                            counter++;
                            value += array[x, y - 1];
                        }
                        if (array[x, y + 1] != 0)
                        {
                            counter++;
                            value += array[x, y + 1];
                        }

                        array[x, y] = value / counter;
                    }
                }
            }
        }
    }
}
