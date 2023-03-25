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
     * 작업대에 올라가 있는 아이템과 아이템 레시피 비교하여 만들 수 있는 조합 아이템 찾기
     * 1. 아이템 레시피 데이터 베이스 가져오기
     * 2. 아이템 레시피들 제작대와 비교해서 조합 아이템 탐색
     */
    public void compare_workbench_with_recipes()
    {
        crafting_item_slot.reset_item_slot();
        var item_recipe_datas = ItemDataBase.GetInstance.get_item_recipe_data(workbench_material_quantity);

        foreach (var item_recipe in item_recipe_datas)
        {
            string crafting_item_name = item_recipe.Key;
            string[,] crafting_item_recipe = item_recipe.Value.Recipe;

            bool is_craftable = true;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    string material_item_name = workbench[i, j].item_info.get_item_name();

                    if (true == is_common_material(material_item_name, ECommonMaterial.none))
                    {
                        string[] split_item_name = material_item_name.Split(' ');
                        string prefix = split_item_name[0];
                        string suffix = split_item_name[1];

                        if (crafting_item_recipe[i, j] != suffix) { is_craftable = false; break; }
                        if (true == is_common_material(crafting_item_name, ECommonMaterial.none)) { crafting_item_name = prefix + " " + crafting_item_name; }
                    }
                    else if (crafting_item_recipe[i, j] != material_item_name) { is_craftable = false; break; }
                }

                if (false == is_craftable) break;
            }

            if (true == is_craftable) { item_crafting(crafting_item_name, item_recipe.Value.CreateQuantity); break; }
        }
    }
    private void item_crafting(string crafting_item_name, int create_quantity)
    {
        Item crafting_item = ItemDataBase.GetInstance.get_item_data(crafting_item_name);

        for (int i = 0; i < create_quantity; i++)
            crafting_item_slot.item_info.item_stack.Push(crafting_item);

        crafting_item_slot.item_info.update_UI();
    }

    public bool is_common_material(string item_name, ECommonMaterial from)
    {
        for (ECommonMaterial suffix = from + 1; suffix < ECommonMaterial.max; suffix++)
            if (true == item_name.Contains(suffix.ToString())) return true;

        return false;
    }

    public string search_nonconsume_material(string item_name, ENonConsumeMaterialItem from)
    {
        for (ENonConsumeMaterialItem item = from + 1; item < ENonConsumeMaterialItem.max; item++)
            if (true == item_name.Contains(item.ToString())) return item.ToString();

        return String.Empty;
    }

    public void consume_material_item()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                ItemInfo current_item_info = workbench[i, j].item_info;
                if (null == current_item_info.get_item_info()) continue;

                string material_item_name = search_nonconsume_material(current_item_info.get_item_name(), ENonConsumeMaterialItem.none);
                if (String.Empty == material_item_name)
                {
                    current_item_info.item_stack.Pop();
                    if (true == current_item_info.is_item_stack_empty()) --workbench_material_quantity;
                }
                else
                {
                    current_item_info.item_stack.Clear();
                    current_item_info.item_stack.Push(ItemDataBase.GetInstance.get_item_data(material_item_name));
                }
                current_item_info.update_UI();
            }
        }
    }
}
