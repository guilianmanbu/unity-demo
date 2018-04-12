using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestTweenner : MonoBehaviour {

    public Transform trans;
    public float delta;

	// Use this for initialization
	void Start () {
        print("start");

        //Tweenner.Instance.DelayCall(2, () => { print("2秒到"); });
        //Tweenner.Instance.DelayCall(2, () => { print("2秒到"); });
        //Tweenner.Instance.DelayCall(2, () => { print("2秒到"); });

        //a=Tweenner.Instance.To(0,1,0.1f,2,)
        //doing = true;
        //Tweenner.Instance.NextFrameCall(() => { print("下一帧"); doing = false; });

        //Func<Action> ac = new Func<Action>(closure);
        ls = new List<Action>();
        for (int i = 0; i < 3; i++)
        {
            ls.Add(() => {  print(i); });

            Func<int ,Action> a = delegate (int j)
            {
                return () => { print(i); };
            };

            //print("闭包："+a(4).GetHashCode()+"  外层:"+a.GetHashCode());
            //Func<Action> ac = new Func<Action>(closure);
            print("  外层:" + a(1).GetHashCode());
            //ls.Add(closure());
        }
        for (int j = 0; j < ls.Count; j++)
        {
            print("闭包：" + " :" +ls[j].GetHashCode());
            ls[j]();
        }
    }
    List<Action> ls;

    //int cas = 0;
    //Action closure()
    //{
    //    int i = 0;
    //    Action<int> action=delegate (int p)  // 是
    //    {
    //        //print(cas);
    //        print(i);
    //        //print(p);
    //    };
    //    Action<int> action2 = delegate (int p)  // 否
    //    {
    //        //print(cas);
    //        //print(i);
    //        print(p);
    //    };
    //    Action<int> action3 = delegate (int p)  // 是
    //    {
    //        print(cas);
    //        //print(i);
    //        //print(p);
    //    };
    //    Action action4 = delegate ()  // 是
    //    {
    //        print(cas);
    //        //print(i);
    //        //print(p);
    //        cas++;
    //    };
    //    //print("action1:"+action.GetHashCode());
    //    //print("action2:" + action2.GetHashCode());
    //    //print("action3:" + action3.GetHashCode());
    //    //print("action4:" + action4.GetHashCode());
    //    return action4;
    //}

    // Update is called once per frame
    void Update () {
        //if(doing) print("updat->"+Time.deltaTime);
        
        
    }

    public void OnGUI()
    {
        if (GUI.Button(new Rect(100,100,100,50),"开始"))
        {
            for (int i = 0; i < ls.Count; i++)
            {
                print("闭包：" + " :" + ls[i].GetHashCode());
                ls[i]();
            }
        }
    }

    //int a;
    //public bool doing = false;
    //void Begin()
    //{
    //    a=Tweenner.Instance.To(Vector3.zero, new Vector3(6,0,0), delta, 1,
    //        f =>
    //        {
    //            trans.position = f;
    //        },
    //        () =>
    //        {
    //            print("逐" + delta + "秒结束");
    //            //doing = false;
    //        }
    //    );
    //    print(a);
    //    //print(Tweenner.Instance.DelayCall(1, () =>
    //    //{
    //    //    Tweenner.Instance.Remove(a, false);
    //    //}));
    //    //a = Tweenner.Instance.DelayCall(3, () => { print("3秒到"); },"3");
    //    //Tweenner.Instance.DelayCall(2, () =>
    //    //{
    //    //    print("2秒到");
    //    //    Tweenner.Instance.RemoveByTag("3", true);
    //    //});

    //    //Tweenner.Instance.NextFrameCall(() => { print("下一帧"); });

    //}



    //    void action() { }

    //    Action lastdele = null;
    //    void PrintDele(Action dele)
    //    {

    //        print(dele.GetHashCode());


    //        print(dele.Equals(lastdele));

    //        lastdele = dele;
    //    }

    //    List<object> list = new List<object>();
    //    Dictionary<object, object> dic = new Dictionary<object, object>();
}
//class aa
//{
//    public void a() { }
//    public void b() { }
//}
