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
    private int material_quantity;  // 필요한 총 재료량

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
    public Dictionary<string, Item> item_database = null;                      // 아이템 정보
    public List<Dictionary<string, ItemRecipe>> item_recipe_database = null;   // 아이템 레시피
    
    
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

                Debug.LogFormat($"아이템 이름 : {0}\n" +
                                $"아이템 개수 : {1}\n" +
                                $"---아이템 레시피---\n" +
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
            string item_name = "";                                                                  // 조합 아이템 이름 저장 용도  
            string[,] item_recipe = new string[3, 3];                                               // 조합 아이템 레시피 저장 용도
            int material_quantity = 0;                                                              // 조합 아이템의 총 재료량

            /* object 형식 데이터 언박싱 */
            item_name = csv_data[i]["조합 아이템"] as string;                                         // 조합 아이템 이름

            /* 조합법 :: CSV파일 작성 할 때 0,0 ~ 2,2 형식으로 구성  */
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    string temp_str = csv_data[i][$"\"{j},{k}\""] as string;                        // j,k 위치의 재료 아이템 이름
                    item_recipe[j, k] = string.IsNullOrEmpty(temp_str) ? string.Empty : temp_str;   // 문자열 비었으면 빈 문자열 : 재료 아이템 이름
                    material_quantity += string.IsNullOrEmpty(temp_str) ? 0 : 1;                    // 문자열 비었으면 0 : 1 카운트 (조합 비교 할 때 사용)
                }
            }

            /* 객체에 값 할당 후 딕셔너리 형식으로 데이터 세팅 */
            ItemRecipe temp_item_recipe = new ItemRecipe();                                         // 조합 아이템 레시피를 저장할 임시 객체
            temp_item_recipe.Recipe = item_recipe;
            temp_item_recipe.MaterialAmount = material_quantity;
            item_recipe_database[material_quantity].Add(item_name, temp_item_recipe);
        }
    }
}
