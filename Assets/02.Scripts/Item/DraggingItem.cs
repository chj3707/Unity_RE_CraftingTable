using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  �巡�� ���� ������ ������ ��ũ��Ʈ
 */

public class DraggingItem : Slot
{
    private void Start()
    {
        this.Initialize();
    }

    protected override void Initialize()
    {
        m_ItemImage = this.GetComponent<Image>();
        m_ItemCount = this.GetComponentInChildren<Text>();

        m_ItemImage.enabled = false;
        m_ItemCount.enabled = false;
    }
}
