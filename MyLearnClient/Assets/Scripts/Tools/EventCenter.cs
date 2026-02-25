using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 事件中心核心类（支持0-5个参数、优先级执行、泛型委托）
/// 添加监听：AddListener | 移除监听：RemoveListener | 广播事件：Broadcast
/// 优先级规则：数值越小，优先级越高（越先执行）；数值越大，越靠后执行
/// </summary>
public static class EventCenter
{
    #region 泛型委托定义（支持0-5个参数）
    // 0个参数
    public delegate void EventHandler();
    // 1个参数
    public delegate void EventHandler<T1>(T1 arg1);
    // 2个参数
    public delegate void EventHandler<T1, T2>(T1 arg1, T2 arg2);
    // 3个参数
    public delegate void EventHandler<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    // 4个参数
    public delegate void EventHandler<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    // 5个参数
    public delegate void EventHandler<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    #endregion

    #region 内部数据结构
    /// <summary>
    /// 监听者信息（包含委托和优先级）
    /// </summary>
    private class Listener
    {
        /// <summary>
        /// 委托对象（存储为object，适配不同参数数量的委托）
        /// </summary>
        public object Handler { get; }

        /// <summary>
        /// 执行优先级（数值越小，优先级越高；数值越大，越靠后执行）
        /// </summary>
        public int Priority { get; }

        public Listener(object handler, int priority)
        {
            Handler = handler;
            Priority = priority;
        }
    }

    /// <summary>
    /// 事件注册表：key=事件类型(string)，value=该事件的所有监听者
    /// </summary>
    private static readonly Dictionary<string, List<Listener>> _eventRegistry = new Dictionary<string, List<Listener>>();
    #endregion

    #region 添加监听者方法（0-5个参数重载）
    /// <summary>
    /// 添加无参数事件监听者
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="handler">处理方法</param>
    /// <param name="priority">优先级（默认0，数值越小越先执行）</param>
    public static void AddListener(string eventType, EventHandler handler, int priority = 0)
    {
        AddListenerInternal(eventType, handler, priority);
    }

    /// <summary>
    /// 添加1个参数的事件监听者
    /// </summary>
    public static void AddListener<T1>(string eventType, EventHandler<T1> handler, int priority = 0)
    {
        AddListenerInternal(eventType, handler, priority);
    }

    /// <summary>
    /// 添加2个参数的事件监听者
    /// </summary>
    public static void AddListener<T1, T2>(string eventType, EventHandler<T1, T2> handler, int priority = 0)
    {
        AddListenerInternal(eventType, handler, priority);
    }

    /// <summary>
    /// 添加3个参数的事件监听者
    /// </summary>
    public static void AddListener<T1, T2, T3>(string eventType, EventHandler<T1, T2, T3> handler, int priority = 0)
    {
        AddListenerInternal(eventType, handler, priority);
    }

    /// <summary>
    /// 添加4个参数的事件监听者
    /// </summary>
    public static void AddListener<T1, T2, T3, T4>(string eventType, EventHandler<T1, T2, T3, T4> handler, int priority = 0)
    {
        AddListenerInternal(eventType, handler, priority);
    }

    /// <summary>
    /// 添加5个参数的事件监听者
    /// </summary>
    public static void AddListener<T1, T2, T3, T4, T5>(string eventType, EventHandler<T1, T2, T3, T4, T5> handler, int priority = 0)
    {
        AddListenerInternal(eventType, handler, priority);
    }
    #endregion

    #region 移除监听者方法（0-5个参数重载）
    /// <summary>
    /// 移除无参数事件监听者
    /// </summary>
    public static void RemoveListener(string eventType, EventHandler handler)
    {
        RemoveListenerInternal(eventType, handler);
    }

    /// <summary>
    /// 移除1个参数的事件监听者
    /// </summary>
    public static void RemoveListener<T1>(string eventType, EventHandler<T1> handler)
    {
        RemoveListenerInternal(eventType, handler);
    }

    /// <summary>
    /// 移除2个参数的事件监听者
    /// </summary>
    public static void RemoveListener<T1, T2>(string eventType, EventHandler<T1, T2> handler)
    {
        RemoveListenerInternal(eventType, handler);
    }

    /// <summary>
    /// 移除3个参数的事件监听者
    /// </summary>
    public static void RemoveListener<T1, T2, T3>(string eventType, EventHandler<T1, T2, T3> handler)
    {
        RemoveListenerInternal(eventType, handler);
    }

    /// <summary>
    /// 移除4个参数的事件监听者
    /// </summary>
    public static void RemoveListener<T1, T2, T3, T4>(string eventType, EventHandler<T1, T2, T3, T4> handler)
    {
        RemoveListenerInternal(eventType, handler);
    }

