using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  �κ��丮, ���۴� ���� ������ ��ũ��Ʈ
 */


public class SlotGenerator : MonoBehaviour
{
    public GameObject m_SlotObj = null;     // ���� ������Ʈ
    public int m_SlotCount;                 // ���� ����

    void Start()
    {
        UIManager.GetInstance.GenerateObject(m_SlotObj, m_SlotCount, this.transform);
    }
}
