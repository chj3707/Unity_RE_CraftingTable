using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  드래그 중인 아이템
 */

public class DraggingItem : MonoBehaviour
{
    public ItemInfo item_info;

    private void Start()
    {
        initialize();
    }

    private void initialize()
    {
        item_info = new ItemInfo();
        item_info.item_image = GetComponent<Image>();
        item_info.item_quantity_text = GetComponentInChildren<Text>();

        item_info.item_image.enabled = false;
        item_info.item_quantity_text.enabled = false;
    }
}
