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

    // ������ ���� ���� ���� ������ ȣ���Ͽ� UI ������Ʈ
    public void update_UI()
    {
        // ������ ������ ����� ��
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
        // ���� �� �ִ� ������, ������ ������ 1���� ũ�� ����(Text) ǥ��
        item_quantity_text.enabled = current_item.is_stackable ? true : false;
        item_quantity_text.enabled = 1 == get_item_stack_quantity() ? false : true;
        item_quantity_text.text = item_stack.Count.ToString();
    }

    // ���� ���ÿ� ���� ������ ���� ��������
    public int get_item_stack_quantity()
    {
        return item_stack.Count;
    }

    // �������� �ִ� ���� ���� ��������
    public int get_max_item_stack()
    {
        return get_top_item_info().is_stackable ? MaxItemStack.stackable : MaxItemStack.non_stackable;
    }

    // �ֻ����� �ִ� ������ ���� ��������
    public Item get_top_item_info()
    {
        return true == is_item_stack_empty() ? null : item_stack.Peek();
    }

    // �ֻ����� �ִ� ������ �̸� ��������
    public string get_top_item_name()
    {
        return true == is_item_stack_empty() ? string.Empty : get_top_item_info().item_name;
    }

    // ������ ������ ���� á�°�
    public bool is_item_stack_full()
    {
        Item temp_item = get_top_item_info();
        if (true == temp_item.is_stackable && item_stack.Count == MaxItemStack.stackable) return true;
        else if (false == temp_item.is_stackable && item_stack.Count == MaxItemStack.non_stackable) return true;
        return false;
    }

    // ������ ������ ��� �ִ°�
    public bool is_item_stack_empty()
    {
        if (0 == item_stack.Count) return true;
        return false;
    }
}
