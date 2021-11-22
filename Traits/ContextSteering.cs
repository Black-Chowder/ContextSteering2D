using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spooky_Stealth.Utils;
using System.Diagnostics;

namespace Black_Magic
{
    public class ContextSteering : Entity
    {
        //Variables
        private int x = 150;
        private int y = 250;

        private int resolution = 16;
        private double[] contextMap;

        private double[] aMap;
        private double[] rMap;

        //Stores all context vectors applicable to this steering map
        private List<ContextVector> vectors;
        
        //Class to handle attraction and repulsion nodes
        public class ContextVector
        {
            public float x { get; set; }
            public float y { get; set; }
            public bool isAttraction { get; set; }

            public Vector2 pos
            {
                get
                {
                    return new Vector2(x, y);
                }
                set
                {
                    x = value.X;
                    y = value.Y;
                }
            }

            public ContextVector(float x, float y, bool isAttraction = true)
            {
                this.x = x;
                this.y = y;
                this.isAttraction = isAttraction;
            }
            public ContextVector(Vector2 vector)
            {
                this.pos = vector;
            }
        }

        //Visual Variables
        //Textures
        Texture2D backCirc;
        Texture2D inCirc;
        Texture2D weightTexture;
        Texture2D vectorText;

        //Drawing Specs
        private int radius = 250;
        private int circWidth = 8;

        private float weightWidth = 5;

        private float vectorRadius = 25f;


        private const string traitName = "contextSteering";
        public ContextSteering() : base(traitName, 250, 250)
        {
            contextMap = new double[resolution];
            vectors = new List<ContextVector>();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            double angle = Math.Atan2(mouse.Y - y, mouse.X - x);

            //Create New Vectors
            if (ClickHandler.leftClicked)
            {
                AddVector(new ContextVector(mouse.X, mouse.Y, true));
            }
            if (ClickHandler.rightClicked)
            {
                AddVector(new ContextVector(mouse.X, mouse.Y, false));
            }

            if (ClickHandler.IsClicked(Keys.R))
            {
                vectors.Clear();
            }

            Realize();
        }

        public double[] Realize()
        {
            //Instantiate maps
            aMap = new double[resolution]; // Attraction Map
            rMap = new double[resolution]; // Repulsion Map

            //Find Max
            double maxDist = 0d;
            for (int i = 0; i < vectors.Count; i++) //TODO: This can be made signifficantly more efficient
            {
                if (General.getDistance(new Vector2(x, y), vectors[i].pos) > maxDist)
                {
                    maxDist = General.getDistance(new Vector2(x, y), vectors[i].pos);
                }
            }

            //Calculate weight values for vectors
            for (int i = 0; i < vectors.Count; i++)
            {
                double[] modMap = vectors[i].isAttraction ? aMap : rMap;
                for (int j = 0; j < resolution; j++)
                {
                    double dot = General.dot(
                            Math.PI * 2 / resolution * j,
                            Math.Atan2(vectors[i].y - y, vectors[i].x - x));

                    if (dot > 0)
                    {
                        modMap[j] = Math.Max(modMap[j], dot * maxDist / General.getDistance(new Vector2(x, y), vectors[i].pos));
                    }
                }
            }

            //Combine Context Maps
            for (int i = 0; i < contextMap.Length; i++)
            {
                contextMap[i] = aMap[i] > rMap[i] ? aMap[i] : 0;
            }

            contextMap = Normalize(contextMap);

            return contextMap;
        }

        //Normalizes an array of doubles so that all values range between 1 and 0
        private static double[] Normalize(double[] weights)
        {
            //Find longest value in weights array
            double longestValue = 0d;
            for (int i = 0; i < weights.Length; i++)
            {
                if (longestValue < weights[i] && !double.IsInfinity(weights[i]))
                {
                    longestValue = weights[i];
                }
            }

            //If all are 0, just return given map
            if (longestValue == 0) { return weights; }

            //Calculate Scalar Value
            double scalar = 1 / longestValue;

            //Scale context map
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] *= scalar;
            }

            return weights;
        }

        //Adds vector to be accounted for
        public void AddVector(ContextVector cv)
        {
            vectors.Add(cv);
        }
        
        //Removes vector to be accounted for
        public void DeleteVector(ContextVector cv)
        {
            vectors.Remove(cv);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            //Create Background Circle
            //Textures
            backCirc ??= General.createCircleText(graphicsDevice, (int)radius);
            inCirc ??= General.createCircleText(graphicsDevice, (int)radius - circWidth);

            //Drawing
            spriteBatch.Draw(backCirc,
                new Vector2(x, y),
                new Rectangle(0, 0, radius, radius),
                Color.White,
                0f,
                new Vector2(radius / 2, radius / 2),
                1,
                SpriteEffects.None,
                0f);
            spriteBatch.Draw(inCirc,
                new Vector2(x, y),
                new Rectangle(0, 0, radius + circWidth / 2, radius + circWidth / 2),
                Color.Black,
                0f,
                new Vector2((radius - circWidth) / 2, (radius - circWidth)/ 2),
                1,
                SpriteEffects.None,
                0f);


            //Draw weight values
            weightTexture ??= General.createTexture(graphicsDevice);
            for (int i = 0; i < contextMap.Length; i++)
            {
                General.DrawLine(spriteBatch, weightTexture, new Vector2(x, y), (float)contextMap[i] * radius / 2, i * 2 * MathF.PI / contextMap.Length, contextMap[i] == 1 ? Color.Green : Color.DarkGreen, weightWidth);
                General.DrawLine(spriteBatch, weightTexture, new Vector2(x + radius, y), (float)Normalize(aMap)[i] * radius / 2, i * 2 * MathF.PI / contextMap.Length, Normalize(aMap)[i] == 1 ? Color.Green : Color.DarkGreen, weightWidth);
                General.DrawLine(spriteBatch, weightTexture, new Vector2(x + radius * 2, y), (float)Normalize(rMap)[i] * radius / 2, i * 2 * MathF.PI / contextMap.Length, Color.Red, weightWidth);
            }

            //Draw Attraction Vectors
            vectorText ??= General.createCircleText(graphicsDevice, (int)vectorRadius);
            for (int i = 0; i < vectors.Count; i++)
            {
                spriteBatch.Draw(vectorText,
                    new Vector2(vectors[i].x, vectors[i].y),
                    new Rectangle(0, 0, (int)vectorRadius, (int)vectorRadius),
                    vectors[i].isAttraction ? Color.Green : Color.Red,
                    0f,
                    new Vector2(vectorRadius / 2, vectorRadius / 2),
                    1,
                    SpriteEffects.None,
                    .1f);
            }
        }
    }
}
