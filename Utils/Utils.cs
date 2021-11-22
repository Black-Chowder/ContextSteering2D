using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Black_Magic.Utils
{
    static class General
    {
        public static Boolean pointRectCollision(float pointX, float pointY, float x, float y, float w, float h)
        {
            if (x < pointX && pointX < x + w && y < pointY && pointY < y + h)
            {
                return true;
            }

            return false;
        }

        public static Boolean rectCollision(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            return (
                y1 + h1 > y2 &&
                y1 < y2 + h2 &&
                x1 < x2 + w2 &&
                x1 + w1 > x2
                );
        }


        public static Boolean rectCollision2(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            if (w1 < w2)
            {
                if ((x1 > x2 && x1 < x2 + w2) || (x1 + w1 > x2 && x1 + w1 < x2 + w2))
                {
                    if (h1 < h2)
                    {
                        if ((y1 >= y2 && y1 <= y2 + h2) || (y1 + h1 >= y2 && y1 + h1 <= y2 + h2))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if ((y2 >= y1 && y2 <= y1 + h1) || (y2 + h2 >= y1 && y2 + h2 <= y1 + h1))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if ((x2 > x1 && x2 < x1 + w1) || (x2 + w2 > x1 && x2 + w2 < x1 + w1))
                {
                    if (h1 < h2)
                    {
                        if ((y1 >= y2 && y1 <= y2 + h2) || (y1 + h1 >= y2 && y1 + h1 <= y2 + h2))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if ((y2 >= y1 && y2 <= y1 + h1) || (y2 + h2 >= y1 && y2 + h2 <= y1 + h1))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static float getDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static float getDistance(Vector2 point1, Vector2 point2)
        {
            return getDistance(point1.X, point1.Y, point2.X, point2.Y);
        }


        //Credit: http://csharphelper.com/blog/2016/09/find-the-shortest-distance-between-a-point-and-a-line-segment-in-c/
        public static double FindDistanceToSegment(Vector2 pt, Vector2 p1, Vector2 p2, out Vector2 closest)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
                (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new Vector2(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new Vector2(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new Vector2(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static float DegToRad(float deg)
        {
            return deg * MathF.PI / 180;
        }

        public static float RadToDeg(float rad)
        {
            return rad * 180 / MathF.PI;
        }

        //Gets closest entity in a list
        public static Entity getClosestEntity(Entity self, List<Entity> entities, out float dist)
        {
            //Edge Case: No Entities
            if (entities.Count == 0)
            {
                dist = float.PositiveInfinity;
                return null;
            }

            Entity closest = entities[0];
            dist = getDistance(self.x, self.y, closest.x, closest.y);

            for (int i = 1; i < entities.Count; i++)
            {
                float entityDist = getDistance(self.x, self.y, entities[i].x, entities[i].y);
                if (entityDist < dist)
                {
                    closest = entities[i];
                    dist = entityDist;
                }
            }
            
            return closest;
        }

        /// <summary>
        /// Loads Sprite Sheet Into List Of Rectangles To Be Used 
        /// As Parameters In spriteBatch.Draw() as the parameter sourceRectangle.  
        /// 
        /// Example:
        /// init(){
        ///     Rectangle[] sprites = new Rectangle[200];
        ///     sprites = spriteSheetLoader(32, 32, 320, 320);
        /// }
        /// draw(){
        ///     spriteBatch.Begin();
        ///     spriteBatch.Draw(texture, 
        ///         destinationRectangle: new Rectangle(this.x, this.y, this.width, this.height), 
        ///         sourceRectangle: sprites[0],   <<<======
        ///         color: Color.White);
        ///     spriteBatch.End();
        /// }
        /// </summary>
        /// <param name="spriteWidth"></param>
        /// <param name="spriteHeight"></param>
        /// <param name="spriteSheetWidth"></param>
        /// <param name="spriteSheetHeight"></param>
        /// <returns> Rectangle[] </returns>
        public static Rectangle[] spriteSheetLoader(int spriteWidth, int spriteHeight, int columns, int rows)
        {
            int spritesInSpriteSheet = columns * rows;

            Rectangle[] placeholder = new Rectangle[0];
            return spriteSheetLoader(placeholder, spriteWidth, spriteHeight, columns, rows, 0, spritesInSpriteSheet, false);
        }
        public static Rectangle[] spriteSheetLoader(int spriteWidth, int spriteHeight, int columns, int rows, int endingSprite)
        {
            Rectangle[] placeholder = new Rectangle[0];
            return spriteSheetLoader(placeholder, spriteWidth, spriteHeight, columns, rows, 0, endingSprite, false);
        }
        public static Rectangle[] spriteSheetLoader(int spriteWidth, int spriteHeight, int columns, int rows, int startingSprite, int endingSprite)
        {
            Rectangle[] placeholder = new Rectangle[0];
            return spriteSheetLoader(placeholder, spriteWidth, spriteHeight, columns, rows, startingSprite, endingSprite, false);
        }
        public static Rectangle[] spriteSheetLoader(int spriteWidth, int spriteHeight, int columns, int rows, int startingSprite, int endingSprite, Boolean inReverse)
        {
            Rectangle[] placeholder = new Rectangle[0];
            return spriteSheetLoader(placeholder, spriteWidth, spriteHeight, columns, rows, startingSprite, endingSprite, inReverse);
        }
        public static Rectangle[] spriteSheetLoader(Rectangle[] spriteSheet, int spriteWidth, int spriteHeight, int columns, int rows, int startingSprite, int endingSprite, Boolean inReverse)
        {
            //TODO: Implement Starting Sprite Functionality!!! <<<========  High Priority
            Rectangle[] toReturn = new Rectangle[spriteSheet.Count() + Math.Abs(endingSprite - startingSprite)];

            Boolean wantToBreak = false;
            int spriteCounter = 0;

            //Writes loaded spriteSheet to toReturn
            for (int i = 0; i < spriteSheet.Count(); i++)
            {
                toReturn[i] = spriteSheet[i];
            }

            //FOR GOING NORMAL DIRECTION
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (spriteCounter >= startingSprite)
                    {
                        toReturn[(spriteCounter - startingSprite) + spriteSheet.Count()] = new Rectangle(
                            x * spriteWidth, 
                            y * spriteHeight, 
                            spriteWidth, 
                            spriteHeight);
                    }
                    if (spriteCounter + 2 > endingSprite)
                    {
                        wantToBreak = true;
                        break;
                    }

                    spriteCounter++;
                }
                if (wantToBreak)
                {
                    break;
                }
            }

            if (inReverse)
            {
                Rectangle[] reverseReturn = new Rectangle[toReturn.Count()];

                //Loads previous sprite sheet
                for (int i = 0; i < spriteSheet.Count(); i++)
                {
                    reverseReturn[i] = spriteSheet[i];
                }

                //Reverses new sprites
                for (int i = spriteSheet.Count(); i < toReturn.Count(); i++)
                {
                    reverseReturn[reverseReturn.Count() + spriteSheet.Count() - i - 1] = toReturn[i];
                }
                return reverseReturn;
            }

            return toReturn;
        }

        //Draws Lines
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            return createTexture(spriteBatch.GraphicsDevice);
                //new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, texture, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, .5f);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(texture, point, null, color, angle, origin, scale, SpriteEffects.None, .5f);
        }



        public static Texture2D createCircleText(GraphicsDevice GraphicsDevice, int radius)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        private static Texture2D texture = null;
        public static Texture2D createTexture(GraphicsDevice graphicsDevice)
        {
            if (texture != null) return texture;
            texture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });
            return texture;
        }

        public static Rectangle createRectangle(float x, float y, float width, float height)
        {
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public const int Left = 0;
        public const int Right = 1;
        public const int Top = 2;
        public const int Bottom = 3;

        public static void Print2DArray(bool[,] array)
        {
            if (array.GetLength(0) == 0 || array.GetLength(1) == 0) throw new ArgumentOutOfRangeException(nameof(array), "was empty");
            for (int row = 0; row < array.GetLength(1); row++)
            {
                for (int col = 0; col < array.GetLength(0); col++)
                {
                    System.Diagnostics.Debug.Write((array[col, row] ? 1 : 0) + ", ");
                }
                System.Diagnostics.Debug.Write("\n");
            }
        }

        //Converts string in format {R:___ G:___ B:___ A:___} to Color value
        public static Color RGBAToColor(string rgba) //TODO: Add error handling
        {
            //Setup local variables
            string rawR = "";
            string rawG = "";
            string rawB = "";
            string rawA = "";
            byte j = 0; // <= To keep track of which (rgba) is being modified

            //Loop through characters of string
            for (int i = 0; i < rgba.Length;)
            {
                //Skip any variables taht aren't numbers
                if (!byte.TryParse(rgba[i].ToString(), out _))
                {
                    i++;
                    continue;
                }

                //Locate and parse number segments into corresponding string variables
                while (i < rgba.Length && byte.TryParse(rgba[i].ToString(), out _))
                {
                    switch (j)
                    {
                        case 0:
                            rawR += rgba[i].ToString();
                            break;
                        case 1:
                            rawG += rgba[i].ToString();
                            break;
                        case 2:
                            rawB += rgba[i].ToString();
                            break;
                        case 3:
                            rawA += rgba[i].ToString();
                            break;
                    }
                    i++;
                }
                j++;
            }

            //Convert raw string values to integers
            byte r = byte.Parse(rawR);
            byte g = byte.Parse(rawG);
            byte b = byte.Parse(rawB);
            byte a = byte.Parse(rawA);

            //Create new color
            Color color = new Color(r, g, b, a);

            //Return value
            return color;
        }

        //Dot Product Calculators
        public static double dot(double x1, double y1, double x2, double y2)
        {
            double normX1 = x1 / (x1 == 0 && y1 == 0 ? 0 : Math.Sqrt(x1 * x1 + y1 * y1));
            double normY1 = y1 / (x1 == 0 && y1 == 0 ? 0 : Math.Sqrt(x1 * x1 + y1 * y1));

            double normX2 = x2 / (x2 == 0 && y2 == 0 ? 0 : Math.Sqrt(x2 * x2 + y2 * y2));
            double normY2 = y2 / (x2 == 0 && y2 == 0 ? 0 : Math.Sqrt(x2 * x2 + y2 * y2));


            return normX1 * normX2 + normY1 * normY2;
        }

        public static double dot(double a1, double a2)
        {
            return Math.Cos(a1) * Math.Cos(a2) + Math.Sin(a1) * Math.Sin(a2);
        }
    }
}
