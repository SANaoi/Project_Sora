using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; } // 可以用return的Instance的属性来返回上面的私有变量instance
    }

    //protected函数：只允许继承类可以访问的方法， virtual函数：可在子类继承并且重写的Awake函数
    protected virtual void Awake() 
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    public static bool IsInitialized // 检测当前单例是否生成过了
    {
        get { return instance != null; }
    }

    // 运行Destroy函数时生命周期会运行OnDestroy函数
    protected virtual void OnDestroy()
    {
        if (instance == this) // 当前实例被销毁的话
        {
            instance = null; // 清空当前的static instance
        }
    }
}
