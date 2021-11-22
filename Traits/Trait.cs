using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Black_Magic
{
    public abstract class Trait
    {
        public string name;
        public Entity parent;
        public bool isActive = true;
        public const byte defaultPriority = 100;
        public byte priority { get; protected set; } = defaultPriority;

        public Trait(String name, Entity parent)
        {
            this.name = name;
            this.parent = parent;
        }

        public abstract void Update(GameTime gameTime);
    }
}
