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
    public Stack<Item> m_ItemStack = new Stack<Item>();  // ���Կ� �ִ� ������ ����
    public Image m_ItemImage = null;                     // ������ �̹���
    public Text m_ItemCount = null;                      // ������ ����

    private void Awake()
    {
        m_ItemImage = transform.GetChild(0).GetComponent<Image>();
        m_ItemCount = m_ItemImage.GetComponentInChildren<Text>();
        
        m_ItemImage.enabled = false;
        m_ItemCount.enabled = false;
    }

    // ���� ���� ���� ���� ������ ȣ���Ͽ� UI ������Ʈ
    public void UpdateUI()
    {
        Item currItem = m_ItemStack.Peek();

        // ������ ����� ��
        if (m_ItemStack.Count == 0)
        {
            m_ItemImage.sprite = null;
            m_ItemImage.enabled = false;

            m_ItemCount.text = m_ItemStack.Count.ToString();
            m_ItemCount.enabled = false;
            return;
        }

        // ���Կ� �������� ���� ��
        m_ItemImage.enabled = true;
        m_ItemImage.sprite = currItem.m_ItemSprite;
        // ���� �� �ִ� �����۸� ���� ǥ��
        if (currItem.m_IsStackable) m_ItemCount.enabled = true;
        m_ItemCount.text = m_ItemStack.Count.ToString();
    }

    // ���Կ� ���� ������ ���� �������� �뵵
    public int GetItemStackCount()
    {
        return m_ItemStack.Count;
    }

    // ���Կ� �ִ� ������ ���� �������� �뵵
    public Item GetItemInfo()
    {
        return m_ItemStack.Count == 0 ? null : m_ItemStack.Peek();
    }

    // ���Կ� �ִ� ������ ������ ���� á���� Ȯ��
    public bool IsItemStackFull()
    {
        Item tempItem = m_ItemStack.Peek();
        if (tempItem.m_IsStackable && m_ItemStack.Count == MaxItemStack.Stackable) return true;
        else if(!tempItem.m_IsStackable && m_ItemStack.Count == MaxItemStack.NonStackable) return true;
        return false;
    }
}
