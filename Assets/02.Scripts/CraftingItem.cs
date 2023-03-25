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

    // 완성품 드래그
    void IPointerClickHandler.OnPointerClick(PointerEventData eventdata)
    {
        if (true == item_info.is_item_stack_empty()) return;

        EventManager eventmanager = EventManager.GetInstance;
        DraggingItem dragging_item = eventmanager.dragging_item_obj.GetComponent<DraggingItem>();
        if (false == dragging_item.item_info.is_item_stack_empty() && true == dragging_item.item_info.is_item_stack_full()) return;
        if (true == eventmanager.is_dragging && dragging_item.item_info.get_item_info() != this.item_info.get_item_info()) return;

        int pickup_item_quantity = this.item_info.get_current_item_quantity();
        eventmanager.is_dragging = true;

        // 슬롯 아이템 스택 Pop(), 드래그 아이템 스택 Push() 반복
        for (int i = 0; i < pickup_item_quantity; i++)
            dragging_item.item_info.item_stack.Push(this.item_info.item_stack.Pop());
            
        workbench.consume_material_item();
        dragging_item.item_info.update_UI();
        this.item_info.update_UI();
        workbench.compare_workbench_with_recipes();
    }
}
