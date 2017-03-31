using System;

public class Class1
{
	public MyGame()
	{
        // Hero stuff ...
        TexturedPrimitive mHero;
        Vector2 kHeroSize = new Vector2(15, 15);
        Vector2 kHeroPosition = Vector2.Zero;

        // Basketballs ...
        List<BasketBall> mBBallList;
        TimeSpan mCreationTimeStamp;
        int mTotalBBallCreated = 0;
        // this is 0.5 seconds
        const int kBballMSecInterval = 500;

        // Game state
        int mScore = 0;
        int mBBallMissed = 0, mBBallHit = 0;
        const int kBballTouchScore = 1;
        const int kBballMissedScore = -2;
        const int kWinScore = 10;
        const int kLossScore = -10;
        TexturedPrimitive mFinal = null;

        // Hero ...
        mHero = new TexturedPrimitive("Me", kHeroPosition, kHeroSize);
        // Basketballs
        mCreationTimeStamp = new TimeSpan(0);
        mBBallList = new List<BasketBall>();

    }
}

public void UpdateGame(GameTime gameTime)
{
    #region Step a.
    if (null != mFinal) // Done!!
        return;
    #endregion Step a.
    #region Step b.
    // Hero movement: right thumb stick
    mHero.Update(InputWrapper.ThumbSticks.Right);
    // Basketball ...
    for (int b = mBBallList.Count - 1; b >= 0; b--)
    {
        if (mBBallList[b].UpdateAndExplode())
        {
            mBBallList.RemoveAt(b);
            mBBallMissed++;
            mScore += kBballMissedScore;
        }
    }
    #endregion Step b.
    #region Step c.
    for (int b = mBBallList.Count - 1; b >= 0; b--)
    {
        if (mHero.PrimitivesTouches(mBBallList[b]))
        {
            mBBallList.RemoveAt(b);
            mBBallHit++;
            mScore += kBballTouchScore;
        }
    }
    #endregion Step c
    #region Step d.
    // Check for new basketball condition
    TimeSpan timePassed = gameTime.TotalGameTime;
    timePassed = timePassed.Subtract(mCreationTimeStamp);
    if (timePassed.TotalMilliseconds > kBballMSecInterval)
    {
        mCreationTimeStamp = gameTime.TotalGameTime;
        BasketBall b = new BasketBall();
        mTotalBBallCreated++;
        mBBallList.Add(b);
    }
    #endregion Step d.
    #region Step e.
    // Check for winning condition ...
    if (mScore > kWinScore)
    mFinal = new TexturedPrimitive("Winner", new Vector2(75, 50), new Vector2(30, 20));
    else if (mScore < kLossScore)
    mFinal = new TexturedPrimitive("Looser",
    new Vector2(75, 50), new Vector2(30, 20));
    #endregion Step e.
}

public void DrawGame()
{
    mHero.Draw();
    foreach (BasketBall b in mBBallList)
        b.Draw();
    if (null != mFinal)
        mFinal.Draw();
    // Drawn last to always show up on top
    FontSupport.PrintStatus("Status: " +
   "Score=" + mScore + " Basketball: Generated( " +
   mTotalBBallCreated + ") Collected(" + mBBallHit + ") Missed(" +
   mBBallMissed + ")",
   null);
}