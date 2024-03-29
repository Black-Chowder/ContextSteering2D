﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Spooky_Stealth.Utils;
using Black_Magic.Utils;
using System.Diagnostics;
using Black_Magic;

namespace ContextSteering2D
{
    public class ContextSteerTester : Entity
    {
        TContextSteering contextSteering;

        List<ContextVector> vectors;

        //Texture Variables
        Texture2D backCirc;
        Texture2D inCirc;
        Texture2D vectorText;

        private int radius = 250;
        private int circWidth = 8;
        private float vectorRadius = 25f;

        private const string id = "contextSteerTester";
        public ContextSteerTester(float x = 600, float y = 150) : base(id, x, y)
        {
            contextSteering = new TContextSteering(this);

            vectors = new List<ContextVector>();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            double angle = Math.Atan2(mouse.Y - y, mouse.X - x);

            //Create New Vectors
            bool vectorsChanged = false; //Boolean to store if number of vectors changed
            if (ClickHandler.leftClicked)
            {
                ContextVector newVector = new ContextVector(mouse.X, mouse.Y, true);
                contextSteering.AddVector(newVector);
                vectors.Add(newVector);
                vectorsChanged = true;
            }
            if (ClickHandler.rightClicked)
            {
                ContextVector newVector = new ContextVector(mouse.X, mouse.Y, false);
                contextSteering.AddVector(newVector);
                vectors.Add(newVector);
                vectorsChanged = true;
            }

            if (ClickHandler.IsClicked(Keys.Space))
            {
                ContextVector newVector = new RelativeVector(contextSteering.GetContextSteering(), MathF.Atan2(mouse.Y - y, mouse.X - x), radius / 2);
                Debug.WriteLine(newVector.x);
                contextSteering.AddVector(newVector);
                vectors.Add(newVector);
                vectorsChanged = true;
            }

            if (ClickHandler.IsClicked(Keys.R))
            {
                contextSteering.ClearVectors();
                vectors.Clear();
                vectorsChanged = true;
            }

            //Update steering only if context vectors changed
            if (vectorsChanged) contextSteering.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            //Create Background Circle
            //Textures
            backCirc ??= General.createCircleText(graphicsDevice, (int)radius);
            inCirc ??= General.createCircleText(graphicsDevice, (int)radius - circWidth);

            //Draw background circle
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
                new Vector2((radius - circWidth) / 2, (radius - circWidth) / 2),
                1,
                SpriteEffects.None,
                0f);

            //Draw main context map
            contextSteering.drawRadius = radius;
            contextSteering.weightWidth = 5;
            contextSteering.DrawContextMap(spriteBatch);

            //Draw attraction and repulsion maps
            contextSteering.drawRadius = radius / 2;
            contextSteering.weightWidth = 2;
            contextSteering.DrawAttractionMap(spriteBatch, (int)(x - radius/3), (int)y + radius);
            contextSteering.DrawRepulsionMap(spriteBatch, (int)(x + radius/3), (int)y + radius);

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

            //Draw Angle Indicator
            spriteBatch.Draw(vectorText,
                new Vector2(x + MathF.Cos((float)contextSteering.angle) * radius / 2, y + MathF.Sin((float)contextSteering.angle) * radius / 2),
                new Rectangle(0, 0, (int)vectorRadius, (int)vectorRadius),
                Color.Blue,
                0f,
                new Vector2(vectorRadius / 2, vectorRadius / 2),
                .75f,
                SpriteEffects.None,
                .2f);

        }
    }
}
