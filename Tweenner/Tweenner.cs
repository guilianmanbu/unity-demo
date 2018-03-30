using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tweenner : MonoBehaviour {

    private static Tweenner instance;
    public static Tweenner Instance
    {
        get
        {
            if (instance==null)
            {
                GameObject go = new GameObject("Tweenner");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<Tweenner>();
            }
            return instance;
        }
    }

    private Dictionary<int, TweenUnit> units = new Dictionary<int, TweenUnit>();
    private Dictionary<object, TweenUnit> toDic = new Dictionary<object, TweenUnit>();

    private int index = 0;
    private List<TweenUnit> endList = new List<TweenUnit>();

    public int To(float startValue,float endValue,float interval,float time,Action<float> _delegate,Action endCallback,bool fixedTime,string tag)
    {
        if (_delegate == null) return -1;

        TweenUnit unit;
        if (toDic.TryGetValue(_delegate,out unit))
        {
            unit.Init(unit.index, startValue, endValue, interval, time, _delegate, endCallback, fixedTime, tag);
        }
        else
        {
            unit = new TweenUnit();
            unit.Init(NextIndex(), startValue, endValue, interval, time, _delegate, endCallback, fixedTime, tag);

            units.Add(unit.index, unit);
            toDic.Add(_delegate, unit);
        }

        return unit.index;
    }

    public int To(Vector3 startValue, Vector3 endValue, float interval, float time, Action<Vector3> _delegate, Action endCallback, bool fixedTime, string tag)
    {
        if (_delegate == null) return -1;
        
        TweenUnit unit;
        if (toDic.TryGetValue(_delegate, out unit))
        {
            unit.Init(unit.index, startValue, endValue, interval, time, _delegate, endCallback, fixedTime, tag);
        }
        else
        {
            unit = new TweenUnit();
            unit.Init(NextIndex(), startValue, endValue, interval, time, _delegate, endCallback, fixedTime, tag);

            units.Add(unit.index, unit);
            toDic.Add(_delegate, unit);
        }

        return unit.index;
    }

    public int To(float startValue, float endValue, float interval, float time, Action<float> _delegate, Action endCallback, bool fixedTime)
    {
        return To(startValue, endValue, interval, time, _delegate, endCallback, fixedTime, string.Empty);
    }
    public int To(float startValue, float endValue, float interval, float time, Action<float> _delegate, Action endCallback, string tag)
    {
        return To(startValue, endValue, interval, time, _delegate, endCallback, false, tag);
    }
    public int To(float startValue, float endValue, float interval, float time, Action<float> _delegate, Action endCallback)
    {
        return To(startValue, endValue, interval, time, _delegate, endCallback, false, string.Empty);
    }
    public int To(Vector3 startValue, Vector3 endValue, float interval, float time, Action<Vector3> _delegate, Action endCallback)
    {
        return To(startValue, endValue, interval, time, _delegate, endCallback, false, string.Empty);
    }

    public int DelayCall(float time, Action callBack,string tag)
    {
        TweenUnit unit = new TweenUnit();
        unit.Init(NextIndex(), 0, 0, 0, time, null, callBack, false, tag);

        units.Add(unit.index, unit);

        return unit.index;
    }
    public int DelayCall(float time,Action callBack)
    {
        return DelayCall(time, callBack, string.Empty);
    }

    public int NextFrameCall(Action callBack)
    {
        return DelayCall(0, callBack);
    }

    public void Remove(int index,bool toEnd)
    {
        if (index == -1) return;

        TweenUnit unit;
        if (units.TryGetValue(index,out unit))
        {
            Remove(unit);

            if (toEnd)
            {
                unit.DoEnd();
            }
        }
    }

    public void RemoveByTag(string tag,bool toEnd)
    {
        List<TweenUnit> tempList = new List<TweenUnit>();
        var iterator = units.GetEnumerator();
        while (iterator.MoveNext())
        {
            TweenUnit unit = iterator.Current.Value;
            if (unit.tag.Equals(tag))
            {
                tempList.Add(unit);
            }
        }

        if (tempList.Count>0)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                TweenUnit unit = tempList[i];

                Remove(unit);

                if(toEnd) unit.DoEnd();
            }
        }
    }

    public void RemoveAll(bool toEnd)
    {
        if (toEnd)
        {
            Dictionary<int, TweenUnit> tempDic = units;

            units = new Dictionary<int, TweenUnit>();
            toDic = new Dictionary<object, TweenUnit>();
            
            var iterator = tempDic.GetEnumerator();
            while (iterator.MoveNext())
            {
                iterator.Current.Value.DoEnd();
            }
        }
        else
        {
            units.Clear();
            toDic.Clear();
        }
    }

    private void Remove(TweenUnit unit)
    {
        units.Remove(unit.index);
        if (unit.hashKey != null) toDic.Remove(unit.hashKey);
    }
	
	// Update is called once per frame
	void Update () {
        if (units.Count>0)
        {
            float nowTime = Time.time;
            float nowTimeFixed = Time.unscaledTime;

            var iterator = units.GetEnumerator();
            while (iterator.MoveNext())
            {
                TweenUnit unit = iterator.Current.Value;

                float tempTime = unit.fixedTime ? nowTimeFixed : nowTime;

                bool end = unit.Update(tempTime);

                if (end) endList.Add(unit);
            }

            if (endList.Count>0)
            {
                for (int i = 0; i < endList.Count; i++)
                {
                    Remove(endList[i]);

                    endList[i].DoEnd();
                }

                endList.Clear();
            }
        }
	}

    int NextIndex()
    {
        return index++;
    }
}
