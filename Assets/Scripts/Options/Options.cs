﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//  Our collection of options data
public class Options
{
    //  Dictionary of options using the option name as a key
    private Dictionary<string, GenericOption> optionDictionary;

    private static string OPTIONFILEPATH;

    public Options()
    {
        optionDictionary = new Dictionary<string, GenericOption>();
    }

    ~Options()
    {
        EventManager.Instance.onOptionUISetup -= DisplayCurrentSelectedOptions;
    }

    public void Setup()
    {
        if (Application.isEditor)
            OPTIONFILEPATH = Application.persistentDataPath + "/testOptions.ini";
        else
            OPTIONFILEPATH = Application.persistentDataPath + "/options.ini";

        EventManager.Instance.onOptionUISetup += DisplayCurrentSelectedOptions;

        CreateOptions();
        FillOptions();
        LoadOptions();
    }

    //  Use our factory pattern to create objects from our OptionData.cs file
    private void CreateOptions()
    {
        OptionFactory optionFactory = new OptionFactory();
        for (int i = 0; i < OptionData.GetOptions().Length; ++i)
        {
            string name = OptionData.GetOptions()[i];

            //  Make sure we don't already have an option with the same name
            if (optionDictionary.ContainsKey(name))
                throw new System.Exception($"There is already a {name} option!\nOptions.cs::CreateOptions()");

            GenericOption op = optionFactory.GetOption(in name);
            AddOption(in op);
        }
    }

    //  Load our saved option values from file
    private void LoadOptions()
    {
        string fileLine;

        //  Check Option file exists
        if (File.Exists(OPTIONFILEPATH))
        {
            using (StreamReader sr = new StreamReader(OPTIONFILEPATH))
            {
                //  Parse each line in the file
                while((fileLine = sr.ReadLine()) != null)
                {
                    //  Should be format of [OptionName]=[OptionValue]
                    string[] values = fileLine.Split('=');
                    if(values.Length != 2)
                    {
                        Debug.Log($"Improper format for option detected!");
                        continue;
                    }
                    //  Make sure [OptionName] matches one of our listed options in our data
                    if(!OptionData.IsOption(in values[0]))
                    {
                        Debug.Log($"{values[0]} is not a listed option.  If it should be, make sure it is added to OptionData.cs.");
                        continue;
                    }

                    //  Get our option from data
                    GenericOption genOp = TryGetOption(in values[0]);

                    //  Make sure [OptionValue] is a valid selection for that option
                    if (genOp.IsValidChoice(in values[1]))
                    {
                        //  Now we can set the value
                        Debug.Log($"{genOp.GetValue()} is a valid choice for {genOp.GetName()} option");
                        genOp.SetValue(in values[1]);
                    }
                    else
                    {
                        Debug.Log("invalid choice");
                    }
                }
            }
        }

        //  Re-Save our options to file to fix any errors or improper data detected on load
        SaveOptions();
        ApplyOptionUpdates();
    }

    public void SaveOptions()
    {
        //  Write stuff
        using (StreamWriter sw = new StreamWriter(OPTIONFILEPATH))
        {
            foreach (var option in optionDictionary.Values)
                sw.WriteLine(option.ToString());
        }
    }

    //  Wrapper for Dictionary.add
    public void AddOption(in GenericOption option)
    {
        optionDictionary.Add(option.GetName(), option);
    }

    //  Wrapper for Dictionary.TryGetValue
    public GenericOption TryGetOption(in string name)
    {
        GenericOption go;
        optionDictionary.TryGetValue(name, out go);
        return go;
    }

    //  Get option value by name
    public string GetOptionValue(in string name)
    {
        GenericOption go = TryGetOption(in name);

        if (go == null)
            throw new System.Exception($"No {name} option found!\nOption.cs::GetOptionValue()");

        return go.GetValue();
    }

    //  Fill UI objects with each option's valid choices
    private void FillOptions()
    {
        foreach(GenericOption option in optionDictionary.Values)
        {
            option.PopulateChoices();
        }
    }

    //  Fill UI objects with each option's currently selected value
    private void DisplayCurrentSelectedOptions()
    {
        foreach(GenericOption option in optionDictionary.Values)
        {
            option.DisplayChoice();
        }
    }

    //  Apply any updates needed when an option changes value
    //  ie resolution change
    private void ApplyOptionUpdates()
    {
        foreach(GenericOption option in optionDictionary.Values)
        {
            option.Apply();
        }
    }

    //  Get a list of our option data
    public List<GenericOption> GetOptionList()
    {
        List<GenericOption> temp = new List<GenericOption>();

        foreach(GenericOption option in optionDictionary.Values)
        {
            temp.Add(option);
        }

        return temp;
    }
}