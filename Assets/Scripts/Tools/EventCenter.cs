using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Events;

// 作为所有事件的管理器
public class EventCenter
{
    // 存储所有事件
    private Dictionary<string, IEventInfo> _eventDic = new Dictionary<string, IEventInfo>();
    private static EventCenter instance;
    public static EventCenter Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new EventCenter();
            }
            return instance;
        }
    }
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        if (_eventDic.ContainsKey(name))
        {
            (_eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            _eventDic.Add(name, new EventInfo(action));
        }
    }
    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        if (_eventDic.ContainsKey(name))
        {
            if ((_eventDic[name] as EventInfo).actions != null)
            {
                (_eventDic[name] as EventInfo).actions.Invoke(); 
            }
        }
    }
    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (_eventDic.ContainsKey(name))
        {
            (_eventDic[name] as EventInfo).actions -= action;
        }
    }

    public void Clear()
    {
        _eventDic.Clear();
    }

    /// <summary>
    /// 添加带参数的事件监听
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (_eventDic.ContainsKey(name))
        {
            (_eventDic[name] as EventInfo<T>).actions += action;
        }
        else
        {
            _eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 分发带参数的事件
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger<T>(string name, T info)
    {
        if (_eventDic.ContainsKey(name))
        {
            if ((_eventDic[name] as EventInfo<T>).actions != null)
            {
                (_eventDic[name] as EventInfo<T>).actions.Invoke(info); 
            }
        }
    }

    /// <summary>
    /// 移除带参数的事件监听
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (_eventDic.ContainsKey(name))
        {
            (_eventDic[name] as EventInfo<T>).actions -= action;
        }
    }
}

// 使用接口，方便扩展有参和无参事件
public interface IEventInfo
{

}

// 响应事件
public class EventInfo: IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
// 带泛型的相应事件
public class EventInfo<T>: IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}