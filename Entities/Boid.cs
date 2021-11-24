using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Black_Magic.Utils;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace ContextSteering2D
{
    public class Boid : Entity
    {
        public static List<ContextVector> vectors;
        //public static List<Boid> boids;

        TContextSteering contextSteering;
        public ContextVector contextVector;

        public double angle = 0d;
        public double angleSpeed = Math.PI / 180d * 3; //3 degrees every frame

        public float speed = 2f;

        //Texture Variables
        private static Texture2D texture;

        private static float size = 5;

        public bool debug;

        public float range = 200;

        private const string id = "boid";
        public Boid(float x, float y, bool debug = false) : base(id, x, y)
        {
            width = size;
            height = size;

            //Establish boid list
            vectors ??= new List<ContextVector>();

            contextVector = new ContextVector(x, y, false);
            vectors.Add(contextVector);

            contextSteering = new TContextSteering(this);
            contextSteering.drawRadius = 100;
            contextSteering.weightWidth = 1;
            addTrait(contextSteering);

            this.debug = debug;
        }

        public override void Update(GameTime gameTime)
        {

            //Set context vectors
            contextSteering.ClearVectors();
            foreach (ContextVector cv in vectors)
            {
                //Don't include self in vector calculations
                if (cv != contextVector)
                {
                    contextSteering.AddVector(cv);
                }
            }

            //Update context vector position representation
            contextVector.x = x;
            contextVector.y = y;

            //Move delta pos according to angle
            angle += angle - contextSteering.angle < 0 ? angleSpeed : -angleSpeed;
            dx += MathF.Cos((float)angle) * speed;
            dy += MathF.Sin((float)angle) * speed;

            base.Update(gameTime);

            //Bounding Teleportation
            if (x < 0)
            {
                x = 400;
            }
            else if (x > 400)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 400;
            }
            else if (y > 400)
            {
                y = 0;
            }
            

            //Reset delta position
            dx = 0;
            dy = 0;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            texture ??= General.createTexture(graphicsDevice);

            spriteBatch.Draw(texture,
                new Vector2(x - width / 2, y - height / 2),
                new Rectangle(0, 0, 1, 1),
                debug ? Color.Blue : Color.Red,
                0,
                new Vector2(0, 0),
                size,
                SpriteEffects.None,
                0);

            if (debug)
            {
                contextSteering.DrawContextMap(spriteBatch, x, y);
                contextSteering.DrawRepulsionMap(spriteBatch);
            }
        }
    }
}
