using System.Collections.Generic;
using UnityEngine;
using System;

/*
 *  ������ �����ͺ��̽� (������ ����, ������ ������ ����)
 *  ������ ���� ����, �̱��� ����
 */

public class ItemRecipe
{
    private string[,] recipe;       // ������ ������
    private int material_quantity;  // �ʿ��� �� ��ᷮ

    public ItemRecipe()
    {
        recipe = new string[3, 3];
        material_quantity = 0;
    }

    public string[,] Recipe
    {
        get { return recipe; }
        set { recipe = value; }
    }

    public int MaterialAmount
    {
        get { return material_quantity; }
        set { material_quantity = value; }
    }
}

public class ItemDataBase : Singleton_Mono<ItemDataBase>
{
    public Dictionary<string, Item> item_database = null;                      // ������ ����
    public List<Dictionary<string, ItemRecipe>> item_recipe_database = null;   // ������ ������
    
    
    private void Awake()
    {
        initialize();
        set_items_info();
        set_items_recipe_info();

        for (int i = 0; i < item_recipe_database.Count; i++)
        {
            //Dictionary<string, ItemRecipe>.KeyCollection itemsName = item_recipe_database[i].Keys;
            //Dictionary<string, ItemRecipe>.ValueCollection itemsRecipe = item_recipe_database[i].Values;

            foreach (var item_recipes in item_recipe_database[i])
            {

                Debug.LogFormat($"������ �̸� : {0}\n" +
                                $"������ ���� : {1}\n" +
                                $"---������ ������---\n" +
                                $"{2}\t{3}\t{4}\n" +
                                $"{5}\t{6}\t{7}\n" +
                                $"{8}\t{9}\t{10}",
                                item_recipes.Key, i,
                                item_recipes.Value.Recipe[0, 0], item_recipes.Value.Recipe[0, 1], item_recipes.Value.Recipe[0, 2],
                                item_recipes.Value.Recipe[1, 0], item_recipes.Value.Recipe[1, 1], item_recipes.Value.Recipe[1, 2],
                                item_recipes.Value.Recipe[2, 0], item_recipes.Value.Recipe[2, 1], item_recipes.Value.Recipe[2, 2]);
            }
        }
    }

    private void initialize()
    {
        item_database = new Dictionary<string, Item>();
        item_recipe_database = new List<Dictionary<string, ItemRecipe>>(new Dictionary<string, ItemRecipe>[Workbench.workbench_size + 1]);
    }

    // ��ũ���ͺ� ������Ʈ�� ������ �����۵� ������ �����ͺ��̽��� ����
    private void set_items_info()
    {
        Item[] items_info = Resources.LoadAll<Item>("ScriptableObject/Items");      
        int item_count = items_info.Length;                                        

        for (int i = 0; i < item_count; i++)
        {
            item_database.Add(items_info[i].item_name, items_info[i]);           
        }
    }

    // CSV ������ �о ������ ������ ����
    private void set_items_recipe_info()
    {
        List<Dictionary<string, object>> csv_data = CSVReader.Read("CSV/Recipe");
        int recipe_count = csv_data.Count;

        for (int i = 0; i < recipe_count; i++)
        {
            string item_name = "";                                                                  // ���� ������ �̸� ���� �뵵  
            string[,] item_recipe = new string[3, 3];                                               // ���� ������ ������ ���� �뵵
            int material_quantity = 0;                                                              // ���� �������� �� ��ᷮ

            /* object ���� ������ ��ڽ� */
            item_name = csv_data[i]["���� ������"] as string;                                         // ���� ������ �̸�

            /* ���չ� :: CSV���� �ۼ� �� �� 0,0 ~ 2,2 �������� ����  */
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    string temp_str = csv_data[i][$"\"{j},{k}\""] as string;                        // j,k ��ġ�� ��� ������ �̸�
                    item_recipe[j, k] = string.IsNullOrEmpty(temp_str) ? string.Empty : temp_str;   // ���ڿ� ������� �� ���ڿ� : ��� ������ �̸�
                    material_quantity += string.IsNullOrEmpty(temp_str) ? 0 : 1;                    // ���ڿ� ������� 0 : 1 ī��Ʈ (���� �� �� �� ���)
                }
            }

            /* ��ü�� �� �Ҵ� �� ��ųʸ� �������� ������ ���� */
            ItemRecipe temp_item_recipe = new ItemRecipe();                                         // ���� ������ �����Ǹ� ������ �ӽ� ��ü
            temp_item_recipe.Recipe = item_recipe;
            temp_item_recipe.MaterialAmount = material_quantity;
            item_recipe_database[material_quantity].Add(item_name, temp_item_recipe);
        }
    }
}
