using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/*
 *  ������ Ŭ���� (��ũ���ͺ� ������Ʈ)
 */

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public string m_ItemName;                      // ������ �̸�
    public E_ItemType m_ItemType;                  // ������ Ÿ�� (���, �Һ�)
    public Sprite m_ItemSprite;                    // ������ ��������Ʈ
    public bool m_IsStackable;                     // �������� ���� �� �ִ��� üũ
    public bool m_IsMaterial;                      // ��� ���������� üũ
}
