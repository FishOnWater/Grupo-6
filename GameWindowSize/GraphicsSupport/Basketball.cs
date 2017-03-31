using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class BasketBall : TexturedPrimitive.TexturedPrimitive
    {
        // Change current position by this amount
        private const float kIncreaseRate = 1.001f;
        private Vector2 kInitSize = new Vector2(5, 5);
        private const float kFinalSize = 15f;
        public BasketBall(Vector2 position, float diameter) : base("BasketBall", position, new Vector2(diameter,diameter))
        {
            mSize = kInitSize;
        }
        public bool UpdateAndExplode()
        {
            mSize *= kIncreaseRate;
            return mSize.X > kFinalSize;
        }
    }
}
