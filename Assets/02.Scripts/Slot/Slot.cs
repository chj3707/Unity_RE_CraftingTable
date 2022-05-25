using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MaxItemStack
{
    public static int Stackable = 64;
    public static int NonStackable = 1;
}

public class Slot : MonoBehaviour
{
    public Stack<Item> m_ItemStack = new Stack<Item>();  // 슬롯에 있는 아이템 정보
    public Image m_ItemImage = null;                     // 아이템 이미지
    public Text m_ItemCount = null;                      // 아이템 개수

    private void Awake()
    {
        m_ItemImage = transform.GetChild(0).GetComponent<Image>();
        m_ItemCount = m_ItemImage.GetComponentInChildren<Text>();
        
        m_ItemImage.enabled = false;
        m_ItemCount.enabled = false;
    }

    // 슬롯 변경 내용 있을 때마다 호출하여 UI 업데이트
    public void UpdateUI()
    {
        Item currItem = m_ItemStack.Peek();

        // 슬롯이 비었을 때
        if (m_ItemStack.Count == 0)
        {
            m_ItemImage.sprite = null;
            m_ItemImage.enabled = false;

            m_ItemCount.text = m_ItemStack.Count.ToString();
            m_ItemCount.enabled = false;
            return;
        }

        // 슬롯에 아이템이 있을 때
        m_ItemImage.enabled = true;
        m_ItemImage.sprite = currItem.m_ItemSprite;
        // 쌓을 수 있는 아이템만 개수 표시
        if (currItem.m_IsStackable) m_ItemCount.enabled = true;
        m_ItemCount.text = m_ItemStack.Count.ToString();
    }

    // 슬롯에 쌓인 아이템 개수 가져가는 용도
    public int GetItemStackCount()
    {
        return m_ItemStack.Count;
    }

    // 슬롯에 있는 아이템 정보 가져가는 용도
    public Item GetItemInfo()
    {
        return m_ItemStack.Count == 0 ? null : m_ItemStack.Peek();
    }

    // 슬롯에 있는 아이템 스택이 가득 찼는지 확인
    public bool IsItemStackFull()
    {
        Item tempItem = m_ItemStack.Peek();
        if (tempItem.m_IsStackable && m_ItemStack.Count == MaxItemStack.Stackable) return true;
        else if(!tempItem.m_IsStackable && m_ItemStack.Count == MaxItemStack.NonStackable) return true;
        return false;
    }
}
