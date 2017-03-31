using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1
{
    class MyGame2
    {
        TexturedPrimitive.TexturedPrimitive mHero;
        Vector2 kHeroSize = new Vector2(15, 15);
        Vector2 kHeroPosition = Vector2.Zero;

        //Size of all the positions
        Vector2 kPointSize = new Vector2(5f, 5f);

        List<BasketBall> mBBallList;
        TimeSpan mCreationTimeSpan;
        int mTotalBBallCreated = 0;
        //this is 0.5 seconds
        const int kBallMSecInterval = 500;

        //Rocket support
        Vector2 mRocketInDirection = Vector2.UnitY; //this does not change
        TexturedPrimitive.TexturedPrimitive mRocket;

        //Support the flying net
        TexturedPrimitive.TexturedPrimitive mNet;
        bool mNetInFlight = false;
        Vector2 mNetVelocity = Vector2.Zero;
        float mNetSpeed = 0.5f;

        //Insect support
        TexturedPrimitive.TexturedPrimitive mInsect;
        bool mInsectPreset = true;

        //Simple game status
        int mNumInsectShot;
        int mNumMissed;

        Vector2 kIntRocketPosition = new Vector2(10, 10);
        //Rocket support
        GameObject gRocket;
        //the arrow
        GameObject mArrow;

        ChaserGameObject mChaser;
        //Simple game status
        int mChaserHit, mChaserMissed;

        int mScore = 0;
        int mBBallMissed = 0, mBBallHit = 0;
        TexturedPrimitive.TexturedPrimitive mBall;
        TexturedPrimitive.TexturedPrimitive mUWBLogo;
        TexturedPrimitive.TexturedPrimitive mWorkPrim;
        TexturedPrimitive.TexturedPrimitive mPa, mPb; //The locators for showing Point A and Ponit B
        TexturedPrimitive.TexturedPrimitive mPx;      //To show same displacement can be applied to any position
        TexturedPrimitive.TexturedPrimitive mPy;      //To show we can rotate/manipukate vectors indenpendently
        Vector2 mVectorAyPv = new Vector2(10, 0);     //Start with vector in the X direction
        TexturedPrimitive.TexturedPrimitive mCurrentLocator;
        const int kBBallTouchScore = 1;
        const int KBBallMissedScore = -2;
        const int kWinScore = 10;
        const int kLossScore= -10;
        TexturedPrimitive.TexturedPrimitive mFinal = null;

        public MyGame2()
        {
            mHero = new TexturedPrimitive.TexturedPrimitive("Me", kHeroPosition, kHeroSize);
            mCreationTimeSpan = new TimeSpan(0);
            mBBallList = new List<BasketBall>();
        }

        void  GameState()
        {
            //Create the primitives
            mBall = new TexturedPrimitive.TexturedPrimitive("Soccer", new Vector2(30, 30), new Vector2(10, 15));
            mUWBLogo = new TexturedPrimitive.TexturedPrimitive("UWB-JPG", new Vector2(60, 30), new Vector2(20, 20));
            mWorkPrim = mBall;
            mRocket = new TexturedPrimitive.TexturedPrimitive("Rocket", new Vector2(5, 5), new Vector2(3, 10));
            //Initially the rocket is pointing in the positive y direction
            mRocketInDirection = Vector2.UnitY;
            mNet = new TexturedPrimitive.TexturedPrimitive("Net", new Vector2(0, 0), new Vector2(2, 5));
            mNetInFlight = false; //until user press "A", rocket is not in flight
            mNetVelocity = Vector2.Zero;
            mNetSpeed = 0.5f;

            //Initialize a new insect
            mInsect = new TexturedPrimitive.TexturedPrimitive("Insect", Vector2.Zero, new Vector2(5, 5));
            mInsectPreset = false;

            //Initialize game status
            mNumInsectShot = 0;
            mNumMissed = 0;
            //Create other primitives
            mPa = new TexturedPrimitive.TexturedPrimitive("Position", new Vector2(30, 30), kPointSize, "Pa");
            mPb = new TexturedPrimitive.TexturedPrimitive("Position", new Vector2(60, 30), kPointSize, "Pb");
            mPx = new TexturedPrimitive.TexturedPrimitive("Position", new Vector2(20, 10), kPointSize, "Px");
            mPy = new TexturedPrimitive.TexturedPrimitive("Position", new Vector2(20, 50), kPointSize, "Py");
            mCurrentLocator = mPa;

            gRocket = new GameObject("Rocket", kIntRocketPosition, new Vector2(3, 10));
            mArrow = new GameObject("RightArrow", new Vector2(50, 30), new Vector2(10, 4));
            //Initialy pointing in the x direction
            mArrow.InitialFrontDirection = Vector2.UnitX;

            mChaser = new ChaserGameObject("Chaser", Vector2.Zero, new Vector2(6f, 1.7f), null);
            //Initialy facing in the negative x direction
            mChaser.InitialFrontDirection = -Vector2.UnitX;
            mChaser.Speed = 0.2f;
            mChaserHit = 0;
            mChaserMissed = 0;
        }

        public void UpdateGame(GameTime gameTime)
        {
            #region Step a.
            if (null != mFinal) //Done!!
                return;
            #endregion Step a.

            #region Step b.
            //Hero movement: right thumb stick
            mHero.Update(InputWrapper.InputWrapper.ThumbSticks.Right, InputWrapper.InputWrapper.ThumbSticks.Left);
            //Basketball...
            for(int b=mBBallList.Count-1; b>=0; b--)
            {
                if (mBBallList[b].UpdateAndExplode())
                {
                    mBBallList.RemoveAt(b);
                    mBBallMissed++;
                    mScore += KBBallMissedScore;
                }
            }
            #endregion Step b.

            #region Step c.
            for(int b=mBBallList.Count-1; b>=0; b--)
            {
                if (mHero.PrimitivesTouches(mBBallList[b]))
                {
                    mBBallHit++;
                    mScore += kBBallTouchScore;
                }
            }
            #endregion Step c.

            #region Step d.
            //Check for new basketball condition
            TimeSpan timePassed = gameTime.TotalGameTime;
            timePassed = timePassed.Subtract(mCreationTimeSpan);
            if(timePassed.TotalMilliseconds > kBallMSecInterval)
            {
                mCreationTimeSpan = gameTime.TotalGameTime;
                BasketBall b = new BasketBall();
                mTotalBBallCreated++;
                mBBallList.Add(b);
            }
            #endregion Step d.

            #region Step e.
            //Check for winning condition
            if (mScore > kWinScore)
                mFinal = new TexturedPrimitive.TexturedPrimitive("Winner", new Vector2(75, 50), new Vector2(30, 20));
            else if (mScore < kLossScore)
                mFinal = new TexturedPrimitive.TexturedPrimitive("Loser", new Vector2(75, 50), new Vector2(30, 20));
            #endregion Step e.

            #region Select wich primitive to work on
            if (InputWrapper.InputWrapper.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                mWorkPrim = mUWBLogo;
            #endregion

            #region Update the work primitive
            float rotation = 0;
            if (InputWrapper.InputWrapper.Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                rotation = MathHelper.ToRadians(1f); //1 degree pre-press
            else if (InputWrapper.InputWrapper.Buttons.Y == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                rotation = MathHelper.ToRadians(-1f); //1 degree pre-press
            mWorkPrim.Update(InputWrapper.InputWrapper.ThumbSticks.Left, InputWrapper.InputWrapper.ThumbSticks.Right, rotation);
            #endregion

            #region Step 3a. Change current selected vector
            if (InputWrapper.InputWrapper.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                mCurrentLocator = mPa;
            else if (InputWrapper.InputWrapper.Buttons.B == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                mCurrentLocator = mPb;
            else if (InputWrapper.InputWrapper.Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                mCurrentLocator = mPx;
            else if (InputWrapper.InputWrapper.Buttons.Y == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                mCurrentLocator = mPy;
            #endregion

            #region Step 3b. Move Vector
            //Change the current locator position
            mCurrentLocator.mPosition += InputWrapper.InputWrapper.ThumbSticks.Right;
            #endregion

            #region Step 3c. Rotate Vector
            //Left thumbstick- X rotates the vector at Py
            float rotateYByRadian = MathHelper.ToRadians(InputWrapper.InputWrapper.ThumbSticks.Left.X);
            #endregion

            #region Stepp 3d. Increase/Decrease the lenght of vector
            //Left thumbstick- Y increase/decrease the lenght of vector
            float vecYLen = mVectorAyPv.Length();
            vecYLen += InputWrapper.InputWrapper.ThumbSticks.Left.Y;
            #endregion

            #region Step 3e. Compute vector changes
            //Compute the rotated direction of vector Py
            mVectorAyPv = ShowVector.RotateVectorByAngle(mVectorAyPv, rotateYByRadian);
            mVectorAyPv.Normalize(); //Normalize vectorAtPy to size of 1f
            mVectorAyPv *= vecYLen;  //Scale the vector to the new size
            #endregion

            #region Step 3a. Control and fly the rocket
            mRocket.RotateAngleInRadian += MathHelper.ToRadians(InputWrapper.InputWrapper.ThumbSticks.Right.X);
            gRocket.Speed += InputWrapper.InputWrapper.ThumbSticks.Left.Y * 0.1f;
            gRocket.VelocityDirection = gRocket.FrontDirection;

            if(Camera.CollidedWithCameraWindow(mRocket)!= Camera.CameraWindowCollisionStatus.InsideWindow)
            {
                gRocket.Speed = 0f;
                mRocket.mPosition = kIntRocketPosition;
            }
            gRocket.Update();
            #endregion

            #region Step 3b. Set the arrow to point toward the rocket
            Vector2 toRocket = mRocket.mPosition - mArrow.mPosition;
            mArrow.FrontDirection = toRocket;
            #endregion

            #region 3. Check/launch the chaser!
            if (mChaser.HasValidTarget)
            {
                if (mChaser.HitTarget)
                {
                    mChaserHit++;
                    mChaser.Target = null;
                }

                if (Camera.CollidedWithCameraWindow(mChaser) != Camera.CameraWindowCollisionStatus.InsideWindow)
                {
                    mChaserMissed++;
                    mChaser.Target = null;
                }
            }

            if (InputWrapper.InputWrapper.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                mChaserMissed++;
                mChaser.Target = null;
            }
            #endregion

            mRocket.RotateAngleInRadian += MathHelper.ToRadians(InputWrapper.InputWrapper.ThumbSticks.Right.X);
            mRocket.mPosition += InputWrapper.InputWrapper.ThumbSticks.Left;

            //Set net to flight
            if (InputWrapper.InputWrapper.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                mNetInFlight = true;
                mNet.RotateAngleInRadian = mRocket.RotateAngleInRadian;
                mNet.mPosition = mRocket.mPosition;
                mNetVelocity = ShowVector.RotateVectorByAngle(mRocketInDirection, mNet.RotateAngleInRadian) * mNetSpeed;
            }

            if (!mInsectPreset)
            {
                float x = 15f + ((float)Game1.sRan.NextDouble() * 30f);
                float y = 15f + ((float)Game1.sRan.NextDouble() * 30f);
                mInsectPreset = true;
            }

            if (mNetInFlight)
            {
                mInsectPreset = false;
                mNetInFlight = false;
                mNumInsectShot++;
            }

            if ((Camera.CollidedWithCameraWindow(mNet) != Camera.CameraWindowCollisionStatus.InsideWindow))
            {
                mNetInFlight = false;
                mNumMissed++;
            }


        }

        public void DrawGame()
        {
            mHero.Draw();
            mBall.Draw();
            mUWBLogo.Draw();
            mRocket.Draw();

            if (mNetInFlight)
                mNet.Draw();

            if (mInsectPreset)
                mInsect.Draw();
            if (mChaser.HasValidTarget)
                mChaser.Draw();

            //Print out text message to echo status
            FontSupport.PrintStatus("Chaser Hit=" + mChaserHit + " Missed=" + mChaserMissed, null);

            //Print out text message to echo status
            FontSupport.PrintStatus("Num insects netted = " + mNumInsectShot + " Num missed = " + mNumMissed, null);

            //Print out text message to echo status
            FontSupport.PrintStatus("Rocket Speed(LeftThumb-Y)=" + gRocket.Speed + " VelocityDirection(RightThumb-X):" + gRocket.VelocityDirection, null);
            FontSupport.PrintStatusAt(mRocket.mPosition, mRocket.mPosition.ToString(), Color.White);

            //Draw vectorAtPy at Py
            ShowVector.DrawPointVector(mPy.mPosition, mVectorAyPv);

            mPa.Draw();
            mPb.Draw();
            mPx.Draw();
            mPy.Draw();
            foreach (BasketBall b in mBBallList)
                b.Draw();
            if (null != mFinal)
                mFinal.Draw();
            //Drawn last to always show up on top
            FontSupport.PrintStatus("Status: "+"Score="+mScore+"Baskeball: Generated("+mTotalBBallCreated+")Collected(" + mBBallHit+")Missed("+mBBallMissed+")", null);
            FontSupport.PrintStatusAt(mBall.mPosition, mBall.RotateAngleInRadian.ToString(), Color.Red);
            FontSupport.PrintStatusAt(mUWBLogo.mPosition, mUWBLogo.mPosition.ToString(), Color.Black);
            FontSupport.PrintStatus("A-Soccer B-Logo LeftThumb:Move RightThumb:Scale X/Y:Rotate", null);

            //Print out text message to echo status
            FontSupport.PrintStatus("Locator Positions: A=" + mPa.mPosition + " B=" + mPb.mPosition, null);

            //Drawing the vectors
            Vector2 v = mPb.mPosition - mPa.mPosition; //Vector V is from Pa to Pb

            //Draw Vector-V at Pa, and Px
            ShowVector.DrawFromTo(mPa.mPosition, mPb.mPosition);
            ShowVector.DrawFromTo(mPx.mPosition, v);
        }
    }
}
