﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//  View for an individual EquipmentSlot
public class EquipmentSlotView : AbsSetupItemSlotView, IDisplayable<SetupItem>, IPointerClickHandler
{
    public int equipmentSlotNumber { private get; set; }

    [SerializeField] private Image itemImage;
    [SerializeField] private ItemSlotCategories itemSlotCategory;
    //  Default image is the equipment slot's placeholder for when it has no item
    private Sprite defaultImage;

    public void Init(int equipmentSlotNumber)
    {
        slotCategory = itemSlotCategory;
        defaultImage = itemImage.sprite;
        item = null;
        this.equipmentSlotNumber = equipmentSlotNumber;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        base.OnClick(in eventData, equipmentSlotNumber);
    }

    public void Display(in SetupItem setupItem)
    {
        base.Display(in setupItem, in itemImage);
    }
}