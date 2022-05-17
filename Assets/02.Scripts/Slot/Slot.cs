using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item m_Item = null;            // 슬롯에 있는 아이템 정보
    public Image m_ItemImage = null;      // 아이템 이미지
    public Text m_ItemCount = null;       // 아이템 개수

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
