using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject slot_prefab = null;                                       // ������ ���� ������
    public int slot_quantity = 27;                                              // ���� ������ ����

    private static List<Slot> slot_list = new List<Slot>();   // ������ ���� ������ ����Ʈ

    void Awake()
    {
        initialize();
    }

    private void initialize()
    {
        if (true == slot_prefab.activeSelf) slot_prefab.SetActive(false);
        UIManager.GetInstance.generate_gameobject(slot_prefab, slot_quantity, transform, ref slot_list);
    }

    // �κ��丮�� ��� ������ �߰�
    public static void insert_item_to_inventory(Item insert_item)
    {
        // ����Ʈ ������ ����
        IEnumerator<Slot> enumerator = slot_list.GetEnumerator();
        // ���Կ� ä�� ������ ����
        int insert_item_stack = insert_item.is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;

        while (enumerator.MoveNext())
        {
            Slot current_slot = enumerator.Current;            // ���� Ȯ���� ����
            Item current_item = current_slot.get_item_info();  // ���Կ� �ִ� ������ ����

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
                while (!current_slot.is_item_stack_full() && insert_item_stack > 0)
                {
                    current_slot.item_stack.Push(insert_item);
                    --insert_item_stack;
                }
            }
            current_slot.update_UI();                  // ���� UI ������Ʈ
            if (0 == insert_item_stack) break;         // �߰��� ������ ������ 0�� �̸� ����
        }
    }
}
