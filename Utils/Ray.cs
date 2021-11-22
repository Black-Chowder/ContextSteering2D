using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Black_Magic;
using Microsoft.Xna.Framework.Graphics;

/*
namespace Black_Magic.Utils
{
    public class Ray
    {
        public Vector2 pos;
        public float angle;

        private Vector2? tip;
        private Entity closestEntity = null;

        public const int maxSteps = 100;
        
        //Constructors
        public Ray(float x, float y, float angle = 0f)
        {
            this.pos = new Vector2(x, y);
            this.angle = angle;
        }

        public Ray(Vector2 pos, float angle = 0f)
        {
            this.pos = pos;
            this.angle = angle;
        }

        public Ray(Vector3 data)
        {
            this.pos = new Vector2(data.X, data.Y);
            this.angle = data.Z;
        }

        //Casts ray to be colliding with all rigidbodies in given list
        public Vector2? cast(List<Rigidbody> rigidbodies, Rigidbody self)
        {
            Vector2 rayTip = new Vector2(pos.X, pos.Y);

            for (int i = 0; i < maxSteps; i++)
            {
                //Step 1: get shortest distance
                float shortestDist = float.PositiveInfinity;
                foreach (Rigidbody rigidbody in rigidbodies)
                {
                    //Skips entity if any of the following criteria is met:
                    if (rigidbody == self ||
                        self.ignoreTypes.Contains(rigidbody.parent.GetType().BaseType) ||
                        self.ignoreTypes.Contains(rigidbody.parent.GetType())) continue;

                    float distance = rigidbody.getDistance(rayTip);
                    if (distance < shortestDist)
                    {
                        shortestDist = distance;
                        closestEntity = rigidbody.parent;
                    }
                }

                //Step 2: Check if touching object (or really really close)
                if (shortestDist < .01) // ARBITRAIRILY SMALL NUMBER (in pixel units)
                {
                    tip = new Vector2(rayTip.X, rayTip.Y);
                    return tip;
                }

                //Step 3: Extend Tip
                Vector2 extension = new Vector2((float)(shortestDist * Math.Cos(angle)), (float)(shortestDist * Math.Sin(angle)));
                rayTip += extension;

                //Repeat
            }
            tip = null;
            return null;
        }

        //Casts ray to be colliding with all entities in given list
        public Vector2? cast(List<Entity> entities, Entity self)
        {
            Vector2 rayTip = new Vector2(pos.X, pos.Y);

            //Get Self Rigidbody
            Rigidbody selfRB = (Rigidbody)self.getTrait("rigidbody");

            for (int i = 0; i < maxSteps; i++)
            {
                //Step 1: get shortest distance
                float shortestDist = float.PositiveInfinity;
                foreach (Entity entity in entities)
                {
                    //Skips entity if any of the following criteria is met:
                    if (!entity.hasTrait("rigidbody") //Doesn't have rigidbody trait
                        || entity == self //Is itself
                        || selfRB.ignoreTypes.Contains(entity.GetType().BaseType) //Self is supposed to ignore entity base type
                        || selfRB.ignoreTypes.Contains(entity.GetType())) //Self is supposed to ignore entity type
                            continue;

                    //Gets entitie's rigidbody trait
                    Rigidbody rigidbody = (Rigidbody)entity.getTrait("rigidbody");

                    float distance = rigidbody.getDistance(rayTip);
                    if (distance < shortestDist)
                    {
                        shortestDist = distance;
                        closestEntity = entity;
                    }
                }
                
                //Step 2: Check if touching object (or really really close)
                if (shortestDist < .01)//ARBITRAIRILY SMALL NUMBER (in pixel units)
                {
                    tip = new Vector2(rayTip.X, rayTip.Y);
                    return tip;
                }

                //Step 3: Extend Point
                Vector2 extension = new Vector2((float)(shortestDist*Math.Cos(angle)), (float)(shortestDist*Math.Sin(angle)));
                rayTip += extension;
                
                //Repeat
            }
            tip = null;
            return null;
        }

        //Gets entity that the ray collides with (can be null, if not colliding with anything)
        public Entity getEntity()
        {
            return closestEntity;
        }

        public void drawRay(SpriteBatch spriteBatch)
        {
            Vector2 line = new Vector2((float)(10000*Math.Cos(angle)), (float)(10000*Math.Sin(angle)));
            General.DrawLine(spriteBatch, pos, new Vector2(pos.X + line.X, pos.Y + line.Y), Color.White);
            if (tip != null)
            {
                General.DrawLine(spriteBatch, pos, tip.Value, Color.White);
            }
        }

        public void drawPoint(SpriteBatch spriteBatch, Texture2D circle)
        {
            spriteBatch.Draw(circle, new Rectangle((int)(pos.X - 1), (int)(pos.Y - 1), 2, 2), Color.White);
        }

        public List<Vector3> debugDrawRay(List<Entity> entities, Entity self)
        {
            List<Vector3> toReturn = new List<Vector3>();
            Color debugColor = new Color(255, 255, 255, 100);
            Vector2 rayTip = new Vector2(pos.X, pos.Y);
            for (int i = 0; i < 100; i++)
            {
                //Step 1:
                float shortestDist = float.PositiveInfinity;
                foreach (Entity entity in entities)
                {
                    if (!entity.hasTrait("rigidbody") || entity == self) continue;
                    Rigidbody rigidbody = (Rigidbody)entity.getTrait("rigidbody");
                    float dist = rigidbody.getDistance(rayTip);
                    
                    if (dist < shortestDist)
                    {
                        shortestDist = dist;
                    }

                }
                if (shortestDist < 1000)
                {
                    toReturn.Add(new Vector3(rayTip.X, rayTip.Y, shortestDist));
                }

                //Step 2:
                if (shortestDist < .0001)
                {
                    break;
                }

                //Step 3:
                Vector2 extension = new Vector2((float)(shortestDist * Math.Cos(angle)), (float)(shortestDist * Math.Sin(angle)));
                rayTip.X += extension.X;
                rayTip.Y += extension.Y;
            }

            return toReturn;
        }
    }
}
*/
