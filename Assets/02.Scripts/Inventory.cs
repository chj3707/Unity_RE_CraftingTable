using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static List<Slot> slot_list;            
    public Transform inventory_slots_parent = null;

    void Awake()
    {
        initialize();
    }

    private void initialize()
    {
        slot_list = new List<Slot>(inventory_slots_parent.GetComponentsInChildren<Slot>());
    }

    // 인벤토리에 재료 아이템 추가
    public static void insert_item_to_inventory(Item insert_item)
    {
        IEnumerator<Slot> enumerator = slot_list.GetEnumerator();
        // 슬롯에 채울 아이템 개수
        int insert_item_count = insert_item.is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;

        while (enumerator.MoveNext())
        {
            Slot current_slot = enumerator.Current;                          // 현재 확인할 슬롯
            Item current_item = current_slot.item_info.get_top_item_info();  // 현재 슬롯에 있는 아이템 정보

            /* 
             *  재료 아이템 추가 조건
             *  1.슬롯이 비어 있다.
             *  2.추가할 아이템과 같은 아이템 이고, 아이템 스택이 가득 차 있지 않다.
             */

            if (null == current_item)
            {
                for (int i = 0; i < insert_item_count; i++)
                    current_slot.item_info.item_stack.Push(insert_item);

                insert_item_count = 0;
            }
            else if(insert_item == current_item)
            {
                while (false == current_slot.item_info.is_item_stack_full() && insert_item_count > 0)
                {
                    current_slot.item_info.item_stack.Push(insert_item);
                    --insert_item_count;
                }
            }
            current_slot.item_info.update_UI();        // 슬롯 UI 업데이트
            if (0 == insert_item_count) break;         // 추가할 아이템 개수가 0개 이면 종료
        }
    }
}
