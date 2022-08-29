using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  UI 관리용 스크립트
 */

public class UIManager : Singleton_Mono<UIManager>
{
    public GameObject workbench_panel_obj = null; 

    void Start()
    {
        initialize();
    }

    private void initialize()
    {
        if (true == workbench_panel_obj.activeSelf) workbench_panel_obj.SetActive(false);
    }

    // 게임오브젝트 [비활성화 -> 활성화 , 활성화 -> 비활성화] 처리
    public void set_gameobject_active(GameObject request_object)
    {
        if (true == request_object.activeSelf) request_object.SetActive(false);
        else request_object.SetActive(true);
    }

    // 사용처 :: 아이템 추가 버튼 생성
    public void generate_gameobject(GameObject request_object, Item item_info, Transform parent_transform)
    {
        GameObject copy_object = GameObject.Instantiate(request_object);        
        copy_object.SetActive(true);                                            
        copy_object.GetComponent<Item_Scriptable>().item = item_info;              
        copy_object.GetComponent<Image>().sprite = item_info.item_sprite;        
        copy_object.name = string.Format($"Add Item [{item_info.item_name}]");  
        copy_object.transform.SetParent(parent_transform);                      
    }
}
