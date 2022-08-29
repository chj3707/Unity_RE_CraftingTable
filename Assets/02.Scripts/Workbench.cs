using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Workbench : MonoBehaviour
{
    private Slot[,] workbench = null;
    private static int row = 3;
    private static int col = 3;
    public static int workbench_size = row * col;
    public static int workbench_material_quantity = 0;

    public Transform workbench_slots_parent = null;

    private CraftingItem crafting_item_slot;

    void Awake()
    {
        initialize();
    }

    private void initialize()
    {
        workbench = new Slot[row, col];
        Queue<Slot> workbench_slots = new Queue<Slot>(workbench_slots_parent.GetComponentsInChildren<Slot>());

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Slot temp_slot = workbench_slots.Dequeue();
                temp_slot.is_workbench_slot = true;
                workbench[i, j] = temp_slot;
            }
        }

        crafting_item_slot = GetComponentInChildren<CraftingItem>();
    }

    /*
     * �۾��뿡 �ö� �ִ� �����۰� ������ ������ ���Ͽ� ���� �� �ִ� ���� ������ ã��
     * 1. ������ ������ ������ ���̽� ��������
     * 2. ������ �����ǵ� ���۴�� ���ؼ� ���� ������ Ž��
     */
    public void compare_workbench_and_recipes()
    {
        var item_recipe_datas = ItemDataBase.GetInstance.get_item_recipe_data(workbench_material_quantity);

        foreach (var item_name_recipe in item_recipe_datas)
        {
            string crafting_item_name = item_name_recipe.Key;
            string[,] crafting_item_recipe = item_name_recipe.Value.Recipe;

            bool flag = true;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    string material_item_name = workbench[i, j].item_info.get_top_item_name();
                    bool is_material_item_name_contains_suffix = is_common_material(material_item_name, ECommonMaterial.none);
                    bool is_crafting_item_name_contains_suffix = is_common_material(crafting_item_name, ECommonMaterial.none);

                    if (true == is_material_item_name_contains_suffix)
                    {
                        string[] split_item_name = material_item_name.Split(' ');
                        string prefix = split_item_name[0];
                        string suffix = split_item_name[1];

                        if (crafting_item_recipe[i, j] != suffix)
                        {
                            flag = false;
                            break;
                        }

                        if (true == is_crafting_item_name_contains_suffix)
                            crafting_item_name = prefix + " " + crafting_item_name;
                    }

                    else if (crafting_item_recipe[i, j] != material_item_name)
                    {
                        flag = false;
                        break;
                    }
                }
                if (false == flag) break;
            }

            if (true == flag)
            {
                Item crafting_item = ItemDataBase.GetInstance.item_database[crafting_item_name];

                for (int i = 0; i < item_name_recipe.Value.CreateQuantity; i++)
                    crafting_item_slot.item_info.item_stack.Push(crafting_item);

                crafting_item_slot.item_info.update_UI();
                return;
            }
        }

        // �����ǿ��� Ž���� �ȵǾ����� ���� ������ ���� ����
        crafting_item_slot.reset_item_slot();
    }

    public bool is_common_material(string item_name, ECommonMaterial from)
    {
        for (ECommonMaterial suffix = from + 1; suffix < ECommonMaterial.max; suffix++)
        {
            if (true == item_name.Contains(suffix.ToString())) return true;
        }

        return false;
    }

    public string is_consume_material(string item_name, ENonConsumeMaterialItem from)
    {
        for (ENonConsumeMaterialItem item = from + 1; item < ENonConsumeMaterialItem.max; item++)
        {
            if (true == item_name.Contains(item.ToString())) return item.ToString();
        }

        return String.Empty;
    }

    public void consume_material_items()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                ItemInfo current_item = workbench[i, j].item_info;
                if (null != current_item.get_top_item_info())
                {
                    string material_item_name = is_consume_material(current_item.get_top_item_name(), ENonConsumeMaterialItem.none);
                    if (String.Empty == material_item_name)
                    {
                        current_item.item_stack.Pop();
                        if (0 == current_item.get_item_stack_quantity()) --workbench_material_quantity;
                        current_item.update_UI();
                        continue;
                    }

                    current_item.item_stack.Clear();
                    current_item.item_stack.Push(ItemDataBase.GetInstance.item_database[material_item_name]);
                    current_item.update_UI();
                }
            }
        }
    }
}
