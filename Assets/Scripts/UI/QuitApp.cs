using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitApp : MonoBehaviour
{
	Manager _manager;

	//llamado desde Exit Button 
    public void ExitApp()
	{
		_manager = GameObject.FindObjectOfType<Manager>();
		
		_manager.SaveSettingsInfo();

		Application.Quit();
		Debug.Log ("aplicación cerrada");
	}
}
