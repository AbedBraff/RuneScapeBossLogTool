﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  View for BossDisplayWidget UI element
public class BossLogListDisplay : BossLogDictionaryDisplay, IDisplayable<BossLogList>
{
    [SerializeField] private Text bossText;
    [SerializeField] private Text killsText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text lootText;
    [SerializeField] private Text killsPerHourText;
    [SerializeField] private Text lootPerKillText;
    [SerializeField] private Text lootPerHourText;
    [SerializeField] private RareItemScrollListController rareListController;

    public void Display(in BossLogList value)
    {
        rareListController.LoadNewSprites(value.bossName);

        LogDataStruct data = value.GetBossTotalsData();
        double hours = data.time / 3600d;

        base.DisplayBossText(in bossText, value.bossName);
        base.DisplayKillsText(in killsText, data.kills);
        base.DisplayTimeText(in timeText, in hours);
        base.DisplayLootText(in lootText, data.loot);
        base.DisplayKillsPerHourText(in killsPerHourText, data.kills, in hours);
        base.DisplayLootPerKillText(in lootPerKillText, data.loot, data.kills);
        base.DisplayLootPerHourText(in lootPerHourText, data.loot, in hours);

        rareListController.Display(value.GetRareItemList());
    }
}