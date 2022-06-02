using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  드래그 중인 아이템 관리용 스크립트
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
