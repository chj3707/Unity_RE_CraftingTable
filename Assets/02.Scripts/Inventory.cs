using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    public GameObject m_SlotPrefab = null;                                 // ���� ������ Ÿ��
    public int m_SlotCount;                                                // ���� ������ ����

    private static LinkedList<Slot> m_SlotList = new LinkedList<Slot>();   // ������ ���� ������ ����Ʈ

    void Start()
    {
        if (m_SlotPrefab.activeSelf) m_SlotPrefab.SetActive(false);
        UIManager.GetInstance.GenerateObject(m_SlotPrefab, m_SlotCount, this.transform, ref m_SlotList);
    }

    // �κ��丮�� ��� ������ �߰�
    public static void AddItemToInventory(Item p_item)
    {
        // ����Ʈ ������ ����
        IEnumerator<Slot> enumerator = m_SlotList.GetEnumerator();
        // ���Կ� ä�� ������ ����
        int itemStack = p_item.m_IsStackable ? MaxItemStack.Stackable : MaxItemStack.NonStackable;

        while (enumerator.MoveNext())
        {
            Slot currSlot = enumerator.Current;              // ���� Ȯ���� ����
            Item currItem = currSlot.GetItemInfo(currSlot);  // ���Կ� �ִ� ������ ����
            /* 
             *  ��� ������ �߰� ����
             *  1.������ ��� �ִ�.
             *  2.�߰��� �����۰� ���� ������ �̰�, ������ ������ ���� �� ���� �ʴ�.
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
            currSlot.UpdateUI(currSlot);                   // ���� UI ������Ʈ
            if (itemStack == 0) break;                     // ä�� ������ ������ 0�̸� ����
        }
    }
}
