using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItem : MonoBehaviour, 
                            IPointerClickHandler
{
    public ItemInfo item_info;

    private Workbench workbench;

    void Start()
    {
        initialize();
    }

    private void initialize()
    {
        item_info = new ItemInfo();

        item_info.item_image = transform.GetChild(0).GetComponent<Image>();
        item_info.item_quantity_text = item_info.item_image.GetComponentInChildren<Text>();

        item_info.item_image.enabled = false;
        item_info.item_quantity_text.enabled = false;

        workbench = GetComponentInParent<Workbench>();
    }

    public void reset_item_slot()
    {
        this.item_info.item_stack.Clear();
        this.item_info.update_UI();
    }

    // �ϼ�ǰ �巡��
    void IPointerClickHandler.OnPointerClick(PointerEventData eventdata)
    {
        if (true == item_info.is_item_stack_empty()) return;

        EventManager eventmanager = EventManager.GetInstance;

        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();

        int pickup_item_count = this.item_info.get_item_stack_quantity();

        eventmanager.is_dragging = true;

        // ���� ������ ���� Pop(), �巡�� ������ ���� Push() �ݺ�
        for (int i = 0; i < pickup_item_count; i++)
            dragging_item.item_info.item_stack.Push(this.item_info.item_stack.Pop());

        workbench.consume_material_items();
        dragging_item.item_info.update_UI();
        this.item_info.update_UI();
        workbench.compare_workbench_and_recipes();
    }
}