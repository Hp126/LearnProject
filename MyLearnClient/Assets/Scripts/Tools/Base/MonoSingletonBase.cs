using UnityEngine;

/// <summary>
/// 继承MonoBehaviour的单例模式基类
/// </summary>
/// <typeparam name="T">要实现单例的MonoBehaviour子类</typeparam>
public abstract class MonoSingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    // 静态实例
    private static T _instance;

    // 用于在Unity编辑器中标记是否为DontDestroyOnLoad的对象
    [SerializeField]
    protected bool _isDontDestroyOnLoad = true;

    /// <summary>
    /// 全局唯一实例
    /// </summary>
    public static T Instance
    {
        get
        {
            // 如果实例不存在，尝试在场景中查找
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                // 如果场景中也没有，创建一个新的GameObject并挂载该组件
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name + " (Singleton)");
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 如果场景中已经存在一个实例，销毁自身
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("发现重复的 " + typeof(T).Name + " 实例，已自动销毁多余实例。");
            return;
        }

        // 如果是第一个实例，设置为DontDestroyOnLoad
        _instance = this as T;
        if (_isDontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        // 调用初始化方法
        OnSingletonInit();
    }

    /// <summary>
    /// 单例初始化时调用的方法，可以在子类中重写
    /// </summary>
    protected virtual void OnSingletonInit()
    {
        // 子类可以在这里进行初始化操作
    }
}