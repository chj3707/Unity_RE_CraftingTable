using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    private Slot[,] workbench = null;
    private static int row = 3;
    private static int col = 3;
    public static int workbench_size = row * col;
    public static int workbench_material_quantity = 0;

    public GameObject slot_prefab = null;

    void Awake()
    {
        initialize();
    }

    private void initialize()
    {
        if (true == slot_prefab.activeSelf) slot_prefab.SetActive(false);
        workbench = new Slot[row, col];
        UIManager.GetInstance.generate_gameobject(slot_prefab, row, col, transform, ref workbench);
    }

    /*
     * �۾��뿡 �ö� �ִ� �����۰� ������ ������ ���Ͽ� ���� �� �ִ� ���� ������ ã��
     * 1. ������ ������ ������ ���̽� ��������
     * 2. ������ �����ǵ� ���۴�� ���ؼ� ���� ������ ã��
     */
    public void compare_workbench_and_recipes()
    {
        var item_recipe_data = ItemDataBase.GetInstance.get_item_recipe_data(workbench_material_quantity);
        var item_recipes = item_recipe_data.Values;

        foreach (var item_recipe in item_recipes)
        {
            bool flag = true;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    string material_item_name = workbench[i, j].get_item_name();
                    if (item_recipe.Recipe[i, j] != material_item_name)
                    {
                        flag = false;
                        break;
                    }
                }
                if (false == flag) break;
            }

            if (true == flag)
            {
                Debug.Log($"���� ������ : {item_recipe.ItemName}");
                break;
            }
        }
    }
}
