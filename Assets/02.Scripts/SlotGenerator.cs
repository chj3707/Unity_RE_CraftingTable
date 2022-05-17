using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  ���� ������
 */

public class SlotGenerator : Singleton_Mono<SlotGenerator>          // �����ϱ� ���� �̱��� ����
{
    public GameObject m_Slot = null;

    public void CreateSlot(int p_slotCnt, Transform p_parentTrans)
    {
        for (int i = 0; i < p_slotCnt; i++)
        {
            GameObject copyObj = GameObject.Instantiate(m_Slot);    // ���� ����
            copyObj.SetActive(true);                                // ���� Ȱ��ȭ
            copyObj.name = string.Format($"Slot_{i + 1} ");         // ���� �̸� ����
            copyObj.transform.parent = p_parentTrans;               // ���� �θ� ������Ʈ ����
        }
    }
}
