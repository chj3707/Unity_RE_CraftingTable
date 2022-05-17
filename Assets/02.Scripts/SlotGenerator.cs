using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  슬롯 생성기
 */

public class SlotGenerator : Singleton_Mono<SlotGenerator>          // 접근하기 쉽게 싱글톤 적용
{
    public GameObject m_Slot = null;

    public void CreateSlot(int p_slotCnt, Transform p_parentTrans)
    {
        for (int i = 0; i < p_slotCnt; i++)
        {
            GameObject copyObj = GameObject.Instantiate(m_Slot);    // 슬롯 복사
            copyObj.SetActive(true);                                // 슬롯 활성화
            copyObj.name = string.Format($"Slot_{i + 1} ");         // 슬롯 이름 설정
            copyObj.transform.parent = p_parentTrans;               // 슬롯 부모 오브젝트 설정
        }
    }
}
