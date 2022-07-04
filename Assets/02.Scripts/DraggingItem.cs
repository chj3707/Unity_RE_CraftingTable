using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  드래그 중인 아이템
 */

public class DraggingItem : Slot
{
    private void Start()
    {
        initialize();
    }

    protected override void initialize()
    {
        item_image = GetComponent<Image>();
        item_quantity_text = GetComponentInChildren<Text>();

        item_image.enabled = false;
        item_quantity_text.enabled = false;
    }
}
