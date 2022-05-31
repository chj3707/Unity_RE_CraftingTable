using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    public GameObject m_SlotPrefab = null;                                 // 슬롯 프로토 타입
    public int m_SlotCount;                                                // 슬롯 복사할 개수

    private static LinkedList<Slot> m_SlotList = new LinkedList<Slot>();   // 복사한 슬롯 데이터 리스트
    private static LinkedListNode<Slot> m_CurrentNode;                     // 링크드 리스트 노드 접근용 변수

    void Start()
    {
        if (m_SlotPrefab.activeSelf) m_SlotPrefab.SetActive(false);
        UIManager.GetInstance.GenerateObject(m_SlotPrefab, m_SlotCount, this.transform, ref m_SlotList);
    }

    // 인벤토리에 재료 아이템 추가
    public static void AddItemToInventory(Item p_item)
    {
        IEnumerator<Slot> enumerator = m_SlotList.GetEnumerator();  // 리스트 열거자 선언
        int ItemStack = 0;

        while (enumerator.MoveNext())
        {
            Slot currSlot = enumerator.Current;                     // 현재 확인할 슬롯
            Item currItem = currSlot.GetItemInfo();                 // 슬롯에 있는 아이템 정보
            if (ItemStack == 0)
                ItemStack = p_item.m_IsStackable ? MaxItemStack.Stackable : MaxItemStack.NonStackable;

            /* 
             *  재료 아이템 추가 조건
             *  1.슬롯이 비어 있다.
             *  2.추가할 아이템과 같은 아이템 이고, 아이템 스택이 가득 차 있지 않다.
             */
            if (currItem == null ||
                currItem == p_item && !currSlot.IsItemStackFull())
            {
                while(!currSlot.IsItemStackFull())
                {
                    currSlot.m_ItemStack.Push(p_item);  // 슬롯에 재료 아이템 추가
                    --ItemStack;
                    if (ItemStack == 0) break;
                }

                currSlot.UpdateUI();                    // 슬롯 UI 업데이트
                if(ItemStack == 0) break;
            }
        }
    }
}
