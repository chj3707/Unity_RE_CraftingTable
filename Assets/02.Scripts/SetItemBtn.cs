using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  ������ �߰� ��ư ���� �뵵 ��ũ��Ʈ
 */


public class SetItemBtn : MonoBehaviour
{
    public GameObject item_btn_obj = null;     // ������ ��ư ������Ʈ

    private void Start()
    {
        create_add_item_buttons();
    }

    // ��� ������ �߰� ��ư ���� �Լ�
    private void create_add_item_buttons()
    {
        // ������ �����ͺ��̽� ���� ��������
        Dictionary<string, Item>.ValueCollection temp_items_info = ItemDataBase.GetInstance.item_database.Values;

        foreach (var item_info in temp_items_info)
        {
            if (false == item_info.is_material) continue;
            UIManager.GetInstance.generate_gameobject(item_btn_obj, item_info, transform);
        }

        item_btn_obj.SetActive(false);
    }
}
