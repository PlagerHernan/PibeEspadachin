using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MusicButton : MonoBehaviour, IPointerUpHandler
{
    MusicHandler _musicHandler;
    Toggle _musicToggle;

    private void Awake() 
    {
        _musicHandler = FindObjectOfType<MusicHandler>();
        _musicToggle = GetComponent<Toggle>();
    }

    private void Start() 
    {
        _musicToggle.isOn = !FindObjectOfType<Manager>().MusicOn;
    }

    public void OnPointerUp (PointerEventData evenData)
	{
        if (_musicToggle.isOn)
            _musicHandler.Play();
        else
            _musicHandler.Pause();
    }
}
