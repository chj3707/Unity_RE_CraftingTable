using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 *  슬롯 관리 스크립트 
 */
public class Slot : MonoBehaviour,
                    IPointerClickHandler
{
    public ItemInfo item_info;

    public bool is_workbench_slot = false;                      

    private void Awake()
    {
        initialize();
    }

    // 초기화
    protected virtual void initialize()
    {
        item_info = new ItemInfo();
        item_info.item_image = transform.GetChild(0).GetComponent<Image>();
        item_info.item_quantity_text = item_info.item_image.GetComponentInChildren<Text>();

        item_info.item_image.enabled = false;
        item_info.item_quantity_text.enabled = false;
    }


    /*
     * 슬롯 마우스 클릭 시 호출 되는 함수
     * 
     * 1. 드래그 중이 아닐 때 빈 슬롯 클릭 :: 아무것도 하지 않음
     * 
     * 2. 드래그 중에 빈 슬롯 클릭
     * 2-1. 슬롯 좌 클릭 :: 드래그 중인 아이템 개수 만큼 슬롯에 드랍
     * 2-2. 슬롯 우 클릭 :: 드래그 중인 아이템 슬롯에 1개 드랍
     * 
     * 3. 드래그 중이 아닐 때 아이템이 있는 슬롯 클릭
     * 3-1. 슬롯 좌 클릭 :: 슬롯에 있는 모든 아이템 드래그
     * 3-2. 슬롯 우 클릭 :: 슬롯에 있는 아이템 절반(올림) 드래그
     * 
     * 4. 드래그 중에 아이템이 있는 슬롯 클릭
     * 4-1. 슬롯에 있는 아이템과 드래그 중인 아이템이 다른 경우 :: 아이템 스왑
     * 4-2. 아이템이 같은 경우 슬롯 좌 클릭 :: 슬롯의 아이템 최대 개수 만큼 드랍 나머지 드래그
     * 4-3. 아이템이 같은 경우 슬롯 우 클릭 :: 슬롯의 아이템이 최대 개수가 아니면 1개 드랍
     * 
     */
    public void OnPointerClick(PointerEventData eventdata)
    {
        EventManager eventmanager = EventManager.GetInstance;

        switch (item_info.is_item_stack_empty())
        {
            // 빈 슬롯 클릭
            case true:
                switch (eventmanager.is_dragging)
                {
                    // 아이템 드래그중이 아님 :: return
                    case false: return;

                    // 아이템 드래그중 :: 아이템 드랍
                    case true:
                        items_drop(eventdata, eventmanager);
                        if (true == is_workbench_slot) ++Workbench.workbench_material_quantity;
                        break;
                }
                break;

            // 아이템이 있는 슬롯 클릭
            case false:
                switch (eventmanager.is_dragging)
                {
                    // 드래그 중 :: 슬롯에 있는 아이템과 드래그 중인 아이템 정보 스왑, 드랍(아이템이 같은 경우)
                    case true:
                        items_swap_drop(eventdata, eventmanager);
                        break;

                    // 드래그 중이 아님 :: 슬롯 아이템 드래그
                    case false:
                        items_drag(eventdata, eventmanager);    // 아이템 드래그 :: 함수 내 에서 좌,우 클릭 분리
                        break;
                }
                break;
        }

        // 워크벤치 슬롯을 클릭 한거라면 조합 실행
        if (true == is_workbench_slot)
        {
            Workbench temp_workbench = this.GetComponentInParent<Workbench>();
            temp_workbench.compare_workbench_with_recipes();
        }
    }

    // 아이템 드래그
    private void items_drag(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();
        int pickup_item_count = 0;

        switch (eventdata.pointerId)
        {
            // 좌 클릭 :: 슬롯에 있는 모든 아이템 드래그
            case -1:
                pickup_item_count = item_info.get_item_stack_quantity();
                if (true == is_workbench_slot) --Workbench.workbench_material_quantity;
                break;

            // 우 클릭 :: 슬롯에 있는 아이템 절반 드래그
            case -2:
                pickup_item_count = Mathf.CeilToInt(item_info.get_item_stack_quantity() * 0.5f);
                if (true == is_workbench_slot && 1 == item_info.get_item_stack_quantity()) --Workbench.workbench_material_quantity;
                break;
        }
        eventmanager.is_dragging = true;

        // 슬롯 아이템 스택 Pop(), 드래그 아이템 스택 Push() 반복
        for (int i = 0; i < pickup_item_count; i++)
            dragging_item.item_info.item_stack.Push(this.item_info.item_stack.Pop());

        dragging_item.item_info.update_UI();
        this.item_info.update_UI();
    }

    // 아이템 슬롯에 드랍
    private void items_drop(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();
        int drop_item_count = 0;

        switch (eventdata.pointerId)
        {
            /*
             * 좌 클릭
             * 1. 빈 슬롯 :: 드래그 중인 아이템 모두 드랍
             * 2. 아이템 있는 슬롯 :: 슬롯의 아이템 최대 개수만큼 드랍
             */
            case -1:
                if (true == this.item_info.is_item_stack_empty())
                {
                    drop_item_count = dragging_item.item_info.get_item_stack_quantity();
                }
                else
                {
                    int dragging_item_count = dragging_item.item_info.get_item_stack_quantity();
                    int required_item_count = this.item_info.get_max_item_stack() - this.item_info.get_item_stack_quantity();

                    drop_item_count = dragging_item_count >= required_item_count ? required_item_count : dragging_item_count;
                }
                break;

            // 우 클릭 :: 슬롯에 아이템 1개 드랍
            case -2:
                if (true == this.item_info.is_item_stack_empty() ||
                    this.item_info.get_item_stack_quantity() < this.item_info.get_max_item_stack())
                {
                    drop_item_count = 1;
                }
                break;
        }

        // 드래그 아이템 스택 Pop(), 슬롯 아이템 스택 Push() 반복
        for (int i = 0; i < drop_item_count; i++)
            this.item_info.item_stack.Push(dragging_item.item_info.item_stack.Pop());

        dragging_item.item_info.update_UI();
        this.item_info.update_UI();

        if (true == dragging_item.item_info.is_item_stack_empty())
            eventmanager.is_dragging = false;
    }

    // 아이템 스왑 , 드랍
    private void items_swap_drop(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();

        // 같은 아이템 :: 아이템 드랍
        if (dragging_item.item_info.get_top_item_info() == this.item_info.get_top_item_info())
        {
            items_drop(eventdata, eventmanager); 
            return;
        }
        else // 다른 아이템 :: 아이템 데이터 스왑
        {
            Core.swap<Stack<Item>>(ref dragging_item.item_info.item_stack, ref this.item_info.item_stack);
            dragging_item.item_info.update_UI();
            this.item_info.update_UI();
        }
        
        

        //// 드래그 아이템과 슬롯 아이템 정보 비교
        //switch (dragging_item.item_info.get_top_item_info() == this.item_info.get_top_item_info())
        //{
        //    // 같은 아이템 :: 아이템 드랍
        //    case true:
        //        items_drop(eventdata, eventmanager);
        //        break;

        //    // 다른 아이템 :: 아이템 데이터 스왑
        //    case false:
        //        Core.swap<Stack<Item>>(ref dragging_item.item_info.item_stack, ref this.item_info.item_stack);
        //        dragging_item.item_info.update_UI();
        //        this.item_info.update_UI();
        //        break;
        //}
    }
}
