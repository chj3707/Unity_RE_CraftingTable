using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  ������ �߰� ��ư ���� �뵵 ��ũ��Ʈ
 */


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
            // ��� �������� �ƴϸ� continue
            if (!itemInfo.m_IsMaterial) continue;

            // ������ ��ư ����
            UIManager.GetInstance.GenerateObject(m_ItemBtn, itemInfo, this.transform);
        }

        // ������ Ÿ�� ��Ȱ��ȭ
        m_ItemBtn.SetActive(false);
    }
}
