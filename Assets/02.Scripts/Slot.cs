using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 *  ���� ���� ��ũ��Ʈ 
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

    // �ʱ�ȭ
    protected virtual void initialize()
    {
        item_info = new ItemInfo();
        item_info.item_image = transform.GetChild(0).GetComponent<Image>();
        item_info.item_quantity_text = item_info.item_image.GetComponentInChildren<Text>();

        item_info.item_image.enabled = false;
        item_info.item_quantity_text.enabled = false;
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

        switch (item_info.is_item_stack_empty())
        {
            // �� ���� Ŭ��
            case true:
                switch (eventmanager.is_dragging)
                {
                    // ������ �巡������ �ƴ� :: return
                    case false: return;

                    // ������ �巡���� :: ������ ���
                    case true:
                        items_drop(eventdata, eventmanager);
                        if (true == is_workbench_slot) ++Workbench.workbench_material_quantity;
                        break;
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
                        break;
                }
                break;
        }

        // ��ũ��ġ ������ Ŭ�� �ѰŶ�� ���� ����
        if (true == is_workbench_slot)
        {
            Workbench temp_workbench = this.GetComponentInParent<Workbench>();
            temp_workbench.compare_workbench_with_recipes();
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
                pickup_item_count = item_info.get_item_stack_quantity();
                if (true == is_workbench_slot) --Workbench.workbench_material_quantity;
                break;

            // �� Ŭ�� :: ���Կ� �ִ� ������ ���� �巡��
            case -2:
                pickup_item_count = Mathf.CeilToInt(item_info.get_item_stack_quantity() * 0.5f);
                if (true == is_workbench_slot && 1 == item_info.get_item_stack_quantity()) --Workbench.workbench_material_quantity;
                break;
        }
        eventmanager.is_dragging = true;

        // ���� ������ ���� Pop(), �巡�� ������ ���� Push() �ݺ�
        for (int i = 0; i < pickup_item_count; i++)
            dragging_item.item_info.item_stack.Push(this.item_info.item_stack.Pop());

        dragging_item.item_info.update_UI();
        this.item_info.update_UI();
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

            // �� Ŭ�� :: ���Կ� ������ 1�� ���
            case -2:
                if (true == this.item_info.is_item_stack_empty() ||
                    this.item_info.get_item_stack_quantity() < this.item_info.get_max_item_stack())
                {
                    drop_item_count = 1;
                }
                break;
        }

        // �巡�� ������ ���� Pop(), ���� ������ ���� Push() �ݺ�
        for (int i = 0; i < drop_item_count; i++)
            this.item_info.item_stack.Push(dragging_item.item_info.item_stack.Pop());

        dragging_item.item_info.update_UI();
        this.item_info.update_UI();

        if (true == dragging_item.item_info.is_item_stack_empty())
            eventmanager.is_dragging = false;
    }

    // ������ ���� , ���
    private void items_swap_drop(PointerEventData eventdata, EventManager eventmanager)
    {
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();

        // ���� ������ :: ������ ���
        if (dragging_item.item_info.get_top_item_info() == this.item_info.get_top_item_info())
        {
            items_drop(eventdata, eventmanager); 
            return;
        }
        else // �ٸ� ������ :: ������ ������ ����
        {
            Core.swap<Stack<Item>>(ref dragging_item.item_info.item_stack, ref this.item_info.item_stack);
            dragging_item.item_info.update_UI();
            this.item_info.update_UI();
        }
        
        

        //// �巡�� �����۰� ���� ������ ���� ��
        //switch (dragging_item.item_info.get_top_item_info() == this.item_info.get_top_item_info())
        //{
        //    // ���� ������ :: ������ ���
        //    case true:
        //        items_drop(eventdata, eventmanager);
        //        break;

        //    // �ٸ� ������ :: ������ ������ ����
        //    case false:
        //        Core.swap<Stack<Item>>(ref dragging_item.item_info.item_stack, ref this.item_info.item_stack);
        //        dragging_item.item_info.update_UI();
        //        this.item_info.update_UI();
        //        break;
        //}
    }
}
