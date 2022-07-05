using System.Collections.Generic;
using UnityEngine;
using System;

/*
 *  ������ �����ͺ��̽� (������ ����, ������ ������ ����)
 *  ������ ���� ����, �̱��� ����
 */

public class ItemRecipe
{
    private string item_name;       // ������ �̸�
    private string[,] recipe;       // ������ ������
    private int material_quantity;  // �ʿ��� �� ��ᷮ

    public ItemRecipe()
    {
        item_name = String.Empty;
        recipe = new string[3, 3];
        material_quantity = 0;
    }
    public string ItemName
    {
        get { return item_name; }
        set { item_name = value; }
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
    }

    private void initialize()
    {
        item_database = new Dictionary<string, Item>();
        item_recipe_database = new List<Dictionary<string, ItemRecipe>>();

        for (int i = 0; i < Workbench.workbench_size + 1; i++)
        {
            item_recipe_database.Add(new Dictionary<string, ItemRecipe>());
        }
    }

    public Dictionary<string, ItemRecipe> get_item_recipe_data(int material_quantity)
    {
        return item_recipe_database[material_quantity];
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
            string item_name = "";                       // ���� ������ �̸�
            string[,] item_recipe = new string[3, 3];    // ���� ������ ������
            int material_quantity = 0;                   // ���� ������ ��� ����

            /* object ���� ������ ��ڽ� */
            item_name = csv_data[i]["���� ������"] as string;                                        

            /* ���չ� :: CSV���� �ۼ� �� �� 0,0 ~ 2,2 �������� ����  */
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    /*
                     * 1. j,k ��ġ�� ������ �б� (CSV ���� ������)
                     * 2. ������� ? �� ���ڿ� : ��� ������ �̸�
                     * 3. ������� ? 0 : 1  ��� ���� ī��Ʈ
                     */
                    string material_item_name = csv_data[i][$"\"{j},{k}\""] as string;             
                    item_recipe[j, k] = string.IsNullOrEmpty(material_item_name) ? string.Empty : material_item_name;  
                    material_quantity += string.IsNullOrEmpty(material_item_name) ? 0 : 1;                  
                }
            }

            /* ��ڽ��� ������ ��ü�� ����, ������ ���̽��� �߰� */
            ItemRecipe temp_item_recipe = new ItemRecipe();
            temp_item_recipe.ItemName = item_name;
            temp_item_recipe.Recipe = item_recipe;
            temp_item_recipe.MaterialAmount = material_quantity;
            item_recipe_database[material_quantity].Add(item_name, temp_item_recipe);
        }
    }
}
