using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  UI 관리용 스크립트
 */

public class UIManager : Singleton_Mono<UIManager>
{
    public GameObject m_CraftingTablePanel = null;     // 작업대 BG

    void Start()
    {
        m_CraftingTablePanel.SetActive(false);
    }

    // 게임오브젝트 [비활성화 -> 활성화 , 활성화 -> 비활성화] 처리
    public void GameObjectSetActive(GameObject p_obj)
    {
        if (p_obj.activeSelf) p_obj.SetActive(false);
        else p_obj.SetActive(true);
    }

    // 사용처 :: 아이템 추가 버튼
    public void GenerateObject(GameObject p_object, Item p_itemInfo, Transform p_parent)
    {
        GameObject copyObj = GameObject.Instantiate(p_object);               // 복사
        copyObj.SetActive(true);                                             // 활성화
        copyObj.GetComponent<ItemInfo>().m_ItemInfo = p_itemInfo;            // 아이템 정보 설정
        copyObj.GetComponent<Image>().sprite = p_itemInfo.m_ItemSprite;      // 스프라이트 설정
        copyObj.name = string.Format($"Add Item [{p_itemInfo.m_ItemName}]"); // 이름 설정
        copyObj.transform.SetParent(p_parent);                               // 부모 오브젝트 설정
    }

    // 사용처 :: 슬롯
    public void GenerateObject(GameObject p_object, int p_copyCnt, Transform p_parent, ref LinkedList<Slot> p_slotList)
    {
        for (int i = 0; i < p_copyCnt; i++)
        {
            GameObject copyObj = GameObject.Instantiate(p_object);    // 복사
            copyObj.SetActive(true);                                  // 활성화
            copyObj.name = string.Format($"Slot_{i + 1} ");           // 이름 설정
            copyObj.transform.SetParent(p_parent);                    // 부모 오브젝트 설정
            p_slotList.AddLast(copyObj.GetComponent<Slot>());         // 리스트에 추가
        }
    }
}
