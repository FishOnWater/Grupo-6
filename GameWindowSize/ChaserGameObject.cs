using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class ChaserGameObject: GameObject
    {
        //The target to go toward
        protected TexturedPrimitive.TexturedPrimitive mTarget;
        //Have we hit the target yet?
        protected bool mHitTarget;
        //How rapidly the chaser homes in on the target
        protected float mHomeInRate;

        public float HomeInRate { get { return mHomeInRate; } set { mHomeInRate = value; } }
        public bool HitTarget { get { return mHitTarget; } }
        public bool HasValidTarget { get { return null != mTarget; } }
        public TexturedPrimitive.TexturedPrimitive Target
        {
            get { return mTarget; }
            set
            {
                mTarget = value;
                mHitTarget = false;
                if (null != mTarget)
                {
                    FrontDirection = mTarget.mPosition - mPosition;
                    VelocityDirection = FrontDirection;
                }
            }
        }

        public ChaserGameObject(string imageName, Vector2 position, Vector2 size, TexturedPrimitive.TexturedPrimitive target) : base(imageName, position, size, null)
        {
            mTarget = target;
            mHomeInRate = 0.05f;
            mSpeed = 0.1f;
        }

        void UpdateGame(GameTime gameTime)
        {
            #region Step 4a.
            if (null == mTarget)
                return;
            //Move the GameObject in the velocity direction
            base.Update();
            #endregion

            #region step 4b.
            mHitTarget = PrimitivesTouches(mTarget);
            if (!mHitTarget)
            {
                #region Calculate angle
                Vector2 targetDir = mTarget.mPosition - mPosition;
                float distToTargetSq = targetDir.LengthSquared();
                targetDir /= (float)Math.Sqrt(distToTargetSq);
                float cosTheta = Vector2.Dot(FrontDirection, targetDir);
                float theta = (float)Math.Acos(cosTheta);
                #endregion

                #region Calculate rotation direction
                if (theta > float.Epsilon)
                {
                    //Not quite alligned
                    Vector3 fIn3D = new Vector3(FrontDirection, 0f);
                    Vector3 tIn3D = new Vector3(targetDir, 0f);
                    Vector3 sign = Vector3.Cross(tIn3D, fIn3D);

                    RotateAngleInRadian += Math.Sign(sign.Z) * theta * mHomeInRate;
                    VelocityDirection = FrontDirection;
                }
                #endregion
            }
            #endregion
        }
    }
}
