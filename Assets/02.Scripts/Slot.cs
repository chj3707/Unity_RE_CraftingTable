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
 *  ���� ���� ��ũ��Ʈ 
 */
public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Stack<Item> item_stack = new Stack<Item>();  // ���Կ� �ִ� ������ ����
    protected Image item_image = null;                  // ������ �̹���
    protected Text item_quantity_text = null;                   // ������ ����

    private void Awake()
    {
        initialize();
    }

    // �ʱ�ȭ
    protected virtual void initialize()
    {
        item_image = transform.GetChild(0).GetComponent<Image>();
        item_quantity_text = item_image.GetComponentInChildren<Text>();

        item_image.enabled = false;
        item_quantity_text.enabled = false;
    }

    // ���� ���� ���� ���� ������ ȣ���Ͽ� UI ������Ʈ
    public void update_UI(Slot request_slot)
    {
        // ������ ����� ��
        if (true == is_slot_empty(request_slot))
        {
            request_slot.item_image.sprite = null;
            request_slot.item_image.enabled = false;

            request_slot.item_quantity_text.text = request_slot.item_stack.Count.ToString();
            request_slot.item_quantity_text.enabled = false;
            return;
        }

        // ���Կ� �������� ���� ��
        Item current_item = request_slot.item_stack.Peek();

        request_slot.item_image.enabled = true;
        request_slot.item_image.sprite = current_item.item_sprite;
        // ���� �� �ִ� �����۸� ���� ǥ��
        request_slot.item_quantity_text.enabled = current_item.is_stackable ? true : false;
        request_slot.item_quantity_text.text = request_slot.item_stack.Count.ToString();
    }

    // ���Կ� ���� ������ ���� �������� �뵵
    public int get_item_stack_quantity(Slot request_slot)
    {
        return request_slot.item_stack.Count;
    }

    // ���Կ� �ִ� �������� �ִ� ���� �������� �뵵
    public int get_max_item_stack_in_slot(Slot request_slot)
    {
        return request_slot.get_item_info(request_slot).is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;
    }

    // ���Կ� �ִ� ������ ���� �������� �뵵
    public Item get_item_info(Slot request_slot)
    {
        return is_slot_empty(request_slot) ? null : request_slot.item_stack.Peek();
    }

    // ���Կ� �ִ� ������ ������ ���� á���� Ȯ��
    public bool is_item_stack_full(Slot request_slot)
    {
        Item temp_item = request_slot.item_stack.Peek();
        if (temp_item.is_stackable && request_slot.item_stack.Count == MaxItemStack.stackable) return true;
        else if(!temp_item.is_stackable && request_slot.item_stack.Count == MaxItemStack.non_stackable) return true;
        return false;
    }

    // ������ ��� �ִ��� Ȯ��
    public bool is_slot_empty(Slot request_slot)
    {
        if (0 == request_slot.item_stack.Count) return true;
        return false;
    }

    /*
     * ���� ���콺 Ŭ�� �� ȣ�� �Ǵ� �Լ�
     * 
     * 1. �巡�� ���� �ƴ� �� �� ���� Ŭ�� :: �ƹ��͵� ���� ����
     * 
     * 2. �巡�� �߿� �� ���� Ŭ��
     * 2-1. ���� �� Ŭ�� :: �巡�� ���� ������ ���� ��ŭ ���Կ� ���
     * 2-2. ���� �� Ŭ�� :: �巡�� ���� ������ ���Կ� 1�� ���
     * 
     * 3. �巡�� ���� �ƴ� �� �������� �ִ� ���� Ŭ��
     * 3-1. ���� �� Ŭ�� :: ���Կ� �ִ� ��� ������ �巡��
     * 3-2. ���� �� Ŭ�� :: ���Կ� �ִ� ������ ����(�ø�) �巡��
     * 
     * 4. �巡�� �߿� �������� �ִ� ���� Ŭ��
     * 4-1. ���Կ� �ִ� �����۰� �巡�� ���� �������� �ٸ� ��� :: ������ ����
     * 4-2. �������� ���� ��� ���� �� Ŭ�� :: ������ ������ �ִ� ���� ��ŭ ��� ������ �巡��
     * 4-3. �������� ���� ��� ���� �� Ŭ�� :: ������ �������� �ִ� ������ �ƴϸ� 1�� ���
     * 
     */
    public void OnPointerClick(PointerEventData eventdata)
    {
        EventManager eventmanager = EventManager.GetInstance;
        switch(is_slot_empty(this))
        {
            // �� ���� Ŭ��
            case true:
                switch(eventmanager.is_dragging)
                {
                    // ������ �巡���� :: ������ ���
                    case true:
                        items_drop(eventdata, eventmanager);
                        break;

                    // ������ �巡������ �ƴ� :: return
                    case false:
                        return;
                }
                break;

            // �������� �ִ� ���� Ŭ��
            case false:
                switch (eventmanager.is_dragging)
                {
                    // �巡�� �� :: ���Կ� �ִ� �����۰� �巡�� ���� ������ ���� ����, ���(�������� ���� ���)
                    case true:
                        items_swap_drop(eventdata, eventmanager);
                        break;

                    // �巡�� ���� �ƴ� :: ���� ������ �巡��
                    case false:
                        items_drag(eventdata, eventmanager);    // ������ �巡�� :: �Լ� �� ���� ��,�� Ŭ�� �и�
                        return;
                }
                break;
        }
    }

    // ������ �巡��
    private void items_drag(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();
        int pickup_item_count = 0;

        switch (eventdata.pointerId)
        {
            // �� Ŭ�� :: ���Կ� �ִ� ��� ������ �巡��
            case -1:
                pickup_item_count = item_stack.Count;
                break;

            // �� Ŭ�� :: ���Կ� �ִ� ������ ���� �巡��
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

    // ������ ���Կ� ���
    private void items_drop(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();
        int drop_item_count = 0;

        switch (eventdata.pointerId)
        {
            /*
             * �� Ŭ��
             * 1. �� ���� :: �巡�� ���� ������ ��� ���
             * 2. ������ �ִ� ���� :: ������ ������ �ִ� ������ŭ ���
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

            // �� Ŭ�� :: ���Կ� ������ 1�� ���
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

    // ������ ���� , ���
    private void items_swap_drop(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();

        // �巡�� �����۰� ���� ������ ���� ��
        switch(get_item_info(dragging_item) == get_item_info(this))
        {
            // ���� ������ :: ������ ���
            case true:
                items_drop(eventdata, eventmanager);
                break;

            // �ٸ� ������ :: �巡�� �����۰� ���� ������ ������ ����
            case false:
                Core.swap<Stack<Item>>(ref dragging_item.item_stack, ref item_stack);
                update_UI(dragging_item);
                update_UI(this);
                break;
        }
    }
}
