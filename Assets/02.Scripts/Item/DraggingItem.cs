using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  �巡�� ���� ������ ������ ��ũ��Ʈ
 */


public class DraggingItem : MonoBehaviour
{
    public Stack<Item> m_ItemStack = new Stack<Item>();
    public Image m_ItemImage;
    public Text m_ItemCount;

    private void Start()
    {
        m_ItemImage = this.GetComponent<Image>();
        m_ItemCount = this.GetComponentInChildren<Text>();

        m_ItemImage.enabled = false;
        m_ItemCount.enabled = false;
        this.gameObject.SetActive(false);
    }

    // �巡�� ������ UI ������Ʈ
    public void UpdateUI()
    {
        if(m_ItemStack.Count == 0)
        {
            m_ItemImage.sprite = null;
            m_ItemImage.enabled = false;

            m_ItemCount.text = m_ItemStack.Count.ToString();
            m_ItemCount.enabled = false;
            this.gameObject.SetActive(false);
            return;
        }

        Item tempItem = m_ItemStack.Peek();
        this.gameObject.SetActive(true);
        m_ItemImage.enabled = true;
        m_ItemImage.sprite = tempItem.m_ItemSprite;

        if (tempItem.m_IsStackable) m_ItemCount.enabled = true;
        m_ItemCount.text = m_ItemStack.Count.ToString();
    }
}
