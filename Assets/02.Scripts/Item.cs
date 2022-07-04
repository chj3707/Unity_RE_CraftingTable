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
    public string item_name;                      // ������ �̸�
    public EItemType item_type;                  // ������ Ÿ�� (���, �Һ�)
    public Sprite item_sprite;                    // ������ ��������Ʈ
    public bool is_stackable;                     // �������� ���� �� �ִ��� üũ
    public bool is_material;                      // ��� ���������� üũ
}
