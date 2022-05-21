using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  아이템 추가 버튼 세팅 용도 스크립트
 */


public class SetItem : MonoBehaviour
{
    public GameObject m_ItemBtn = null;     // 아이템 버튼 프로토타입

    private void Start()
    {
        // 아이템들 정보 가져오기
        Dictionary<string, Item>.ValueCollection tempItemsInfo = ItemManager.GetInstance.m_ItemDic.Values;
        int itemTotalCount = tempItemsInfo.Count;

        // 재료 아이템 개수 만큼 버튼 생성
        foreach (var itemInfo in tempItemsInfo)
        {
            // 재료 아이템이 아니면 continue
            if (!itemInfo.m_IsMaterial) continue;

            // 아이템 버튼 생성
            UIManager.GetInstance.GenerateObject(m_ItemBtn, itemInfo, this.transform);
        }

        // 프로토 타입 비활성화
        m_ItemBtn.SetActive(false);
    }
}
