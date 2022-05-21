using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem : MonoBehaviour
{
    public GameObject m_ItemBtn = null;     // ������ ��ư ������Ÿ��

    private void Start()
    {
        // �����۵� ���� ��������
        Dictionary<string, Item>.ValueCollection tempItemsInfo = ItemManager.GetInstance.m_ItemDic.Values;
        int itemTotalCount = tempItemsInfo.Count;

        // ��� ������ ���� ��ŭ ��ư ����
        foreach (var itemInfo in tempItemsInfo)
        {
            if (!itemInfo.m_IsMaterial) continue;                           // ��� �������� �ƴϸ� continue

            UIManager.GetInstance.GenerateObject(m_ItemBtn,
                                                 itemInfo.m_ItemSprite,
                                                 string.Format($"Add Item [{itemInfo.m_ItemName}]"),
                                                 this.transform);           // ������ ��ư ����
        }
        
        m_ItemBtn.SetActive(false);                                         // ������ Ÿ�� ��Ȱ��ȭ
    }
}
