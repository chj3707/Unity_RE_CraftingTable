using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  ������ ������ ���� (Dictionary)
 *  ������ ������ �� ����� ��ũ��Ʈ
 */

public class ItemManager : Singleton_Mono<ItemManager>
{
    public Dictionary<string, Item> m_ItemDic = new Dictionary<string, Item>(); // ������ ������ <������ �̸�, ������ ����>

    private void Awake()
    {
        Item[] tempItemArr = Resources.LoadAll<Item>("ScriptableObject/Items"); // ���ҽ� �������� ������ ����(��ũ���ͺ�) �����ͼ� �Ҵ�
        int itemArrSize = tempItemArr.Length;                                   // ������ �迭 ũ�� �Ҵ�

        for (int i = 0; i < itemArrSize; i++)
        {
            m_ItemDic.Add(tempItemArr[i].m_ItemName, tempItemArr[i]);           // ��ųʸ��� ������ ���� �߰�
        }
    }

}
