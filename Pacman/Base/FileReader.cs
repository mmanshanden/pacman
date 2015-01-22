using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Base
{
    public class FileReader
    {
        Dictionary<string, string> data;

        public FileReader(string path)
        {
            this.data = FileReader.ParseFile(path);
        }

        public Vector2 ReadVector(string key)
        {
            string[] values = this.data[key].Split(';');
            Vector2 vector = new Vector2();
            vector.X = this.ParseFloat(values[0]);
            vector.Y = this.ParseFloat(values[1]);

            return vector;
        }

        // do not rely on "Culture settings"
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
                    if (key != "")
                    {
                        data[key] = value;
                        value = "";
                    }

                    key = parts[0];
                    value += parts[1];
                }
                else
                {
                    value += line + ';';
                }

                line = reader.ReadLine();

                if (line == null)
                    data[key] = value;
            }

            reader.Close();
            return data;
        }
    }
}
