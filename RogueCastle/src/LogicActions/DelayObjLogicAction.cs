﻿using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle.LogicActions;

// Hack method to store floats in "pointers".
public class DelayObjLogicAction(GameObj delayObj) : LogicAction
{
    private float _delayCounter;
    private GameObj _delayObj = delayObj;

    public override void Execute()
    {
        if (ParentLogicSet is not { IsActive: true })
        {
            return;
        }

        SequenceType = Types.Sequence.Serial;
        _delayCounter = _delayObj.X;
        base.Execute();
    }

    public override void Update(GameTime gameTime)
    {
        _delayCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        ExecuteNext();
        base.Update(gameTime);
    }

    public override void ExecuteNext()
    {
        if (_delayCounter <= 0)
        {
            base.ExecuteNext();
        }
    }

    //public override void Execute()
    //{
    //    if (ParentLogicSet != null && ParentLogicSet.IsActive == true)
    //    {
    //        SequenceType = Types.Sequence.Serial;

    //        if (m_maxDelayDuration > m_minDelayDuration)
    //            m_minDelayDuration = CDGMath.RandomFloat(m_minDelayDuration, m_maxDelayDuration);

    //        m_delayTween = Tween.To(m_blankObj, m_minDelayDuration, Linear.EaseNone, "X", "1");
    //        m_delayTween.UseTicks = m_useTicks;
    //        base.Execute();
    //    }
    //}

    //public override void ExecuteNext()
    //{
    //    if (m_delayTween != null)
    //    {
    //        if (NextLogicAction != null)
    //            m_delayTween.EndHandler(NextLogicAction, "Execute");
    //        else
    //            m_delayTween.EndHandler(ParentLogicSet, "ExecuteComplete");
    //    }
    //    else
    //        base.ExecuteNext();
    //    //base.ExecuteNext();  Must override base.ExecuteNext() entirely.
    //}

    public override void Stop()
    {
        // Okay. Big problem with delay tweens.  Because logic actions are never disposed in-level (due to garbage collection concerns) the reference to the delay tween in this logic action will exist until it is 
        // disposed, EVEN if the tween completes.  If the tween completes, it goes back into the pool, and then something else will call it, but this logic action's delay tween reference will still be pointing to it.
        // This becomes a HUGE problem if this logic action's Stop() method is called, because it will then stop the tween that this tween reference is pointing to. 
        // The solution is to comment out the code below. This means that this tween reference will always call it's endhandler, which in this case is either Execute() or ExecuteComplete().  This turns out
        // to not be a problem, because when a logic set is called to stop, its IsActive flag is set to false, and all Execute() methods in logic actions have to have their parent logic set's IsActive flag to true
        // in order to run.  Therefore, when the tween calls Execute() or ExecuteComplete() nothing will happen.
        // - This bug kept you confused for almost 5 hours.  DO NOT FORGET IT. That is what this long explanation is for.
        // TL;DR - DO NOT UNCOMMENT THE CODE BELOW OR LOGIC SETS BREAK.

        //if (m_delayTween != null)
        //    m_delayTween.StopTween(false);

        base.Stop();
    }

    public override object Clone()
    {
        return new DelayObjLogicAction(_delayObj);
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _delayObj = null;
        base.Dispose();
    }

    //public override void Dispose()
    //{
    //    if (IsDisposed == false)
    //    {
    //        // See above for explanation.
    //        //if (m_delayTween != null)
    //        //    m_delayTween.StopTween(false);

    //        m_delayTween = null;
    //        m_blankObj.Dispose();
    //        m_blankObj = null;
    //        base.Dispose();
    //    }
    //}
}
