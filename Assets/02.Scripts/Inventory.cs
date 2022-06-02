using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    public GameObject m_SlotPrefab = null;                                 // 슬롯 프로토 타입
    public int m_SlotCount;                                                // 슬롯 복사할 개수

    private static LinkedList<Slot> m_SlotList = new LinkedList<Slot>();   // 복사한 슬롯 데이터 리스트

    void Start()
    {
        if (m_SlotPrefab.activeSelf) m_SlotPrefab.SetActive(false);
        UIManager.GetInstance.GenerateObject(m_SlotPrefab, m_SlotCount, this.transform, ref m_SlotList);
    }

    // 인벤토리에 재료 아이템 추가
    public static void AddItemToInventory(Item p_item)
    {
        // 리스트 열거자 선언
        IEnumerator<Slot> enumerator = m_SlotList.GetEnumerator();
        // 슬롯에 채울 아이템 개수
        int itemStack = p_item.m_IsStackable ? MaxItemStack.Stackable : MaxItemStack.NonStackable;

        while (enumerator.MoveNext())
        {
            Slot currSlot = enumerator.Current;              // 현재 확인할 슬롯
            Item currItem = currSlot.GetItemInfo(currSlot);  // 슬롯에 있는 아이템 정보
            /* 
             *  재료 아이템 추가 조건
             *  1.슬롯이 비어 있다.
             *  2.추가할 아이템과 같은 아이템 이고, 아이템 스택이 가득 차 있지 않다.
             */
            if (currItem == null)
            {
                for (int i = 0; i < itemStack; i++)
                    currSlot.m_ItemStack.Push(p_item); 

                itemStack = 0;
            }
            else if(currItem == p_item)
            {
                while (!currSlot.IsItemStackFull(currSlot) && itemStack > 0)
                {
                    currSlot.m_ItemStack.Push(p_item);
                    --itemStack;
                }
            }
            currSlot.UpdateUI(currSlot);                   // 슬롯 UI 업데이트
            if (itemStack == 0) break;                     // 채울 아이템 개수가 0이면 종료
        }
    }
}
