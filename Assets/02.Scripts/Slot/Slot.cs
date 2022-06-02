using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class MaxItemStack
{
    public static int Stackable = 64;
    public static int NonStackable = 1;
}

/*
 *  슬롯 관리 스크립트 
 */
public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Stack<Item> m_ItemStack = new Stack<Item>();  // 슬롯에 있는 아이템 정보
    protected Image m_ItemImage = null;                  // 아이템 이미지
    protected Text m_ItemCount = null;                   // 아이템 개수

    private void Awake()
    {
        this.Initialize();
    }

    // 초기화
    protected virtual void Initialize()
    {
        m_ItemImage = transform.GetChild(0).GetComponent<Image>();
        m_ItemCount = m_ItemImage.GetComponentInChildren<Text>();

        m_ItemImage.enabled = false;
        m_ItemCount.enabled = false;
    }

    // 슬롯 변경 내용 있을 때마다 호출하여 UI 업데이트
    public void UpdateUI(Slot p_slot)
    {
        // 슬롯이 비었을 때
        if (IsSlotEmpty(p_slot))
        {
            p_slot.m_ItemImage.sprite = null;
            p_slot.m_ItemImage.enabled = false;

            p_slot.m_ItemCount.text = p_slot.m_ItemStack.Count.ToString();
            p_slot.m_ItemCount.enabled = false;
            return;
        }

        // 슬롯에 아이템이 있을 때
        Item currItem = p_slot.m_ItemStack.Peek();

        p_slot.m_ItemImage.enabled = true;
        p_slot.m_ItemImage.sprite = currItem.m_ItemSprite;
        // 쌓을 수 있는 아이템만 개수 표시
        if (currItem.m_IsStackable) p_slot.m_ItemCount.enabled = true;
        p_slot.m_ItemCount.text = p_slot.m_ItemStack.Count.ToString();
    }

    // 슬롯에 쌓인 아이템 개수 가져가는 용도
    public int GetItemStackCount(Slot p_slot)
    {
        return p_slot.m_ItemStack.Count;
    }

    // 슬롯에 있는 아이템 정보 가져가는 용도
    public Item GetItemInfo(Slot p_slot)
    {
        return IsSlotEmpty(p_slot) ? null : p_slot.m_ItemStack.Peek();
    }

    // 슬롯에 있는 아이템 스택이 가득 찼는지 확인
    public bool IsItemStackFull(Slot p_slot)
    {
        Item tempItem = p_slot.m_ItemStack.Peek();
        if (tempItem.m_IsStackable && p_slot.m_ItemStack.Count == MaxItemStack.Stackable) return true;
        else if(!tempItem.m_IsStackable && p_slot.m_ItemStack.Count == MaxItemStack.NonStackable) return true;
        return false;
    }

    // 슬롯이 비어 있는지 확인
    public bool IsSlotEmpty(Slot p_slot)
    {
        if (p_slot.m_ItemStack.Count == 0) return true;
        return false;
    }

    /*
     * 슬롯 마우스 클릭 시 호출 되는 함수
     * 
     * 1. 드래그 중이 아닐 때 빈 슬롯 클릭 :: 아무것도 하지 않음
     * 2. 드래그 중에 빈 슬롯 클릭
     * 2-1. 슬롯 좌 클릭 :: 드래그 중인 아이템 개수 만큼 슬롯에 드랍
     * 2-2. 슬롯 우 클릭 :: 드래그 중인 아이템 슬롯에 1개 드랍
     * 
     * 3. 드래그 중이 아닐 때 아이템이 있는 슬롯 클릭
     * 3-1. 슬롯 좌 클릭 :: 슬롯에 있는 모든 아이템 드래그
     * 3-2. 슬롯 우 클릭 :: 슬롯에 있는 아이템 절반(올림) 드래그
     * 
     * 4. 드래그 중에 아이템이 있는 슬롯 클릭
     * 4-1. 슬롯에 있는 아이템과 드래그 중인 아이템이 다른 경우 :: 아이템 스왑
     * 4-2. 아이템이 같은 경우 슬롯 좌 클릭 :: 슬롯의 아이템 최대 개수 만큼 채우고 나머지 드래그
     * 4-3. 아이템이 같은 경우 슬롯 우 클릭 :: 슬롯의 아이템이 최대 개수가 아니면 1개 채우기
     * 
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager eventManager = EventManager.GetInstance;
        switch(this.IsSlotEmpty(this))
        {
            // 빈 슬롯 클릭
            case true:
                switch(eventManager.m_IsDragging)
                {
                    // 아이템 드래그중 :: 아이템 드랍
                    case true:
                        ItemsDrop(eventData, eventManager);
                        break;

                    // 아이템 드래그중이 아님 :: return
                    case false:
                        return;
                }
                break;

            // 아이템이 있는 슬롯 클릭
            case false:
                switch (eventManager.m_IsDragging)
                {
                    // 드래그 중 :: 슬롯에 있는 아이템과 드래그 중인 아이템 정보 스왑, 드랍(아이템이 같은 경우)
                    case true:
                        ItemsSwapOrDrop(eventData, eventManager);
                        break;

                    // 드래그 중이 아님 :: 슬롯 아이템 드래그
                    case false:
                        ItemsDrag(eventData, eventManager);    // 아이템 드래그 :: 함수 내 에서 좌,우 클릭 분리
                        return;
                }
                break;
        }
    }

    // 아이템 드래그
    private void ItemsDrag(PointerEventData p_eventData, EventManager p_eventManager)
    {
        DraggingItem draggingItem = p_eventManager.m_DraggingItem.GetComponent<DraggingItem>();
        int PickupItemStackSize = 0;

        switch (p_eventData.pointerId)
        {
            // 좌 클릭 :: 슬롯에 있는 모든 아이템 드래그
            case -1:
                PickupItemStackSize = this.m_ItemStack.Count;
                break;

            // 우 클릭 :: 슬롯에 있는 아이템 절반 드래그
            case -2:
                PickupItemStackSize = Mathf.CeilToInt(this.m_ItemStack.Count * 0.5f);
                break;
        }
        p_eventManager.m_IsDragging = true;

        for (int i = 0; i < PickupItemStackSize; i++)
            draggingItem.m_ItemStack.Push(this.m_ItemStack.Pop());

        this.UpdateUI(draggingItem);
        this.UpdateUI(this);
    }

    // 아이템 슬롯에 드랍
    private void ItemsDrop(PointerEventData p_eventData, EventManager p_eventManager)
    {
        DraggingItem draggingItem = p_eventManager.m_DraggingItem.GetComponent<DraggingItem>();
        int DropItemStackSize = 0;

        switch (p_eventData.pointerId)
        {
            // 좌 클릭 :: 슬롯에 드래그 중인 아이템 모두 드랍
            case -1:
                DropItemStackSize = draggingItem.m_ItemStack.Count;
                break;

            // 우 클릭 :: 슬롯에 아이템 1개 드랍
            case -2:
                DropItemStackSize = 1;
                break;
        }

        for (int i = 0; i < DropItemStackSize; i++)
            this.m_ItemStack.Push(draggingItem.m_ItemStack.Pop());

        this.UpdateUI(draggingItem);
        this.UpdateUI(this);

        if (draggingItem.m_ItemStack.Count == 0)
            p_eventManager.m_IsDragging = false;
    }

    // 아이템 스왑 , 드랍
    private void ItemsSwapOrDrop(PointerEventData p_eventData, EventManager p_eventManager)
    {
        DraggingItem draggingItem = p_eventManager.m_DraggingItem.GetComponent<DraggingItem>();

    }
}
