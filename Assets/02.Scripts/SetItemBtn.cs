using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  아이템 추가 버튼 세팅 용도 스크립트
 */


public class SetItemBtn : MonoBehaviour
{
    public GameObject item_btn_obj = null;     // 아이템 버튼 오브젝트

    private void Start()
    {
        create_add_item_buttons();
    }

    // 재료 아이템 추가 버튼 생성 함수
    private void create_add_item_buttons()
    {
        // 아이템 데이터베이스 정보 가져오기
        Dictionary<string, Item>.ValueCollection temp_items_info = ItemDataBase.GetInstance.item_database.Values;

        foreach (var item_info in temp_items_info)
        {
            if (false == item_info.is_material) continue;
            UIManager.GetInstance.generate_gameobject(item_btn_obj, item_info, transform);
        }

        item_btn_obj.SetActive(false);
    }
}
