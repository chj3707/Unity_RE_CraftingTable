using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Ű �Է�, ��ư Ŭ�� �̺�Ʈ�� ������ ��ũ��Ʈ
 */

public class EventManager : MonoBehaviour
{
    public GameObject m_CraftingTablePanel = null;

    void Start()
    {
        
    }

    void Update()
    {
        // Tab :: ���۴� ����, �ݱ�
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_CraftingTablePanel.activeSelf) m_CraftingTablePanel.SetActive(false);
            else m_CraftingTablePanel.SetActive(true);
        }
    }
}
