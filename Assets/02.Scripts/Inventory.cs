using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    public GameObject m_SlotPrefab = null;                                 // ���� ������ Ÿ��
    public int m_SlotCount;                                                // ���� ������ ����

    private static LinkedList<Slot> m_SlotList = new LinkedList<Slot>();   // ������ ���� ������ ����Ʈ
    private static LinkedListNode<Slot> m_CurrentNode;                     // ��ũ�� ����Ʈ ��� ���ٿ� ����

    void Start()
    {
        if (m_SlotPrefab.activeSelf) m_SlotPrefab.SetActive(false);
        UIManager.GetInstance.GenerateObject(m_SlotPrefab, m_SlotCount, this.transform, ref m_SlotList);
    }

    // �κ��丮�� ��� ������ �߰�
    public static void AddItemToInventory(Item p_item)
    {
        IEnumerator<Slot> enumerator = m_SlotList.GetEnumerator();  // ����Ʈ ������ ����

        while(enumerator.MoveNext())
        {
            Slot tempSlot = enumerator.Current;                     // ���� Ȯ���� ����

            /* 
             *  ��� ������ �߰� ����
             *  1.������ ��� �ִ�.
             *  2.�߰��� �����۰� ���� ������ �̰�, ������ ������ ���� �� ���� �ʴ�.
             */
            if (tempSlot.GetItemInfo() == null ||
                tempSlot.GetItemInfo() == p_item && !tempSlot.IsItemStackFull())
            {
                int maxItemStack = p_item.m_IsStackable ? MaxItemStack.Stackable : MaxItemStack.NonStackable;

                for (int i = 0; i < maxItemStack; i++)
                {
                    tempSlot.m_ItemStack.Push(p_item);  // ���Կ� ��� ������ �߰�
                }
                tempSlot.UpdateUI();                    // ���� UI ������Ʈ
                break;
            }
        }
    }
}
