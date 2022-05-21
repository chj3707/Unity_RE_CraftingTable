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
    public void GenerateObject(GameObject p_object, Sprite p_sprite, string p_name, Transform p_parent)
    {
        GameObject copyObj = GameObject.Instantiate(p_object);        // ����
        copyObj.SetActive(true);                                      // Ȱ��ȭ
        copyObj.GetComponent<Image>().sprite = p_sprite;              // ��������Ʈ ����
        copyObj.name = string.Format($"{p_name}");                    // �̸� ����
        copyObj.transform.parent = p_parent;                          // �θ� ������Ʈ ����
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
