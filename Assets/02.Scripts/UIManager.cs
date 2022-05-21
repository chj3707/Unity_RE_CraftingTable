using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  UI ������ ��ũ��Ʈ
 */

public class UIManager : Singleton_Mono<UIManager>
{
    // ���ó :: ������ �߰� ��ư
    public void GenerateObject(GameObject p_object, Item p_itemInfo, Transform p_parent)
    {
        GameObject copyObj = GameObject.Instantiate(p_object);               // ����
        copyObj.SetActive(true);                                             // Ȱ��ȭ
        copyObj.GetComponent<ItemInfo>().m_ItemInfo = p_itemInfo;            // ������ ���� ����
        copyObj.GetComponent<Image>().sprite = p_itemInfo.m_ItemSprite;      // ��������Ʈ ����
        copyObj.name = string.Format($"Add Item [{p_itemInfo.m_ItemName}]"); // �̸� ����
        copyObj.transform.parent = p_parent;                                 // �θ� ������Ʈ ����
    }

    // ���ó :: ����
    public void GenerateObject(GameObject p_object, int p_copyCnt, string p_name, Transform p_parent)
    {
        for (int i = 0; i < p_copyCnt; i++)
        {
            GameObject copyObj = GameObject.Instantiate(p_object);    // ����
            copyObj.SetActive(true);                                  // Ȱ��ȭ
            copyObj.name = string.Format($"{p_name}_{i + 1} ");       // �̸� ����
            copyObj.transform.parent = p_parent;                      // �θ� ������Ʈ ����
        }
    }
}
