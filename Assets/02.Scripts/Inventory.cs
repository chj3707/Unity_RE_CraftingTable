using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : Slot
{
    public void start_initialize()
    {
        initialize();
    }

    protected override void initialize()
    {
        base.initialize();
    }
}

public class Inventory : MonoBehaviour
{
    public GameObject slot_prefab = null;                                       // 복사할 슬롯 프리팹
    public int slot_quantity;                                                   // 슬롯 복사할 개수

    private static List<InventorySlot> slot_list = new List<InventorySlot>();   // 복사한 슬롯 데이터 리스트

    void Awake()
    {
        initialize();
    }

    private void initialize()
    {
        if (true == slot_prefab.activeSelf) slot_prefab.SetActive(false);
        slot_prefab.AddComponent<InventorySlot>().start_initialize();
        UIManager.GetInstance.generate_gameobject(slot_prefab, slot_quantity, transform, ref slot_list);
    }

    // 인벤토리에 재료 아이템 추가
    public static void insert_item_to_inventory(Item insert_item)
    {
        // 리스트 열거자 선언
        IEnumerator<InventorySlot> enumerator = slot_list.GetEnumerator();
        // 슬롯에 채울 아이템 개수
        int insert_item_stack = insert_item.is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;

        while (enumerator.MoveNext())
        {
            InventorySlot current_slot = enumerator.Current;                        // 현재 확인할 슬롯
            Item current_item = current_slot.get_item_info(current_slot);           // 슬롯에 있는 아이템 정보

            /* 
             *  재료 아이템 추가 조건
             *  1.슬롯이 비어 있다.
             *  2.추가할 아이템과 같은 아이템 이고, 아이템 스택이 가득 차 있지 않다.
             */

            if (null == current_item)
            {
                for (int i = 0; i < insert_item_stack; i++)
                    current_slot.item_stack.Push(insert_item);

                insert_item_stack = 0;
            }
            else if(insert_item == current_item)
            {
                while (!current_slot.is_item_stack_full(current_slot) && insert_item_stack > 0)
                {
                    current_slot.item_stack.Push(insert_item);
                    --insert_item_stack;
                }
            }
            current_slot.update_UI(current_slot);                  // 슬롯 UI 업데이트
            if (0 == insert_item_stack) break;                     // 추가할 아이템 개수가 0개 이면 종료
        }
    }
}
