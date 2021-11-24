using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spooky_Stealth.Entities;
using System.Diagnostics;
using Black_Magic.Utils;
using ContextSteering2D;

namespace Black_Magic
{
    public static class EntityHandler
    {
        public static List<Entity> entities;

        public static void Init()
        {
            entities = new List<Entity>();

            entities.Add(new ContextSteerTester());

            Random rand = new Random();
            
            
            //Spawn boid with visable context map
            entities.Add(new Boid(200, 200, true));

            //Spawn other boids
            for (int i = 0; i < 30; i++)
            {
                entities.Add(new Boid(rand.Next(0, 400), rand.Next(0, 400)));
            }
            
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw(spriteBatch, graphicsDevice);
            }
        }
    }
    public abstract class Entity
    {
        private List<Trait> traits;
        
        public string classId { get; protected set; }

        public float x;
        public float y;
        public float z;

        public float dx = 0;
        public float dy = 0;

        public float width;
        public float height;

        public bool isVisable = true;


        public Entity(string classId, float x, float y)
        {
            traits = new List<Trait>();
            this.classId = classId;
            this.x = x;
            this.y = y;
        }

        public void addTrait(Trait t)
        {
            traits.Add(t);

            //Sort Traits
            traits.Sort((a, b) => { return a.priority.CompareTo(b.priority); });
        }

        public T getTrait<T>() where T : Trait
        {
            foreach (Trait trait in traits)
                if (trait is T)
                    return (T)trait;
            return null;
        }

        public Trait getTrait(string name)
        {
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].name == name)
                {
                    return traits[i];
                }
            }
            return null;
        }

        public Boolean hasTrait(string name)
        {
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            traitUpdate(gameTime);
        }

        protected void traitUpdate(GameTime gameTime)
        {
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].isActive) traits[i].Update(gameTime);
            }
            if (MathF.Abs(dx) < .01f) dx = 0;
            if (MathF.Abs(dy) < .01f) dy = 0;
            x += dx;
            y += dy;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

        }
    }
}
