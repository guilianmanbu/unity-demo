using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TweenUnit
{
    public int index;
    public string tag;

    public float startValue;
    public float endValue;

    public float time;
    public float interval;
    public float startTime;
    public float endTime { get { return time + startTime; } }
    private float lastUpdateTime;
    private float deltaTime;

    public bool fixedTime;

    public object hashKey;

    private Action<float> dele;
    private Action endCallback;

    public void Init(int _index,float _startValue,float _endValue,float _interval, float _time,Action<float> _delegate,Action _endCallback,bool _fixedTime, string _tag)
    {
        index = _index;
        tag = _tag;
        startValue = _startValue;
        endValue = _endValue;
        time = _time;
        interval = _interval;
        startTime = _fixedTime ? Time.unscaledTime : Time.time;
        lastUpdateTime = startTime;
        deltaTime = 0;
        fixedTime = _fixedTime;
        hashKey = _delegate;

        if (_delegate == null)
            dele = null;
        else
            dele = value => { _delegate(Mathf.Lerp(startValue, endValue, value)); };

        endCallback = _endCallback;
    }

    private Vector3 startV3;
    private Vector3 endV3;
    public void Init(int _index, Vector3 _startValue, Vector3 _endValue, float _interval, float _time, Action<Vector3> _delegate, Action _endCallback, bool _fixedTime, string _tag)
    {
        index = _index;
        tag = _tag;
        startV3 = _startValue;
        endV3 = _endValue;
        time = _time;
        interval = _interval;
        startTime = _fixedTime ? Time.unscaledTime : Time.time;
        lastUpdateTime = startTime;
        deltaTime = 0;
        fixedTime = _fixedTime;
        hashKey = _delegate;

        if (_delegate == null)
            dele = null;
        else
            dele = value => { _delegate(Vector3.Lerp(startV3, endV3, value)); };

        endCallback = _endCallback;
    }

    private Vector2 startV2;
    private Vector2 endV2;
    public void Init(int _index, Vector2 _startValue, Vector2 _endValue, float _interval, float _time, Action<Vector2> _delegate, Action _endCallback, bool _fixedTime, string _tag)
    {
        index = _index;
        tag = _tag;
        startV2 = _startValue;
        endV2 = _endValue;
        time = _time;
        interval = _interval;
        startTime = _fixedTime ? Time.unscaledTime : Time.time;
        lastUpdateTime = startTime;
        deltaTime = 0;
        fixedTime = _fixedTime;
        hashKey = _delegate;

        if (_delegate == null)
            dele = null;
        else
            dele = value => { _delegate(Vector2.Lerp(startV2, endV2, value)); };

        endCallback = _endCallback;
    }

    //private object lerpEntity;
    private void Init<T,R>(int _index,T lerp,float _interval,float _time, Action<R> _delegate, Action _endCallback, bool _fixedTime, string _tag) where T:ILerpable<R>
    {
        index = _index;
        tag = _tag;
        time = _time;
        interval = _interval;
        startTime = _fixedTime ? Time.unscaledTime : Time.time;
        lastUpdateTime = startTime;
        deltaTime = 0;
        fixedTime = _fixedTime;
        hashKey = _delegate;

        ILerpable<R> lerpEntity = lerp as ILerpable<R>;
        if (_delegate == null)
            dele = null;
        else
            dele = value => { _delegate(lerpEntity.Lerp(value)); };

        endCallback = _endCallback;
        
    }



    public bool Update(float currentTime)
    {
        if (IsEnd(currentTime))
        {
            return true;
        }

        if (dele != null)
        {
            deltaTime += currentTime - lastUpdateTime;
            lastUpdateTime = currentTime;

            if (deltaTime >= interval)
            {
                deltaTime -= interval;
                float value = (currentTime - startTime) / time;
                dele(value);
            }
        }
        return false;
    }

    public bool IsEnd(float currentTime)
    {
        return currentTime >= endTime;
    }

    public void DoEnd()
    {
        if (dele != null) dele(1);
        if (endCallback != null) endCallback();
    }
}

