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
        int ItemStack = 0;

        while (enumerator.MoveNext())
        {
            Slot currSlot = enumerator.Current;                     // ���� Ȯ���� ����
            Item currItem = currSlot.GetItemInfo();                 // ���Կ� �ִ� ������ ����
            if (ItemStack == 0)
                ItemStack = p_item.m_IsStackable ? MaxItemStack.Stackable : MaxItemStack.NonStackable;

            /* 
             *  ��� ������ �߰� ����
             *  1.������ ��� �ִ�.
             *  2.�߰��� �����۰� ���� ������ �̰�, ������ ������ ���� �� ���� �ʴ�.
             */
            if (currItem == null ||
                currItem == p_item && !currSlot.IsItemStackFull())
            {
                while(!currSlot.IsItemStackFull())
                {
                    currSlot.m_ItemStack.Push(p_item);  // ���Կ� ��� ������ �߰�
                    --ItemStack;
                    if (ItemStack == 0) break;
                }

                currSlot.UpdateUI();                    // ���� UI ������Ʈ
                if(ItemStack == 0) break;
            }
        }
    }
}
