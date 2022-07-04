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
    public GameObject slot_prefab = null;                                       // ������ ���� ������
    public int slot_quantity;                                                   // ���� ������ ����

    private static List<InventorySlot> slot_list = new List<InventorySlot>();   // ������ ���� ������ ����Ʈ

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

    // �κ��丮�� ��� ������ �߰�
    public static void insert_item_to_inventory(Item insert_item)
    {
        // ����Ʈ ������ ����
        IEnumerator<InventorySlot> enumerator = slot_list.GetEnumerator();
        // ���Կ� ä�� ������ ����
        int insert_item_stack = insert_item.is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;

        while (enumerator.MoveNext())
        {
            InventorySlot current_slot = enumerator.Current;                        // ���� Ȯ���� ����
            Item current_item = current_slot.get_item_info(current_slot);           // ���Կ� �ִ� ������ ����

            /* 
             *  ��� ������ �߰� ����
             *  1.������ ��� �ִ�.
             *  2.�߰��� �����۰� ���� ������ �̰�, ������ ������ ���� �� ���� �ʴ�.
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
            current_slot.update_UI(current_slot);                  // ���� UI ������Ʈ
            if (0 == insert_item_stack) break;                     // �߰��� ������ ������ 0�� �̸� ����
        }
    }
}
