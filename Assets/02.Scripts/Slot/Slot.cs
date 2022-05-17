using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item m_Item = null;            // ���Կ� �ִ� ������ ����
    public Image m_ItemImage = null;      // ������ �̹���
    public Text m_ItemCount = null;       // ������ ����

    private void Awake()
    {
        m_ItemImage = transform.GetChild(0).GetComponent<Image>();
        m_ItemCount = m_ItemImage.GetComponentInChildren<Text>();

        m_ItemImage.enabled = false;
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
