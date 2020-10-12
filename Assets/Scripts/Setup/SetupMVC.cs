﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//  MVC for the Setup
public class SetupMVC
{
    public SetupMVC(SetupView view)
    {
        model = new Setup();
        this.view = view;

        //  Sub to events
        EventManager.Instance.onNewUsernameEntered += LoadNewPlayerStatsAsync;
    }
    ~SetupMVC()
    {
        //  Unsub to events
        EventManager.Instance.onNewUsernameEntered -= LoadNewPlayerStatsAsync;
    }

    public Player Player { get { return model.player; } }
    public float DegradePerHour { get { return model.DegradePerHour; } }
    public float ChargeDrainPerHour { get { return model.ChargeDrainPerHour; } }
    public float ChargeDrainRate { get { return model.ChargeDrainRate; } }

    private Setup model;
    private SetupView view;

    //  Load stats of a player from username asynchronously
    public async void LoadNewPlayerStatsAsync(string username)
    {
        model.player = await SetupLoader.LoadPlayerStatsAsync(username);
        view.Display(Player);
    }

    public void AddSetupItem(in SetupItem setupItem, in ItemSlotCategories itemSlotCategory, in int index)
    {
        switch (itemSlotCategory)
        {
            case ItemSlotCategories.Inventory:
                Player.Inventory.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Head:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Pocket:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Cape:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Necklace:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Ammunition:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Mainhand:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Body:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Offhand:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Legs:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Gloves:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Boots:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            case ItemSlotCategories.Ring:
                Player.Equipment.SetItemAtIndex(in setupItem, index);
                break;
            default:
                Debug.Log($"{itemSlotCategory.ToString()} could not be added to!");
                break;
        }

        //  Update price UI
        view.DisplaySetupCost(model.TotalCost);
    }

    public void SetCombatIntensity(in CombatIntensity.CombatIntensityLevels intensityLevel)
    {
        model.SetCombatIntensity(in intensityLevel);
        view.DisplaySetupCost(model.TotalCost);
    }

    public void SetChargeDrainRate(in float chargeDrainRate)
    {
        model.SetChargeDrainRate(in chargeDrainRate);
        view.DisplaySetupCost(model.TotalCost);
    }
}
