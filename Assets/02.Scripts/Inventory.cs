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

    // �κ��丮�� ��� ������ �߰�
    public static void insert_item_to_inventory(Item insert_item)
    {
        IEnumerator<Slot> enumerator = slot_list.GetEnumerator();
        // ���Կ� ä�� ������ ����
        int insert_item_count = insert_item.is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;

        while (enumerator.MoveNext())
        {
            Slot current_slot = enumerator.Current;                          // ���� Ȯ���� ����
            Item current_item = current_slot.item_info.get_top_item_info();  // ���� ���Կ� �ִ� ������ ����

            /* 
             *  ��� ������ �߰� ����
             *  1.������ ��� �ִ�.
             *  2.�߰��� �����۰� ���� ������ �̰�, ������ ������ ���� �� ���� �ʴ�.
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
            current_slot.item_info.update_UI();        // ���� UI ������Ʈ
            if (0 == insert_item_count) break;         // �߰��� ������ ������ 0�� �̸� ����
        }
    }
}
