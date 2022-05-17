using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour�� ��� ���� Ŭ������ ���� �ϵ��� ����
public class Singleton_Mono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance = null;

    private static bool m_isClosing = false; // Destory Ȯ�ο� 
    private static object m_Lock = new object();

    public static T GetInstance
    {
        get
        {
            lock (m_Lock) // ������ ���
            {
                if (m_isClosing)
                {
                    Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                    return null;
                }

                if (m_Instance == null)
                {
                    // �ν��Ͻ� �ִ��� Ȯ��
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    if (m_Instance == null)
                    {
                        // �ν��Ͻ� ����(�� ������Ʈ)
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
