using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  UI 관리용 스크립트
 */

public class UIManager : Singleton_Mono<UIManager>
{
    public GameObject workbench_panel_obj = null; 

    void Start()
    {
        initialize();
    }

    private void initialize()
    {
        if (true == workbench_panel_obj.activeSelf) workbench_panel_obj.SetActive(false);
    }

    // 게임오브젝트 [비활성화 -> 활성화 , 활성화 -> 비활성화] 처리
    public void set_gameobject_active(GameObject request_object)
    {
        if (true == request_object.activeSelf) request_object.SetActive(false);
        else request_object.SetActive(true);
    }

    // 사용처 :: 아이템 추가 버튼 생성
    public void generate_gameobject(GameObject request_object, Item item_info, Transform parent_transform)
    {
        GameObject copy_object = GameObject.Instantiate(request_object);        
        copy_object.SetActive(true);                                            
        copy_object.GetComponent<ItemInfo>().item_info = item_info;              
        copy_object.GetComponent<Image>().sprite = item_info.item_sprite;        
        copy_object.name = string.Format($"Add Item [{item_info.item_name}]");  
        copy_object.transform.SetParent(parent_transform);                      
    }

    // 사용처 :: 인벤토리 슬롯 생성
    public void generate_gameobject(GameObject request_object, int copy_count, Transform parent_transform, ref List<InventorySlot> slot_list)
    {
        for (int i = 0; i < copy_count; i++)
        {
            GameObject copy_object = GameObject.Instantiate(request_object);    
            copy_object.SetActive(true);                                        
            copy_object.name = string.Format($"Slot_{i + 1} ");                 
            copy_object.transform.SetParent(parent_transform);                  
            slot_list.Add(copy_object.GetComponent<InventorySlot>());           
        }
    }

    // 사용처 :: 제작대 슬롯 생성
    public void generate_gameobject(GameObject request_object, int row, int col, Transform parent_transform, ref WorkbenchSlot[,] workbench)
    {
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                GameObject copy_object = GameObject.Instantiate(request_object);
                copy_object.SetActive(true);                                
                copy_object.name = string.Format($"Slot_[{y},{x}]");        
                copy_object.transform.SetParent(parent_transform);          
                workbench[y, x] = copy_object.GetComponent<WorkbenchSlot>();
            }
        }
    }
}
