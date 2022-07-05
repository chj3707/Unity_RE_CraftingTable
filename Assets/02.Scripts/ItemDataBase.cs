using System.Collections.Generic;
using UnityEngine;
using System;

/*
 *  아이템 데이터베이스 (아이템 정보, 아이템 레시피 정보)
 *  아이템 정보 세팅, 싱글톤 적용
 */

public class ItemRecipe
{
    private string item_name;       // 아이템 이름
    private string[,] recipe;       // 아이템 레시피
    private int material_quantity;  // 필요한 총 재료량

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
    public Dictionary<string, Item> item_database = null;                      // 아이템 정보
    public List<Dictionary<string, ItemRecipe>> item_recipe_database = null;   // 아이템 레시피
    
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

    // 스크립터블 오브젝트로 만들어둔 아이템들 아이템 데이터베이스에 세팅
    private void set_items_info()
    {
        Item[] items_info = Resources.LoadAll<Item>("ScriptableObject/Items");      
        int item_count = items_info.Length;                                        

        for (int i = 0; i < item_count; i++)
        {
            item_database.Add(items_info[i].item_name, items_info[i]);           
        }
    }

    // CSV 데이터 읽어서 아이템 레시피 세팅
    private void set_items_recipe_info()
    {
        List<Dictionary<string, object>> csv_data = CSVReader.Read("CSV/Recipe");
        int recipe_count = csv_data.Count;

        for (int i = 0; i < recipe_count; i++)
        {
            string item_name = "";                       // 조합 아이템 이름
            string[,] item_recipe = new string[3, 3];    // 조합 아이템 레시피
            int material_quantity = 0;                   // 조합 아이템 재료 개수

            /* object 형식 데이터 언박싱 */
            item_name = csv_data[i]["조합 아이템"] as string;                                        

            /* 조합법 :: CSV파일 작성 할 때 0,0 ~ 2,2 형식으로 구성  */
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    /*
                     * 1. j,k 위치의 데이터 읽기 (CSV 파일 데이터)
                     * 2. 비었으면 ? 빈 문자열 : 재료 아이템 이름
                     * 3. 비었으면 ? 0 : 1  재료 개수 카운트
                     */
                    string material_item_name = csv_data[i][$"\"{j},{k}\""] as string;             
                    item_recipe[j, k] = string.IsNullOrEmpty(material_item_name) ? string.Empty : material_item_name;  
                    material_quantity += string.IsNullOrEmpty(material_item_name) ? 0 : 1;                  
                }
            }

            /* 언박싱한 데이터 객체에 저장, 데이터 베이스에 추가 */
            ItemRecipe temp_item_recipe = new ItemRecipe();
            temp_item_recipe.ItemName = item_name;
            temp_item_recipe.Recipe = item_recipe;
            temp_item_recipe.MaterialAmount = material_quantity;
            item_recipe_database[material_quantity].Add(item_name, temp_item_recipe);
        }
    }
}
