using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Black_Magic.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Black_Magic
{
    public class TContextSteering : Trait
    {
        public float x
        {
            get
            {
                return contextSteering.x;
            }
        }
        public float y
        {
            get
            {
                return contextSteering.y;
            }
        }

        //Stores context steering object
        private ContextSteering contextSteering;

        public bool followParent = false;

        //Texture variables
        static Texture2D weightTexture;
        static Texture2D vectorTexture;

        //Draw Settings
        private int drawRadius = 250;
        private int weightWidth = 5;

        private const string id = "contextString";
        public TContextSteering(Entity parent) : base(id, parent)
        {
            contextSteering = new ContextSteering(parent.x, parent.y);
            Debug.WriteLine("parent x = " + parent.x);
            Debug.WriteLine("Context Steering x = " + contextSteering.x);
        }

        public override void Update(GameTime gameTime)
        {
            if (followParent)
            {
                contextSteering.x = (int)parent.x;
                contextSteering.y = (int)parent.y;
            }

            contextSteering.Realize();
        }

        public void AddVector(ContextVector cv)
        {
            contextSteering.AddVector(cv);
        }

        public void DeleteVector(ContextVector cv)
        {
            contextSteering.DeleteVector(cv);
        }

        public void ClearVectors()
        {
            contextSteering.ClearVectors();
        }

        //Draws Context Map
        public void DrawContextMap(SpriteBatch spriteBatch)
        {
            DrawContextMap(spriteBatch, contextSteering.x, contextSteering.y);
        }
        public void DrawContextMap(SpriteBatch spriteBatch, float x, float y)
        {
            //Generate texture if needed
            weightTexture ??= General.createTexture(spriteBatch.GraphicsDevice);

            double[] contextMap = contextSteering.GetContextMap();
            for (int i = 0; i < contextMap.Length; i++)
            {
                General.DrawLine(spriteBatch, new Vector2(x, y), (float)contextMap[i] * drawRadius / 2, i * 2 * MathF.PI / contextMap.Length, contextMap[i] == 1 ? Color.Green : Color.DarkGreen, weightWidth);
            }
        }

        //Draws Attraction Map
        public void DrawAttractionMap(SpriteBatch spriteBatch)
        {
            DrawAttractionMap(spriteBatch, contextSteering.x, contextSteering.y);
        }
        public void DrawAttractionMap(SpriteBatch spriteBatch, float x, float y)
        {
            //Generate texture if needed
            weightTexture ??= General.createTexture(spriteBatch.GraphicsDevice);

            double[] contextMap = contextSteering.GetAttractionMap();
            for (int i = 0; i < contextMap.Length; i++)
            {
                General.DrawLine(spriteBatch, new Vector2(x, y), (float)contextMap[i] * drawRadius / 2, i * 2 * MathF.PI / contextMap.Length, contextMap[i] == 1 ? Color.Green : Color.DarkGreen, weightWidth);
            }
        }

        //Draws Repulsion Map
        public void DrawRepulsionMap(SpriteBatch spriteBatch)
        {
            DrawRepulsionMap(spriteBatch, contextSteering.x, contextSteering.y);
        }
        public void DrawRepulsionMap(SpriteBatch spriteBatch, float x, float y)
        {
            //Generate texture if needed
            weightTexture ??= General.createTexture(spriteBatch.GraphicsDevice);

            double[] contextMap = contextSteering.GetRepulsionMap();
            for (int i = 0; i < contextMap.Length; i++)
            {
                General.DrawLine(spriteBatch, new Vector2(x, y), (float)contextMap[i] * drawRadius / 2, i * 2 * MathF.PI / contextMap.Length, Color.Red, weightWidth);
            }
        }
    }
}
