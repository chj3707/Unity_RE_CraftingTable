using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    public GameObject m_SlotPrefab = null;
    public int m_SlotCount;

    private LinkedList<Slot> m_SlotList = new LinkedList<Slot>();

    void Start()
    {
        UIManager.GetInstance.GenerateObject(m_SlotPrefab, m_SlotCount, this.transform, ref m_SlotList);
    }

}
