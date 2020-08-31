﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.Events;
using GoogleSheetsToUnity;
using UnityEngine.EventSystems;
using System.Runtime.Serialization;
using System.Threading.Tasks;

public class ProgramControl : MonoBehaviour
{
    public static ProgramControl Instance;
    public static OptionController Options;

    [SerializeField]
    private GameObject uiController;
    private bool wantsToQuit = false;

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

        //  Subscribe to events
        EventManager.Instance.onRSVersionChanged += SetDataAndPanels;
        Application.wantsToQuit += QuitCheck;

        //  Perform setup
        Setup();
    }

    private void Setup()
    {
        uiController.SetActive(true);

        //ProgramState.CurrentState = ProgramState.states.Loading;

        //  Set up our options
        Options = new OptionController(new Options(), UIController.Instance.GetOptionUIScript());
        Task t =  Options.Setup();
    }

    public void LateSetup()
    {
        UIController.Instance.LateSetup();
        EventManager.Instance.BossDropdownValueChanged();
    }

    public void SetDataAndPanels()
    {
        ProgramState.CurrentState = ProgramState.states.Loading;
        UIController.Instance.ResetPanels();
        DataController.Instance.Setup();

        LateSetup();
    }

    //  UI exit menu button calls this
    public void CloseProgram()
    {
        if (QuitCheck())
            Application.Quit();
    }

    //  Prompt the user to confirm they want to exit the application
    private bool QuitCheck()
    {
        if (wantsToQuit)
            return true;

        //  Check if there are unsaved changes
        if (DataController.Instance.bossLogsDictionary.hasUnsavedData)
        {
            string message = $"There is unsaved data!\nAre you sure you want to exit?";
            ConfirmWindow.Instance.NewConfirmWindow(in message, OnExitResponse);
        }
        else
            return true;

        return false;
    }

    //  User confirm/deny callback function
    private void OnExitResponse(UserResponse userResponse, string data)
    {
        if (userResponse.userResponse)
        {
            wantsToQuit = true;
            Application.Quit();
        }
    }

    public void Update()
    {
        //  Ctrl + S to save
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            DataController.Instance.SaveBossLogData();
        else if (Input.GetKeyDown(KeyCode.F12))
            Options.OpenOptionWindow();

        if (Timer.IsRunning)
            Timer.UpdateTime();

        //Debug.Log(DataState.CurrentState);
    }
}

//  Program states for each tab
public class ProgramState
{
    public enum states { None, Loading, Drops, Logs, Setup, Exit };

    private static states currentState;

    public static states CurrentState
    {
        get { return currentState; }
        set
        {
            //  Program is currently in setup
            if(currentState == states.Loading)
            {
                //  New state is not setup
                if(value != states.Loading)
                {
                    currentState = value;
                    UIController.Instance.InputRestrictEnd();
                }
            }
            //  Program is being set to setup
            else if(value == states.Loading)
            {
                //  Turn on input restrict UI
                currentState = value;
                UIController.Instance.InputRestrictStart($"{value.ToString()}...");
            }
            else
            {
                currentState = value;
            }

            Debug.Log($"New program state is {value.ToString()}");
        }
    }
}
