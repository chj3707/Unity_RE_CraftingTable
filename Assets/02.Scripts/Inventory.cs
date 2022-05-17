using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int m_SlotCount;

    void Start()
    {
        SlotGenerator.GetInstance.CreateSlot(m_SlotCount, this.transform);
    }
}
