using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  아이템 정보들 저장 (Dictionary)
 *  정보를 가져갈 때 사용할 스크립트
 */

public class ItemManager : Singleton_Mono<ItemManager>
{
    public Dictionary<string, Item> m_ItemDic = new Dictionary<string, Item>(); // 아이템 정보들 <아이템 이름, 아이템 정보>

    private void Awake()
    {
        Item[] tempItemArr = Resources.LoadAll<Item>("ScriptableObject/Items"); // 리소스 폴더에서 아이템 정보(스크립터블) 가져와서 할당
        int itemArrSize = tempItemArr.Length;                                   // 아이템 배열 크기 할당

        for (int i = 0; i < itemArrSize; i++)
        {
            m_ItemDic.Add(tempItemArr[i].m_ItemName, tempItemArr[i]);           // 딕셔너리에 아이템 정보 추가
        }
    }

}
