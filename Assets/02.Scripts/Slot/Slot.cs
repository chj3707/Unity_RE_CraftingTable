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

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Stack<Item> m_ItemStack = new Stack<Item>();  // 슬롯에 있는 아이템 정보
    public Image m_ItemImage = null;                     // 아이템 이미지
    public Text m_ItemCount = null;                      // 아이템 개수

    private void Awake()
    {
        m_ItemImage = transform.GetChild(0).GetComponent<Image>();
        m_ItemCount = m_ItemImage.GetComponentInChildren<Text>();
        
        m_ItemImage.enabled = false;
        m_ItemCount.enabled = false;
    }

    // 슬롯 변경 내용 있을 때마다 호출하여 UI 업데이트
    public void UpdateUI()
    {
        // 슬롯이 비었을 때
        if (this.IsSlotEmpty())
        {
            m_ItemImage.sprite = null;
            m_ItemImage.enabled = false;

            m_ItemCount.text = m_ItemStack.Count.ToString();
            m_ItemCount.enabled = false;
            return;
        }

        // 슬롯에 아이템이 있을 때
        Item currItem = m_ItemStack.Peek();

        m_ItemImage.enabled = true;
        m_ItemImage.sprite = currItem.m_ItemSprite;
        // 쌓을 수 있는 아이템만 개수 표시
        if (currItem.m_IsStackable) m_ItemCount.enabled = true;
        m_ItemCount.text = m_ItemStack.Count.ToString();
    }

    // 슬롯 정보 초기화
    public void ResetSlot()
    {
        this.m_ItemStack.Clear();
        this.UpdateUI();
    }

    // 슬롯에 쌓인 아이템 개수 가져가는 용도
    public int GetItemStackCount()
    {
        return m_ItemStack.Count;
    }

    // 슬롯에 있는 아이템 정보 가져가는 용도
    public Item GetItemInfo()
    {
        return this.IsSlotEmpty() ? null : m_ItemStack.Peek();
    }

    // 슬롯에 있는 아이템 스택이 가득 찼는지 확인
    public bool IsItemStackFull()
    {
        Item tempItem = m_ItemStack.Peek();
        if (tempItem.m_IsStackable && m_ItemStack.Count == MaxItemStack.Stackable) return true;
        else if(!tempItem.m_IsStackable && m_ItemStack.Count == MaxItemStack.NonStackable) return true;
        return false;
    }

    // 슬롯이 비어 있는지 확인
    public bool IsSlotEmpty()
    {
        if (m_ItemStack.Count == 0) return true;
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
     * 3-1. 슬롯 좌 클릭 :: 슬롯에 있는 모든 아이템 들기
     * 3-2. 슬롯 우 클릭 :: 슬롯에 있는 아이템 절반(올림) 들기
     * 
     * 4. 드래그 중에 아이템이 있는 슬롯 클릭
     * 4-1. 슬롯에 있는 아이템과 드래그 중인 아이템이 다른 경우 :: 아이템 스왑
     * 4-2. 아이템이 같은 경우 슬롯 좌 클릭 :: 슬롯의 아이템 최대 개수 만큼 채우고 나머지 들기
     * 4-3. 아이템이 같은 경우 슬롯 우 클릭 :: 슬롯의 아이템이 최대 개수가 아니면 1개 채우기
     * 
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager eventManager = EventManager.GetInstance;
        switch(this.IsSlotEmpty())
        {
            // 빈 슬롯 클릭
            case true:
                switch(eventManager.m_IsDragging)
                {
                    // 아이템 드래그중 :: 아이템 드랍
                    case true:
                        DropTheItem(eventData, eventManager);
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
                    // 드래그 중 :: 슬롯에 있는 아이템과 드래그 중인 아이템 정보 스왑 또는 드랍(아이템이 같은 경우)
                    case true:
                        break;

                    // 드래그 중이 아님 :: 슬롯 아이템 드래그
                    case false:
                        PickupItem(eventData, eventManager);    // 아이템 들기 :: 함수 내 에서 좌,우 클릭 분리
                        return;
                }
                break;
        }
    }

    // 슬롯에 있는 아이템 드래그
    private void PickupItem(PointerEventData p_eventData, EventManager p_eventManager)
    {
        DraggingItem draggingItem = p_eventManager.m_DraggingItem.GetComponent<DraggingItem>();
        int PickupItemStackSize = 0;

        switch (p_eventData.pointerId)
        {
            // 좌 클릭 :: 슬롯에 있는 모든 아이템 들기
            case -1:
                PickupItemStackSize = this.m_ItemStack.Count;
                break;

            // 우 클릭 :: 슬롯에 있는 아이템 절반 들기
            case -2:
                PickupItemStackSize = Mathf.CeilToInt(this.m_ItemStack.Count * 0.5f);
                break;
        }
        p_eventManager.m_IsDragging = true;

        for (int i = 0; i < PickupItemStackSize; i++)
            draggingItem.m_ItemStack.Push(this.m_ItemStack.Pop());

        draggingItem.UpdateUI();
        this.UpdateUI();
    }

    // 드래그 중인 아이템 슬롯에 드랍
    private void DropTheItem(PointerEventData p_eventData, EventManager p_eventManager)
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

        draggingItem.UpdateUI();
        this.UpdateUI();

        if (draggingItem.m_ItemStack.Count == 0)
            p_eventManager.m_IsDragging = false;
    }
}
