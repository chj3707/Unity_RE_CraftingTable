using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class MaxItemStack
{
    public static int stackable = 64;
    public static int non_stackable = 1;
}

/*
 *  슬롯 관리 스크립트 
 */
public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Stack<Item> item_stack = new Stack<Item>();  // 슬롯에 있는 아이템 정보
    protected Image item_image = null;                  // 아이템 이미지
    protected Text item_quantity_text = null;                   // 아이템 개수

    private void Awake()
    {
        initialize();
    }

    // 초기화
    protected virtual void initialize()
    {
        item_image = transform.GetChild(0).GetComponent<Image>();
        item_quantity_text = item_image.GetComponentInChildren<Text>();

        item_image.enabled = false;
        item_quantity_text.enabled = false;
    }

    // 슬롯 변경 내용 있을 때마다 호출하여 UI 업데이트
    public void update_UI(Slot request_slot)
    {
        // 슬롯이 비었을 때
        if (true == is_slot_empty(request_slot))
        {
            request_slot.item_image.sprite = null;
            request_slot.item_image.enabled = false;

            request_slot.item_quantity_text.text = request_slot.item_stack.Count.ToString();
            request_slot.item_quantity_text.enabled = false;
            return;
        }

        // 슬롯에 아이템이 있을 때
        Item current_item = request_slot.item_stack.Peek();

        request_slot.item_image.enabled = true;
        request_slot.item_image.sprite = current_item.item_sprite;
        // 쌓을 수 있는 아이템만 개수 표시
        request_slot.item_quantity_text.enabled = current_item.is_stackable ? true : false;
        request_slot.item_quantity_text.text = request_slot.item_stack.Count.ToString();
    }

    // 슬롯에 쌓인 아이템 개수 가져가는 용도
    public int get_item_stack_quantity(Slot request_slot)
    {
        return request_slot.item_stack.Count;
    }

    // 슬롯에 있는 아이템의 최대 개수 가져가는 용도
    public int get_max_item_stack_in_slot(Slot request_slot)
    {
        return request_slot.get_item_info(request_slot).is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;
    }

    // 슬롯에 있는 아이템 정보 가져가는 용도
    public Item get_item_info(Slot request_slot)
    {
        return is_slot_empty(request_slot) ? null : request_slot.item_stack.Peek();
    }

    // 슬롯에 있는 아이템 스택이 가득 찼는지 확인
    public bool is_item_stack_full(Slot request_slot)
    {
        Item temp_item = request_slot.item_stack.Peek();
        if (temp_item.is_stackable && request_slot.item_stack.Count == MaxItemStack.stackable) return true;
        else if(!temp_item.is_stackable && request_slot.item_stack.Count == MaxItemStack.non_stackable) return true;
        return false;
    }

    // 슬롯이 비어 있는지 확인
    public bool is_slot_empty(Slot request_slot)
    {
        if (0 == request_slot.item_stack.Count) return true;
        return false;
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
        switch(is_slot_empty(this))
        {
            // 빈 슬롯 클릭
            case true:
                switch(eventmanager.is_dragging)
                {
                    // 아이템 드래그중 :: 아이템 드랍
                    case true:
                        items_drop(eventdata, eventmanager);
                        break;

                    // 아이템 드래그중이 아님 :: return
                    case false:
                        return;
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
                        return;
                }
                break;
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
                pickup_item_count = item_stack.Count;
                break;

            // 우 클릭 :: 슬롯에 있는 아이템 절반 드래그
            case -2:
                pickup_item_count = Mathf.CeilToInt(item_stack.Count * 0.5f);
                break;
        }
        eventmanager.is_dragging = true;

        for (int i = 0; i < pickup_item_count; i++)
            dragging_item.item_stack.Push(item_stack.Pop());

        update_UI(dragging_item);
        update_UI(this);
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
                if (true == is_slot_empty(this))
                {
                    drop_item_count = dragging_item.item_stack.Count;
                }
                else
                {
                    int dragging_item_count = get_item_stack_quantity(dragging_item);
                    int required_item_count = get_max_item_stack_in_slot(this) - get_item_stack_quantity(this);

                    drop_item_count = dragging_item_count >= required_item_count ? required_item_count : dragging_item_count;
                }
                break;

            // 우 클릭 :: 슬롯에 아이템 1개 드랍
            case -2:
                if(true == is_slot_empty(this) ||
                   get_item_stack_quantity(this) < get_max_item_stack_in_slot(this))
                {
                    drop_item_count = 1;
                }       
                break;
        }

        for (int i = 0; i < drop_item_count; i++)
            item_stack.Push(dragging_item.item_stack.Pop());

        update_UI(dragging_item);
        update_UI(this);

        if (true == is_slot_empty(dragging_item))
            eventmanager.is_dragging = false;
    }

    // 아이템 스왑 , 드랍
    private void items_swap_drop(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();

        // 드래그 아이템과 슬롯 아이템 정보 비교
        switch(get_item_info(dragging_item) == get_item_info(this))
        {
            // 같은 아이템 :: 아이템 드랍
            case true:
                items_drop(eventdata, eventmanager);
                break;

            // 다른 아이템 :: 드래그 아이템과 슬롯 아이템 데이터 스왑
            case false:
                Core.swap<Stack<Item>>(ref dragging_item.item_stack, ref item_stack);
                update_UI(dragging_item);
                update_UI(this);
                break;
        }
    }
}
