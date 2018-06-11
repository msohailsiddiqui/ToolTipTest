using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EasingType
{
    SmootherLerp = 0
}

public class Easing
{
    #region singleton class
    private static Easing instance = null;
    private Easing() { }

    public static Easing Instance
    {
        get
        {
            if (instance == null)
                instance = new Easing();

            return instance;
        }
    }

    #endregion
    public static float SmootherStepF(float startVal, float endVal, float t)
    {
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        return Mathf.Lerp(startVal, endVal, t);

    }
}


//************************************************************************************
//========
// TEST CASES
// 1. Check multiple animations on on object
// 2. Check multiple animations on different object running side by side
// 3. Check trying to stop invalid animation ID
// 4. Check trying to stop an animation that has already finished
//************************************************************************************

public class AnimationHelper : MonoBehaviour
{
    private float animationInterval;

    private Dictionary<float, Coroutine> coroutineDict;
    
    public float AnimationInterval
    {
        get
        {
            return animationInterval;
        }

        set
        {
            animationInterval = value;
        }
    }

    private void Start()
    {
        coroutineDict = new Dictionary<float, Coroutine>();
    }

    public float AnimateScale(Transform transformToAnimate, float startVal, float endVal, float duration, EasingType easingToApply, Action onAnimationComplete)
    {
        //Generate a Random ID
        UnityEngine.Random.InitState((int)Time.realtimeSinceStartup);
        float randomID = UnityEngine.Random.Range(0, 1000);
        randomID += Time.realtimeSinceStartup;
        
        Coroutine anim = StartCoroutine(AnimateScaleWithSmoothLerp(transformToAnimate, startVal, endVal,Time.time, duration, onAnimationComplete, randomID));
        coroutineDict.Add(randomID, anim);
        // This ID must be used to stop the animation
        // The object that called the animation should keep a reference to it
        return randomID;
    }

    public void StopAnimation(float animID)
    {
        if(coroutineDict.ContainsKey(animID))
        {
            StopCoroutine(coroutineDict[animID]);
            //Debug.Log("<color=green>AnimationHelper::StopAnimation: Stopping: " + animID + "</color>"); 
        }
        else
        {
            Debug.Log("<color=orange>AnimationHelper::StopAnimation: Trying to stop an invalid anim ID or the animation has already completed</color>");
        }
    }

    private IEnumerator AnimateScaleWithSmoothLerp(Transform transformToAnimate, float startVal, float endVal, float startTime, float duration, Action _onAnimationComplete, float _animID)
    {
        // HACK: Dirty code, this is added to handle the case where we were passed params that cannot be executed even once
        // This happens when the mouse quickly passes over a UI element and the tip tries to disappear even before it appeared
        // In such a scenario we try to remove the animation from the dict even before it has been added
        // This will happen alot of times and is the normal case
        // Need to do this without constantly checking
        bool executedAtleastOnce = false;
        while (Time.time < startTime + duration)
        {
            executedAtleastOnce = true;
            //Debug.Log("AnimationHelper::AnimateScaleWithSmoothLerp: time: " + Time.time + "Finish Time: " + (startTime + duration));
            float t = (Time.time - startTime) / duration;
            float scale = Easing.SmootherStepF(startVal, endVal, t);
            transformToAnimate.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForSeconds(animationInterval);
        }
        _onAnimationComplete();
        if (executedAtleastOnce)
        {
            if (coroutineDict.ContainsKey(_animID))
            {
                coroutineDict.Remove(_animID);
            }
            else
            {
                Debug.LogError("AnimationHelper::AnimateScaleWithSmoothLerp: coroutineDict not have the specified animID");
            }
        }
        //Debug.Log("AnimationHelper::AnimateScaleWithSmoothLerp:Animation Complete!");
    }
}
