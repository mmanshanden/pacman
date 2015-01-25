using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Base
{
    /// <summary>
    /// Reads a .txt level file.
    /// </summary>

    public class FileReader
    {
        Dictionary<string, string> data;

        public FileReader(string path)
        {
            this.data = FileReader.ParseFile(path);
        }

        /// <summary>
        /// Converts two floats separated by a ';' into a
        /// vector2.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Vector2 ReadVector(string key)
        {
            string[] values = this.data[key].Split(';');
            Vector2 vector = new Vector2();
            vector.X = this.ParseFloat(values[0]);
            vector.Y = this.ParseFloat(values[1]);

            return vector;
        }

        /// <summary>
        /// Parse a float. Always uses dot as decimal separator 
        /// because float.Parse() could use either a comma or a
        /// dot depending on "Culture Settings".
        /// </summary>
        /// <param name="s">String containing float</param>
        /// <returns></returns>
        private float ParseFloat(string s)
        {
            string[] parts = s.Split('.');

            if (parts.Length == 1)
                return float.Parse(s);

            float precomma = float.Parse(parts[0]);
            float postcomma = float.Parse(parts[1]);

            postcomma /= (float)Math.Pow(10, parts[1].Length);

            return precomma + postcomma;
        }

        public string ReadString(string key)
        {
            return this.data[key];
        }

        public float ReadFloat(string key)
        {
            return this.ParseFloat(this.data[key]);
        }

        /// <summary>
        /// Returns the string associated to given key
        /// ad a 2d char array. The ';' symbol is used
        /// for line separation.
        /// </summary>
        /// <param name="key">Key in dictionary</param>
        /// <returns>2d char array</returns>
        public char[,] ReadGrid(string key)
        {
            string[] lines = data[key].Split(';');

            int width = lines[0].Length;
            int height = lines.Length - 1;

            char[,] grid = new char[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = lines[y][x];
                }
            }

            return grid;
        }

        /// <summary>
        /// Parses the file. Stores the key and value pair in a
        /// dictionary. A key value pair is read as
        /// "key=value"
        /// where key is the key and value is the value.
        /// Every line forms a new pair, unless the line
        /// doesn't contain the "=" symbol. If this is the
        /// case, every line extends the value until a line
        /// containing the "=" symbol is reached.
        /// </summary>
        /// <param name="path">Path to level file</param>
        /// <returns>Key value pairs contained in file</returns>
        public static Dictionary<string, string> ParseFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            Dictionary<string, string> data = new Dictionary<string, string>();

            string line = reader.ReadLine();
            string key = "";
            string value = "";

            while (line != null)
            {
                string[] parts = line.Split('=');

                if (line.Contains("="))
                {
                    // we have a key from a previous line.
                    // value is complete.
                    if (key != "")
                    {
                        data[key] = value;
                        value = ""; // recycle value 
                    }

                    key = parts[0];
                    value += parts[1];
                }
                else
                {
                    // Add line to value.
                    // Lines are seperated by ";" in resulting
                    // dictionary..
                    value += line + ';';
                }

                // read next line
                line = reader.ReadLine();

                // loop ends if next line is equal to null
                // save current key value pair if that is
                // the case.
                if (line == null)
                    data[key] = value;
            }

            reader.Close();
            return data;
        }
    }
}
