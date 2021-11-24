using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Black_Magic.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Black_Magic;

namespace ContextSteering2D
{
    public class TContextSteering : Trait
    {
        //Getters and setters for position of context steering
        public float x
        {
            get
            {
                return contextSteering.x;
            }
            set
            {
                contextSteering.x = value;
            }
        }
        public float y
        {
            get
            {
                return contextSteering.y;
            }
            set
            {
                contextSteering.y = value;
            }
        }

        //Getter for angle
        public double angle
        {
            get
            {
                return contextSteering.angle;
            }
        }

        //Stores context steering object
        private ContextSteering contextSteering;

        //Setting for if context steering center should automatically follow the parent entity
        public bool followParent { get; set; }

        //Draw Settings
        public int drawRadius = 250;
        public int weightWidth = 5;

        private const string id = "contextString";
        public TContextSteering(Entity parent, bool followParent = true) : base(id, parent)
        {
            this.followParent = followParent;

            contextSteering = new ContextSteering(parent.x, parent.y);
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

        public void SetVectors(List<ContextVector> vectors)
        {
            contextSteering.SetVectors(vectors);
        }

        //Draws Context Map
        public void DrawContextMap(SpriteBatch spriteBatch)
        {
            DrawContextMap(spriteBatch, contextSteering.x, contextSteering.y);
        }
        public void DrawContextMap(SpriteBatch spriteBatch, float x, float y)
        {
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
            double[] contextMap = contextSteering.GetRepulsionMap();
            for (int i = 0; i < contextMap.Length; i++)
            {
                General.DrawLine(spriteBatch, new Vector2(x, y), (float)contextMap[i] * drawRadius / 2, i * 2 * MathF.PI / contextMap.Length, Color.Red, weightWidth);
            }
        }
    }
}
