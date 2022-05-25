using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  UI ������ ��ũ��Ʈ
 */

public class UIManager : Singleton_Mono<UIManager>
{
    public GameObject m_CraftingTablePanel = null;     // �۾��� BG

    void Start()
    {
        m_CraftingTablePanel.SetActive(false);
    }

    // ���ӿ�����Ʈ [��Ȱ��ȭ -> Ȱ��ȭ , Ȱ��ȭ -> ��Ȱ��ȭ] ó��
    public void GameObjectSetActive(GameObject p_obj)
    {
        if (p_obj.activeSelf) p_obj.SetActive(false);
        else p_obj.SetActive(true);
    }

    // ���ó :: ������ �߰� ��ư
    public void GenerateObject(GameObject p_object, Item p_itemInfo, Transform p_parent)
    {
        GameObject copyObj = GameObject.Instantiate(p_object);               // ����
        copyObj.SetActive(true);                                             // Ȱ��ȭ
        copyObj.GetComponent<ItemInfo>().m_ItemInfo = p_itemInfo;            // ������ ���� ����
        copyObj.GetComponent<Image>().sprite = p_itemInfo.m_ItemSprite;      // ��������Ʈ ����
        copyObj.name = string.Format($"Add Item [{p_itemInfo.m_ItemName}]"); // �̸� ����
        copyObj.transform.SetParent(p_parent);                               // �θ� ������Ʈ ����
    }

    // ���ó :: ����
    public void GenerateObject(GameObject p_object, int p_copyCnt, Transform p_parent, ref LinkedList<Slot> p_slotList)
    {
        for (int i = 0; i < p_copyCnt; i++)
        {
            GameObject copyObj = GameObject.Instantiate(p_object);    // ����
            copyObj.SetActive(true);                                  // Ȱ��ȭ
            copyObj.name = string.Format($"Slot_{i + 1} ");           // �̸� ����
            copyObj.transform.SetParent(p_parent);                    // �θ� ������Ʈ ����
            p_slotList.AddLast(copyObj.GetComponent<Slot>());         // ����Ʈ�� �߰�
        }
    }
}
