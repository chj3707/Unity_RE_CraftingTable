using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchSlot : Slot
{
    public void start_initialize()
    {
        initialize();
    }

    protected override void initialize()
    {
        base.initialize();
    }
}

public class Workbench : MonoBehaviour
{
    private WorkbenchSlot[,] workbench = null;
    private static int row = 3;
    private static int col = 3;
    public static int workbench_size = row * col;

    public GameObject slot_prefab = null;

    void Awake()
    {
        init_workbench();
    }

    private void init_workbench()
    {
        if (true == slot_prefab.activeSelf) slot_prefab.SetActive(false);
        slot_prefab.AddComponent<WorkbenchSlot>().start_initialize();

        workbench = new WorkbenchSlot[row, col];
        UIManager.GetInstance.generate_gameobject(slot_prefab, row, col, transform, ref workbench);
    }
}
