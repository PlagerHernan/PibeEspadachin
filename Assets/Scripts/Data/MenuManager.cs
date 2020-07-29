using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Manager
{
    override protected void Awake()
    {
        base.Awake();
        LoadSettingsInfo();
    }

    private void Start() 
    {
        _gameSettings = _saveSystem.GetGameSettings();  
    }


}