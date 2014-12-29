using Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Pacman
{
    class Level : GameWorld
    {
        public Level()
        {
            this.GameBoard = new GameBoard();
        }

        public override void Draw(DrawHelper drawHelper)
        {
            for (int x = 0; x < this.GameBoard.Size.X; x++)
            {
                for (int y = 0; y < this.GameBoard.Size.Y; y++)
                {
                    drawHelper.Translate(x, y);

                    switch (this.GameBoard.GetTileValue(new Point(x, y)))
                    {
                        case 1:
                            drawHelper.DrawBox(Color.Blue);
                            break;
                    }


                    drawHelper.Translate(-x, -y);
                }
            }
            
            base.Draw(drawHelper);
        }

        public void LoadLevel(string path)
        {
            Dictionary<string, string> file = Level.ParseFile(path);

            string[] textgrid = file["level"].Split(';');

            int width = textgrid[0].Length;
            int height = textgrid.Length - 1;

            short[,] grid = new short[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    char tile = textgrid[y][x];
                    short val = 0;

                    switch (tile) 
                    {
                        case '#':
                            val = 1;
                            break;
                    }

                    grid[x, y] = val;
                }
            }


            this.GameBoard.SetGrid(grid, new short[] { 1, 2 });
        }

        public static Dictionary<string, string> ParseFile(string path)
        {
            StreamReader r = new StreamReader(path);
            Dictionary<string, string> data = new Dictionary<string, string>();

            string line = r.ReadLine();
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

                line = r.ReadLine();

                if (line == null)
                    data[key] = value;
            }

            r.Close();
            return data;
        }
    }
}
