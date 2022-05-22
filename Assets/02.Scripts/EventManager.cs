using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  키 입력, 버튼 클릭 이벤트를 관리할 스크립트
 */

public class EventManager : MonoBehaviour
{
    public GameObject m_CraftingTablePanel = null;

    void Start()
    {
        
    }

    void Update()
    {
        // Tab :: 제작대 열기, 닫기
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_CraftingTablePanel.activeSelf) m_CraftingTablePanel.SetActive(false);
            else m_CraftingTablePanel.SetActive(true);
        }
    }
}
