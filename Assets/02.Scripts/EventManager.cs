using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 *  Ű �Է�, ��ư Ŭ�� �̺�Ʈ�� ������ ��ũ��Ʈ
 */

public class EventManager : Singleton_Mono<EventManager>
{
    private GameObject m_CraftingTablePanel = null;     // ���۴� �ǳ�

    public GameObject m_DraggingItem = null;            // �巡�� ������ ������Ʈ
    public bool m_IsDragging;                           // �巡�� Ȯ�ο� ����

    void Start()
    {
        m_CraftingTablePanel = UIManager.GetInstance.m_CraftingTablePanel;
        m_IsDragging = false;
    }

    // ������ �߰� ��ư Ŭ�� �� �����ϴ� �Լ�
    public void _On_AddItemBtnClick()
    {
        GameObject currClickBtn = EventSystem.current.currentSelectedGameObject; // ���� Ŭ���� ���� ������Ʈ
        Item currClickItem = currClickBtn.GetComponent<ItemInfo>().m_ItemInfo;   // Ŭ���� ������ ����

        Inventory.AddItemToInventory(currClickItem);                             // �κ��丮�� Ŭ���� ������ �߰�
    }

    
    void Update()
    {
        // Tab :: ���۴� ����, �ݱ�
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.GetInstance.GameObjectSetActive(m_CraftingTablePanel);
        }

        // �巡���� ������ ���콺 �����Ϳ� ��ġ ��Ű��
        if (m_IsDragging)
        {
            m_DraggingItem.transform.position = Input.mousePosition;
        }
    }
}
