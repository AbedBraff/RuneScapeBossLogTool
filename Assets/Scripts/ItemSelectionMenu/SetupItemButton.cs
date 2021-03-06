﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//  Button created for SetupItems in the SetupItemMenu
public class SetupItemButton : MonoBehaviour, IPointerClickHandler
{
    private Button thisButton;
    private SetupItemStruct setupItemStruct;

    private void Awake()
    {
        if (!(thisButton = GetComponent<Button>()))
            throw new System.Exception($"ItemCategoryButton.cs is not attached to a button gameobject!");
    }

    //  Set item category and button text
    public void Setup(in SetupItemStruct setupItemStruct)
    {
        this.setupItemStruct = setupItemStruct;
        GetComponentInChildren<Text>().text = setupItemStruct.itemName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetupItemMenuController menu = gameObject.GetComponentInParent<SetupItemMenuController>();

        SetupItem setupItem;

        if (SetupItemsDictionary.TryGetItem(in setupItemStruct.itemID, out setupItem))
        {
            //  If the ItemSlot was in the inventory, open the window asking how many to add, otherwise add 1
            if(menu.ItemSlotCategory == ItemSlotCategories.Inventory || menu.ItemSlotCategory == ItemSlotCategories.Scroll)
                AddQuantityWindow.Instance.OpenWindow(new AddedItemData(setupItem, menu.ItemSlotCategory, menu.ClickedSlotID), ProgramState.CurrentState, menu.CollectionType);
            else
                CacheManager.SetupTab.Setup.AddQuantityOfSetupItem(in setupItem, 1, menu.CollectionType, menu.ItemSlotCategory, menu.ClickedSlotID);
        }

        //  Close the menu
        menu.OnPointerExit(new PointerEventData(EventSystem.current));
    }
}
