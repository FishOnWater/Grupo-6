using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TexturedPrimitive
{
    /// <summary>
    /// TexturedPrimitive class
    /// </summary>
    public class TexturedPrimitive
    {
        // Support for drawing the image
        protected Texture2D mImage;     // The UWB-JPG.jpg image to be loaded
        public Vector2 mPosition;    // Center position of image
        protected Vector2 mSize;        // Size of the image to be drawn
        protected float mRotateAngle; //In Radians, clock
        private Color[] mTextureColor = null;

        private void ReadColorData()
        {
            mTextureColor = new Color[mImage.Width * mImage.Height];
            mImage.GetData(mTextureColor);
        }

        private Color GetColor(int i, int j)
        {
            return mTextureColor[(j * mImage.Width) + 1];
        }

        public Vector2 MinBound
        {get{return mPosition-(0.5f * mSize);}}
        public Vector2 MaxBound { get { return mPosition + (0.5f * mSize); } }

        public float RotateAngleInRadian
        {
            get { return mRotateAngle; }
            set { mRotateAngle = value; }
        }

        /// <summary>
        /// Constructor of TexturePrimitive
        /// </summary>
        /// <param name="imageName">name of the image to be loaded as texture.</param>
        /// <param name="position">top left pixel position of the texture to be drawn</param>
        /// <param name="size">width/height of the texture to be drawn</param>
        public TexturedPrimitive(String imageName, Vector2 position, Vector2 size, String label=null)
        {
            mImage = Game1.Game1.sContent.Load<Texture2D>(imageName);
            mPosition = position;
            mSize = size;
            mRotateAngle = 0f;
            ReadColorData();
        }

        public bool PrimitivesTouches(TexturedPrimitive otherPrism)
        {
            Vector2 v = mPosition - otherPrism.mPosition;
            float dist = v.Length();
            return (dist < ((mSize.X / 2f) + (otherPrism.mSize.X / 2f)));
        }

        /// <summary>
        /// Allows the primitive object to update its state
        /// </summary>
        /// <param name="deltaTranslate">Amount to change the position of the primitive. 
        ///                              Value of 0 says position is not changed.</param>
        /// <param name="deltaScale">Amount to change of the scale the primitive. 
        ///                          Value of 0 says size is not changed.</param>
        public void Update(Vector2 deltaTranslate, Vector2 deltaScale, float deltaAngleInRadian)
        {
            mPosition += deltaTranslate;
            mSize += deltaScale;
            mRotateAngle += deltaAngleInRadian;
        }

        /// <summary>
        /// Draw the primitive
        /// </summary>
        public void Draw()
        {
            // Defines location and size of the texture
            Rectangle destRect = Game1.Camera.ComputePixelRectangle(mPosition, mSize);
            Game1.Game1.sSpriteBatch.Draw(mImage, destRect, Color.White);
            // Define the rotation origin
            Vector2 org = new Vector2(mImage.Width / 2, mImage.Height / 2);
            // Draw the texture
            Game1.Game1.sSpriteBatch.Draw(
                mImage,
                destRect,        // Area to be drawn in pixel space
                null,
                Color.White,
                mRotateAngle,   // Angle to rotate (clockwise)
                org,            // Image reference position
                SpriteEffects.None, 0f);
        }
    }
}
