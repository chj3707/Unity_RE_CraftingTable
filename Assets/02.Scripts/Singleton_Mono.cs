using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour를 상속 받은 클래스만 접근 하도록 제약
public class Singleton_Mono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance = null;

    private static bool m_isClosing = false; // Destory 확인용 
    private static object m_Lock = new object();

    public static T GetInstance
    {
        get
        {
            lock (m_Lock) // 쓰레드 잠금
            {
                if (m_isClosing)
                {
                    Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                    return null;
                }

                if (m_Instance == null)
                {
                    // 인스턴스 있는지 확인
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    if (m_Instance == null)
                    {
                        // 인스턴스 생성(빈 오브젝트)
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString();

                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return m_Instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        m_isClosing = true;
    }

    private void OnDestroy()
    {
        m_isClosing = true;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
