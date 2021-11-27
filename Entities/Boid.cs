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
        public static List<Boid> boids;

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

        private ContextVector cohesionVector;
        private RelativeVector alignmentVector;

        private const string id = "boid";
        public Boid(float x, float y, bool debug = false) : base(id, x, y)
        {
            width = size;
            height = size;

            //Establish boid list
            vectors ??= new List<ContextVector>();
            boids ??= new List<Boid>();

            boids.Add(this);

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
            //Update context vector position representation
            contextVector.x = x;
            contextVector.y = y;

            //Store vectors in range
            List<ContextVector> vectorsInRange = new List<ContextVector>();

            //Begin setting context vectors
            contextSteering.ClearVectors();


            //Separation Logic
            //Add repelant context vectors for other boids
            foreach (ContextVector cv in vectors)
            {
                //Don't include self in vector calculations
                if (cv != contextVector && General.getDistance(contextVector.pos, cv.pos) <= range)
                {
                    contextSteering.AddVector(cv);
                    vectorsInRange.Add(cv);
                }
            }

            //Alignment Logic
            //Calculate average angle of nearby boids
            double avgAngle = angle;
            int boidInRangeCount = 0;
            foreach (Boid boid in boids)
            {
                if (boid != this && General.getDistance(contextVector.pos, new Vector2(boid.x, boid.y)) <= range)
                {
                    avgAngle += boid.angle;
                    boidInRangeCount++;
                }
            }
            avgAngle /= boidInRangeCount;

            //Instantiate alignment vector as relative vector
            alignmentVector = new RelativeVector(contextSteering.GetContextSteering(), (float)avgAngle, range, true);

            //Add alignment vector to context steering's list of vectors
            contextSteering.AddVector(alignmentVector);


            //Cohesion Logic
            //Calculate average position among context vectors in range
            float avgX = 0;
            float avgY = 0;
            foreach (ContextVector cv in vectorsInRange)
            {
                avgX += cv.x;
                avgY += cv.y;
            }
            avgX /= vectorsInRange.Count;
            avgY /= vectorsInRange.Count;

            //Set cohesion vector equal to gotten average
            cohesionVector = new ContextVector(avgX, avgY, true);

            //Set strength so that going to this vector is more heavily desired than fleeing other boids
            //Makes sure this behavior isn't "overwhelmed" by the amount of repeling context vectors
            cohesionVector.strength = vectorsInRange.Count / 9d; //Note that constant is chosen arbitrarily to "look good"

            //Add cohesion vector to context steering's list of vectors
            contextSteering.AddVector(cohesionVector);


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

                //Draw Alignment Vector
                General.DrawLine(spriteBatch, alignmentVector.pos, new Vector2(x, y), Color.Blue, 1);

                //Draw Cohesion Vector
                spriteBatch.Draw(texture,
                    cohesionVector.pos,
                    new Rectangle(0, 0, 1, 1),
                    Color.Green,
                    0,
                    new Vector2(0, 0),
                    size,
                    SpriteEffects.None,
                    0);
            }
        }
    }
}
