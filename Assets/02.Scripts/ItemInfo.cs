using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MaxItemStack
{
    public static int stackable = 64;
    public static int non_stackable = 1;
}

public class ItemInfo
{
    public Stack<Item> item_stack;
    public Image item_image;                  
    public Text item_quantity_text;

    public ItemInfo()
    {
        item_stack = new Stack<Item>();
        item_image = null;
        item_quantity_text = null;
    }

    // 아이템 변경 내용 있을 때마다 호출하여 UI 업데이트
    public void update_UI()
    {
        // 아이템 스택이 비었을 때
        if (true == is_item_stack_empty())
        {
            item_image.sprite = null;
            item_image.enabled = false;

            item_quantity_text.text = item_stack.Count.ToString();
            item_quantity_text.enabled = false;
            return;
        }

        Item current_item = get_top_item_info();
        item_image.enabled = true;
        item_image.sprite = current_item.item_sprite;
        // 쌓을 수 있는 아이템, 아이템 스택이 1보다 크면 개수(Text) 표시
        item_quantity_text.enabled = current_item.is_stackable ? true : false;
        item_quantity_text.enabled = 1 == get_item_stack_quantity() ? false : true;
        item_quantity_text.text = item_stack.Count.ToString();
    }

    // 현재 스택에 쌓인 아이템 개수 가져가기
    public int get_item_stack_quantity()
    {
        return item_stack.Count;
    }

    // 아이템의 최대 스택 개수 가져가기
    public int get_max_item_stack()
    {
        return get_top_item_info().is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;
    }

    // 최상위에 있는 아이템 정보 가져가기
    public Item get_top_item_info()
    {
        return true == is_item_stack_empty() ? null : item_stack.Peek();
    }

    // 최상위에 있는 아이템 이름 가져가기
    public string get_top_item_name()
    {
        return true == is_item_stack_empty() ? string.Empty : get_top_item_info().item_name;
    }

    // 아이템 스택이 가득 찼는가
    public bool is_item_stack_full()
    {
        Item temp_item = get_top_item_info();
        if (true == temp_item.is_stackable && item_stack.Count == MaxItemStack.stackable) return true;
        else if (false == temp_item.is_stackable && item_stack.Count == MaxItemStack.non_stackable) return true;
        return false;
    }

    // 아이템 스택이 비어 있는가
    public bool is_item_stack_empty()
    {
        if (0 == item_stack.Count) return true;
        return false;
    }
}
