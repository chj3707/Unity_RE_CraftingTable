using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 *  Ű �Է�, ��ư Ŭ�� �̺�Ʈ�� ������ ��ũ��Ʈ
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

    // ������ �߰� ��ư Ŭ�� �� �����ϴ� �Լ�
    public void _On_AddItemBtnClick()
    {
        GameObject current_click_btn = EventSystem.current.currentSelectedGameObject;     // ���� Ŭ���� ���� ������Ʈ
        Item current_click_item = current_click_btn.GetComponent<Item_Scriptable>().item; // Ŭ���� ������ ����

        Inventory.insert_item_to_inventory(current_click_item);                           // �κ��丮�� Ŭ���� ������ �߰�
    }

    
    void Update()
    {
        // Tab :: ���۴� ����, �ݱ�
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.GetInstance.set_gameobject_active(workbench_panel_obj);
        }

        // �巡���� ������ ���콺 �����Ϳ� ��ġ ��Ű��
        if (is_dragging)
        {
            dragging_item_obj.transform.position = Input.mousePosition;
        }
    }
}
