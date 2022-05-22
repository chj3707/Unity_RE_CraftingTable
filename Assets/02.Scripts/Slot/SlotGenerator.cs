using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  인벤토리, 제작대 슬롯 생성용 스크립트
 */


public class SlotGenerator : MonoBehaviour
{
    public GameObject m_SlotObj = null;     // 슬롯 오브젝트
    public int m_SlotCount;                 // 만들 개수

    void Start()
    {
        UIManager.GetInstance.GenerateObject(m_SlotObj, m_SlotCount, this.transform);
    }
}
