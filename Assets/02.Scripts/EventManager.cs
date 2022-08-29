using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 *  키 입력, 버튼 클릭 이벤트를 관리할 스크립트
 */

public class EventManager : Singleton_Mono<EventManager>
{
    private GameObject workbench_panel_obj = null;    
    public GameObject dragging_item_obj = null;        
    public bool is_dragging;            
    
    void Start()
    {
        initialize();
    }

    private void initialize()
    {
        workbench_panel_obj = UIManager.GetInstance.workbench_panel_obj;
        is_dragging = false;
    }

    // 아이템 추가 버튼 클릭 시 동작하는 함수
    public void _On_AddItemBtnClick()
    {
        GameObject current_click_btn = EventSystem.current.currentSelectedGameObject;     // 현재 클릭한 게임 오브젝트
        Item current_click_item = current_click_btn.GetComponent<Item_Scriptable>().item; // 클릭한 아이템 정보

        Inventory.insert_item_to_inventory(current_click_item);                           // 인벤토리에 클릭한 아이템 추가
    }

    
    void Update()
    {
        // Tab :: 제작대 열기, 닫기
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.GetInstance.set_gameobject_active(workbench_panel_obj);
        }

        // 드래그중 아이템 마우스 포인터에 위치 시키기
        if (is_dragging)
        {
            dragging_item_obj.transform.position = Input.mousePosition;
        }
    }
}
