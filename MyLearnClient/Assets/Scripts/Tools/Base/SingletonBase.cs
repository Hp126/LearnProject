using System;

/// <summary>
/// 不继承MonoBehaviour的单例模式基类
/// </summary>
/// <typeparam name="T">要实现单例的类本身</typeparam>
public abstract class SingletonBase<T> where T : class, new()
{
    // 静态实例
    private static T _instance;

    // 线程锁，用于保证线程安全
    private static readonly object _lock = new object();

    /// <summary>
    /// 全局唯一实例
    /// </summary>
    public static T Instance
    {
        get
        {
            // 双重检查锁定，确保线程安全且性能良好
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // 使用Activator创建实例，要求T必须有一个无参构造函数
                        _instance = Activator.CreateInstance<T>();
                        // 调用初始化方法
                        (_instance as SingletonBase<T>)?.OnSingletonInit();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 构造函数，设为私有防止外部实例化
    /// </summary>
    protected SingletonBase()
    {
        // 防止通过反射创建多个实例
        if (_instance != null)
        {
            throw new InvalidOperationException("单例类 " + typeof(T).Name + " 已经被实例化，请使用 Instance 属性访问。");
        }
    }

    /// <summary>
    /// 单例初始化时调用的方法，可以在子类中重写
    /// </summary>
    protected virtual void OnSingletonInit()
    {
        // 子类可以在这里进行初始化操作
    }

    /// <summary>
    /// 销毁单例实例（谨慎使用，通常单例在程序生命周期内不销毁）
    /// </summary>
    public static void DestroyInstance()
    {
        _instance = null;
    }
}