    /// <summary>
    /// 移除5个参数的事件监听者
    /// </summary>
    public static void RemoveListener<T1, T2, T3, T4, T5>(string eventType, EventHandler<T1, T2, T3, T4, T5> handler)
    {
        RemoveListenerInternal(eventType, handler);
    }
    #endregion

    #region 广播事件方法（0-5个参数重载）
    /// <summary>
    /// 广播无参数事件
    /// </summary>
    public static void Broadcast(string eventType)
    {
        if (!_eventRegistry.TryGetValue(eventType, out var listeners)) return;

        // 按优先级升序排序：数值越小越先执行，数值越大越靠后执行
        var sortedListeners = listeners.OrderBy(l => l.Priority).ToList();
        foreach (var listener in sortedListeners)
        {
            if (listener.Handler is EventHandler handler)
            {
                handler.Invoke();
            }
        }
    }

    /// <summary>
    /// 广播1个参数的事件
    /// </summary>
    public static void Broadcast<T1>(string eventType, T1 arg1)
    {
        if (!_eventRegistry.TryGetValue(eventType, out var listeners)) return;

        var sortedListeners = listeners.OrderBy(l => l.Priority).ToList();
        foreach (var listener in sortedListeners)
        {
            if (listener.Handler is EventHandler<T1> handler)
            {
                handler.Invoke(arg1);
            }
        }
    }

    /// <summary>
    /// 广播2个参数的事件
    /// </summary>
    public static void Broadcast<T1, T2>(string eventType, T1 arg1, T2 arg2)
    {
        if (!_eventRegistry.TryGetValue(eventType, out var listeners)) return;

        var sortedListeners = listeners.OrderBy(l => l.Priority).ToList();
        foreach (var listener in sortedListeners)
        {
            if (listener.Handler is EventHandler<T1, T2> handler)
            {
                handler.Invoke(arg1, arg2);
            }
        }
    }

    /// <summary>
    /// 广播3个参数的事件
    /// </summary>
    public static void Broadcast<T1, T2, T3>(string eventType, T1 arg1, T2 arg2, T3 arg3)
    {
        if (!_eventRegistry.TryGetValue(eventType, out var listeners)) return;

        var sortedListeners = listeners.OrderBy(l => l.Priority).ToList();
        foreach (var listener in sortedListeners)
        {
            if (listener.Handler is EventHandler<T1, T2, T3> handler)
            {
                handler.Invoke(arg1, arg2, arg3);
            }
        }
    }

    /// <summary>
    /// 广播4个参数的事件
    /// </summary>
    public static void Broadcast<T1, T2, T3, T4>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (!_eventRegistry.TryGetValue(eventType, out var listeners)) return;

        var sortedListeners = listeners.OrderBy(l => l.Priority).ToList();
        foreach (var listener in sortedListeners)
        {
            if (listener.Handler is EventHandler<T1, T2, T3, T4> handler)
            {
                handler.Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }

    /// <summary>
    /// 广播5个参数的事件
    /// </summary>
    public static void Broadcast<T1, T2, T3, T4, T5>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        if (!_eventRegistry.TryGetValue(eventType, out var listeners)) return;

        var sortedListeners = listeners.OrderBy(l => l.Priority).ToList();
        foreach (var listener in sortedListeners)
        {
            if (listener.Handler is EventHandler<T1, T2, T3, T4, T5> handler)
            {
                handler.Invoke(arg1, arg2, arg3, arg4, arg5);
            }
        }
    }
    #endregion

    #region 私有辅助方法
    /// <summary>
    /// 内部添加监听者逻辑
    /// </summary>
    private static void AddListenerInternal(string eventType, object handler, int priority)
    {
        if (string.IsNullOrEmpty(eventType)) throw new ArgumentNullException(nameof(eventType));
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        if (!_eventRegistry.ContainsKey(eventType))
        {
            _eventRegistry[eventType] = new List<Listener>();
        }

        // 避免重复添加同一个委托
        if (!_eventRegistry[eventType].Any(l => l.Handler.Equals(handler)))
        {
            _eventRegistry[eventType].Add(new Listener(handler, priority));
        }
    }

    /// <summary>
    /// 内部移除监听者逻辑
    /// </summary>
    private static void RemoveListenerInternal(string eventType, object handler)
    {
        if (string.IsNullOrEmpty(eventType)) throw new ArgumentNullException(nameof(eventType));
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        if (_eventRegistry.TryGetValue(eventType, out var listeners))
        {
            var listenerToRemove = listeners.FirstOrDefault(l => l.Handler.Equals(handler));
            if (listenerToRemove != null)
            {
                listeners.Remove(listenerToRemove);

                // 无监听者时移除注册表项，节省内存
                if (listeners.Count == 0)
                {
                    _eventRegistry.Remove(eventType);
                }
            }
        }
    }
    #endregion
}
