using System.Collections.Generic;
using UnityEngine;
using System;

/*
 *  아이템 데이터베이스 (아이템 정보, 아이템 레시피 정보)
 *  아이템 정보 세팅, 싱글톤 적용
 */

public class ItemRecipe
{
    private string[,] recipe;       // 아이템 레시피
    private int create_quantity;    // 아이템 생성 개수
    private int material_quantity;  // 필요한 총 재료 개수

    public ItemRecipe()
    {
        recipe = new string[3, 3];
        create_quantity = 0;
        material_quantity = 0;
    }

    public string[,] Recipe
    {
        get { return recipe; }
        set { recipe = value; }
    }

    public int CreateQuantity
    {
        get { return create_quantity; }
        set { create_quantity = value; }
    }

    public int MaterialQuantity
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
        int workbench_size = Workbench.workbench_size;

        for (int i = 0; i < workbench_size + 1; i++)
            item_recipe_database.Add(new Dictionary<string, ItemRecipe>());
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
            item_database.Add(items_info[i].item_name, items_info[i]);           
    }

    // CSV 데이터 읽어서 아이템 레시피 세팅
    private void set_items_recipe_info()
    {
        List<Dictionary<string, object>> csv_data = CSVReader.Read("CSV/Recipe");
        int recipe_count = csv_data.Count;

        for (int i = 0; i < recipe_count; i++)
        {
            ItemRecipe curr_item_recipe = new ItemRecipe();  // 현재 아이템 레시피
            string item_name = "";                           // 조합 아이템 이름

            /* object 형식 데이터 언박싱 */
            item_name = csv_data[i]["조합 아이템"] as string;
            curr_item_recipe.CreateQuantity = int.Parse(csv_data[i]["생성 개수"].ToString());

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
                    curr_item_recipe.Recipe[j, k] = string.IsNullOrEmpty(material_item_name) ? string.Empty : material_item_name;
                    curr_item_recipe.MaterialQuantity += string.IsNullOrEmpty(material_item_name) ? 0 : 1;                  
                }
            }

            item_recipe_database[curr_item_recipe.MaterialQuantity].Add(item_name, curr_item_recipe);
        }
    }
}
