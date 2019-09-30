﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBossLogData
{
    public SingleBossLogData()
    {

    }
    public SingleBossLogData(string LogName, string BossName)
    {
        m_LogName = LogName;
        m_BossName = BossName;
        m_Kills = 0;
        m_LootValue = 0;
        m_TimeSpent = 0;
    }


    private string m_LogName, m_BossName;
    private float m_Kills, m_LootValue, m_TimeSpent;


    //  Properties
    public string LogName { get { return m_LogName; } }
    public string BossName { get { return m_BossName; } }
    public float Kills { get { return m_Kills; } set { m_Kills += value; } }
    public float LootValue { get { return m_LootValue; } set { m_LootValue += value; } }
    public float TimeSpent { get { return m_TimeSpent; } set { m_TimeSpent += value; } }


    private float AverageValuePerKill()
    {
        if(m_Kills != 0)
            return m_LootValue / m_Kills;

        return 0f;
    }


    private float AverageKillsPerHour()
    {
        if(m_TimeSpent != 0)
            return m_Kills / (m_TimeSpent / 60f);

        return 0f;
    }


    private float AverageValuePerHour()
    {
        if(m_TimeSpent != 0)
            return m_LootValue / (m_TimeSpent / 60f);

        return 0f;
    }
}
