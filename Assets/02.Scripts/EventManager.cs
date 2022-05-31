using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 *  키 입력, 버튼 클릭 이벤트를 관리할 스크립트
 */

public class EventManager : Singleton_Mono<EventManager>
{
    private GameObject m_CraftingTablePanel = null;     // 제작대 판넬

    public GameObject m_DraggingItem = null;            // 드래그 아이템 오브젝트
    public bool m_IsDragging;                           // 드래그 확인용 변수

    void Start()
    {
        m_CraftingTablePanel = UIManager.GetInstance.m_CraftingTablePanel;
        m_IsDragging = false;
    }

    // 아이템 추가 버튼 클릭 시 동작하는 함수
    public void _On_AddItemBtnClick()
    {
        GameObject currClickBtn = EventSystem.current.currentSelectedGameObject; // 현재 클릭한 게임 오브젝트
        Item currClickItem = currClickBtn.GetComponent<ItemInfo>().m_ItemInfo;   // 클릭한 아이템 정보

        Inventory.AddItemToInventory(currClickItem);                             // 인벤토리에 클릭한 아이템 추가
    }

    
    void Update()
    {
        // Tab :: 제작대 열기, 닫기
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.GetInstance.GameObjectSetActive(m_CraftingTablePanel);
        }

        // 드래그중 아이템 마우스 포인터에 위치 시키기
        if (m_IsDragging)
        {
            m_DraggingItem.transform.position = Input.mousePosition;
        }
    }
}
