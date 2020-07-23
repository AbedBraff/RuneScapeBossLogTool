﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//  Handle UI control
public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private GameObject dropsPanel;
    [SerializeField] private GameObject logsPanel;
    [SerializeField] private GameObject setupPanel;
    [SerializeField] private Button toolbarDropsButton;
    [SerializeField] private Button toolbarLogsButton;
    [SerializeField] private Button toolbarSetupButton;
    [SerializeField] private GameObject inputRestrictPanel;
    [SerializeField] Sprite[] loadSprites;
    [SerializeField] private GameObject optionWindow;
    [SerializeField] private Transform bossTotalsWidgetLoc;
    [SerializeField] private Transform logTotalsWidgetLoc;
    [SerializeField] private Dropdown logsTab_BossDropdown;
    [SerializeField] private Dropdown logsTab_LogDropdown;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        ResetPanels();
        optionWindow.SetActive(true);

        //  Setup the permanent display UI for our Logs tab
        BossDropdownDisplayLink bossDropdownDisplayLink = logsTab_BossDropdown.GetComponent<BossDropdownDisplayLink>();
        LogDropdownDisplayLink logDropdownDisplayLink = logsTab_LogDropdown.GetComponent<LogDropdownDisplayLink>();

        //  Create widgets
        bossDropdownDisplayLink.LinkAndCreateWidget(bossTotalsWidgetLoc.gameObject);
        logDropdownDisplayLink.LinkAndCreateWidget(logTotalsWidgetLoc.gameObject);

        //  Setup the LogDropdownDisplay to update its bossName when the BossDropdown value in the Logs tab is changed
        bossDropdownDisplayLink.AddValueChangedAction(logDropdownDisplayLink.SetBoss);
    }

    public void ResetPanels()
    {
        dropsPanel.SetActive(false);
        logsPanel.SetActive(false);
        setupPanel.SetActive(false);

        dropsPanel.SetActive(true);
        logsPanel.SetActive(true);
        setupPanel.SetActive(true);
    }

    public void LateSetup()
    {
        optionWindow.SetActive(false);
        logsPanel.SetActive(false);
        setupPanel.SetActive(false);
    }

    //  Fetches our OptionUI script for our OptionController
    public OptionUI GetOptionUIScript()
    {
        OptionUI script = optionWindow.GetComponentInChildren<OptionUI>();

        if (!script)
            throw new System.Exception("You forgot to add the OptionUI.cs script to the option window!");

        return script;
    }

    //  Open UI to prevent input during setup/loading/saving
    public void InputRestrictStart(in string _text)
    {
        Image img = inputRestrictPanel.GetComponent<Image>();

        //  Image to use when application is doing initial setup or switching between RSVersions
        if (ProgramState.CurrentState == ProgramState.states.Loading)
        {
            img.sprite = loadSprites[Random.Range(0, loadSprites.Length - 1)];
            img.color = Color.white;
        }
        //  Image to use when doing any other saving or loading
        else
        {
            img.sprite = null;
            Color clr = Color.white;
            clr.a = (100f / 255f);
            img.color = clr;
        }
        
        inputRestrictPanel.SetActive(true);

        Text t = inputRestrictPanel.GetComponentInChildren<Text>();
        t.text = _text;
    }

    //  Close above UI
    public void InputRestrictEnd()
    {
        inputRestrictPanel.SetActive(false);
    }

    public void OnToolbarDropButtonClicked()
    {
        //  Set proper panel active states
        dropsPanel.SetActive(true);
        logsPanel.SetActive(false);
        setupPanel.SetActive(false);

        //  Set app state
        ProgramState.CurrentState = ProgramState.states.Drops;

        //DataController.Instance.CurrentBoss = drops_BossDropdown.options[drops_BossDropdown.value].text;

        //EventManager.Instance.TabSwitched(DataController.Instance.CurrentDropTabLog);
    }

    public void OnToolbarLogButtonClicked()
    {
        //  Set proper panel active states
        dropsPanel.SetActive(false);
        logsPanel.SetActive(true);
        setupPanel.SetActive(false);

        //  Set app state
        ProgramState.CurrentState = ProgramState.states.Logs;

        //DataController.Instance.CurrentBoss = logs_BossDropdown.options[logs_BossDropdown.value].text;

        //EventManager.Instance.TabSwitched(DataController.Instance.CurrentLogTabLog);
    }


    public void OnToolbarSetupButtonClicked()
    {
        //  Set proper panel active states
        dropsPanel.SetActive(false);
        logsPanel.SetActive(false);
        setupPanel.SetActive(true);

        //  Set app state
        ProgramState.CurrentState = ProgramState.states.Setup;

        //EventManager.Instance.TabSwitched("");
    }
}
