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
 *  ���� ���� ��ũ��Ʈ 
 */
public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Stack<Item> m_ItemStack = new Stack<Item>();  // ���Կ� �ִ� ������ ����
    protected Image m_ItemImage = null;                  // ������ �̹���
    protected Text m_ItemCount = null;                   // ������ ����

    private void Awake()
    {
        this.Initialize();
    }

    // �ʱ�ȭ
    protected virtual void Initialize()
    {
        m_ItemImage = transform.GetChild(0).GetComponent<Image>();
        m_ItemCount = m_ItemImage.GetComponentInChildren<Text>();

        m_ItemImage.enabled = false;
        m_ItemCount.enabled = false;
    }

    // ���� ���� ���� ���� ������ ȣ���Ͽ� UI ������Ʈ
    public void UpdateUI(Slot p_slot)
    {
        // ������ ����� ��
        if (IsSlotEmpty(p_slot))
        {
            p_slot.m_ItemImage.sprite = null;
            p_slot.m_ItemImage.enabled = false;

            p_slot.m_ItemCount.text = p_slot.m_ItemStack.Count.ToString();
            p_slot.m_ItemCount.enabled = false;
            return;
        }

        // ���Կ� �������� ���� ��
        Item currItem = p_slot.m_ItemStack.Peek();

        p_slot.m_ItemImage.enabled = true;
        p_slot.m_ItemImage.sprite = currItem.m_ItemSprite;
        // ���� �� �ִ� �����۸� ���� ǥ��
        p_slot.m_ItemCount.enabled = currItem.m_IsStackable ? true : false;
        p_slot.m_ItemCount.text = p_slot.m_ItemStack.Count.ToString();
    }

    // ���Կ� ���� ������ ���� �������� �뵵
    public int GetItemStackCount(Slot p_slot)
    {
        return p_slot.m_ItemStack.Count;
    }

    // ���Կ� �ִ� �������� �ִ� ���� �������� �뵵
    public int GetMaxItemStackInSlot(Slot p_slot)
    {
        return p_slot.GetItemInfo(p_slot).m_IsStackable ? MaxItemStack.Stackable : MaxItemStack.NonStackable;
    }

    // ���Կ� �ִ� ������ ���� �������� �뵵
    public Item GetItemInfo(Slot p_slot)
    {
        return IsSlotEmpty(p_slot) ? null : p_slot.m_ItemStack.Peek();
    }

    // ���Կ� �ִ� ������ ������ ���� á���� Ȯ��
    public bool IsItemStackFull(Slot p_slot)
    {
        Item tempItem = p_slot.m_ItemStack.Peek();
        if (tempItem.m_IsStackable && p_slot.m_ItemStack.Count == MaxItemStack.Stackable) return true;
        else if(!tempItem.m_IsStackable && p_slot.m_ItemStack.Count == MaxItemStack.NonStackable) return true;
        return false;
    }

    // ������ ��� �ִ��� Ȯ��
    public bool IsSlotEmpty(Slot p_slot)
    {
        if (p_slot.m_ItemStack.Count == 0) return true;
        return false;
    }

    /*
     * ���� ���콺 Ŭ�� �� ȣ�� �Ǵ� �Լ�
     * 
     * 1. �巡�� ���� �ƴ� �� �� ���� Ŭ�� :: �ƹ��͵� ���� ����
     * 2. �巡�� �߿� �� ���� Ŭ��
     * 2-1. ���� �� Ŭ�� :: �巡�� ���� ������ ���� ��ŭ ���Կ� ���
     * 2-2. ���� �� Ŭ�� :: �巡�� ���� ������ ���Կ� 1�� ���
     * 
     * 3. �巡�� ���� �ƴ� �� �������� �ִ� ���� Ŭ��
     * 3-1. ���� �� Ŭ�� :: ���Կ� �ִ� ��� ������ �巡��
     * 3-2. ���� �� Ŭ�� :: ���Կ� �ִ� ������ ����(�ø�) �巡��
     * 
     * 4. �巡�� �߿� �������� �ִ� ���� Ŭ��
     * 4-1. ���Կ� �ִ� �����۰� �巡�� ���� �������� �ٸ� ��� :: ������ ����
     * 4-2. �������� ���� ��� ���� �� Ŭ�� :: ������ ������ �ִ� ���� ��ŭ ��� ������ �巡��
     * 4-3. �������� ���� ��� ���� �� Ŭ�� :: ������ �������� �ִ� ������ �ƴϸ� 1�� ���
     * 
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager eventManager = EventManager.GetInstance;
        switch(this.IsSlotEmpty(this))
        {
            // �� ���� Ŭ��
            case true:
                switch(eventManager.m_IsDragging)
                {
                    // ������ �巡���� :: ������ ���
                    case true:
                        ItemsDrop(eventData, eventManager);
                        break;

                    // ������ �巡������ �ƴ� :: return
                    case false:
                        return;
                }
                break;

            // �������� �ִ� ���� Ŭ��
            case false:
                switch (eventManager.m_IsDragging)
                {
                    // �巡�� �� :: ���Կ� �ִ� �����۰� �巡�� ���� ������ ���� ����, ���(�������� ���� ���)
                    case true:
                        ItemsSwapOrDrop(eventData, eventManager);
                        break;

                    // �巡�� ���� �ƴ� :: ���� ������ �巡��
                    case false:
                        ItemsDrag(eventData, eventManager);    // ������ �巡�� :: �Լ� �� ���� ��,�� Ŭ�� �и�
                        return;
                }
                break;
        }
    }

    // ������ �巡��
    private void ItemsDrag(PointerEventData p_eventData, EventManager p_eventManager)
    {
        DraggingItem draggingItem = p_eventManager.m_DraggingItem.GetComponent<DraggingItem>();
        int PickupItemStackSize = 0;

        switch (p_eventData.pointerId)
        {
            // �� Ŭ�� :: ���Կ� �ִ� ��� ������ �巡��
            case -1:
                PickupItemStackSize = this.m_ItemStack.Count;
                break;

            // �� Ŭ�� :: ���Կ� �ִ� ������ ���� �巡��
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

    // ������ ���Կ� ���
    private void ItemsDrop(PointerEventData p_eventData, EventManager p_eventManager)
    {
        DraggingItem draggingItem = p_eventManager.m_DraggingItem.GetComponent<DraggingItem>();
        int DropItemStackSize = 0;

        switch (p_eventData.pointerId)
        {
            /*
             * �� Ŭ��
             * 1. �� ���� :: �巡�� ���� ������ ��� ���
             * 2. ������ �ִ� ���� :: ������ ������ �ִ� ������ŭ ���
             */
            case -1:
                if (this.IsSlotEmpty(this))
                {
                    DropItemStackSize = draggingItem.m_ItemStack.Count;
                }
                else
                {
                    int draggingItemCount = GetItemStackCount(draggingItem);
                    int requiredItemCount = this.GetMaxItemStackInSlot(this) - this.GetItemStackCount(this);
                    
                    if (draggingItemCount >= requiredItemCount)
                        DropItemStackSize = requiredItemCount;
                    else
                        DropItemStackSize = draggingItemCount;
                }
                break;

            // �� Ŭ�� :: ���Կ� ������ 1�� ���
            case -2:
                if(this.IsSlotEmpty(this) ||
                   this.GetItemStackCount(this) < this.GetMaxItemStackInSlot(this))
                {
                    DropItemStackSize = 1;
                }       
                break;
        }

        for (int i = 0; i < DropItemStackSize; i++)
            this.m_ItemStack.Push(draggingItem.m_ItemStack.Pop());

        this.UpdateUI(draggingItem);
        this.UpdateUI(this);

        if (draggingItem.m_ItemStack.Count == 0)
            p_eventManager.m_IsDragging = false;
    }

    // ������ ���� , ���
    private void ItemsSwapOrDrop(PointerEventData p_eventData, EventManager p_eventManager)
    {
        DraggingItem draggingItem = p_eventManager.m_DraggingItem.GetComponent<DraggingItem>();

        // �巡�� �����۰� ���� ������ ���� ��
        switch(GetItemInfo(draggingItem) == GetItemInfo(this))
        {
            // ���� ������ :: ������ ���
            case true:
                ItemsDrop(p_eventData, p_eventManager);
                break;

            // �ٸ� ������ :: �巡�� �����۰� ���� ������ ������ ����
            case false:
                Core.Swap<Stack<Item>>(ref draggingItem.m_ItemStack, ref this.m_ItemStack);
                this.UpdateUI(draggingItem);
                this.UpdateUI(this);
                break;
        }
    }
}
