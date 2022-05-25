using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 *  Ű �Է�, ��ư Ŭ�� �̺�Ʈ�� ������ ��ũ��Ʈ
 */

public class EventManager : MonoBehaviour
{
    private GameObject m_CraftingTablePanel = null;     // ���۴� �ǳ�

    void Start()
    {
        m_CraftingTablePanel = UIManager.GetInstance.m_CraftingTablePanel; 
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
    }
}
