using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _controlsPanel;
    [SerializeField] GameObject _pauseButton;
    [SerializeField] AudioSource _music;
    Player _player;
    bool _onPause;

    private void Awake() 
    {
        _player = FindObjectOfType<Player>();  
    }

    void Start()
    {
        _onPause = false;
        SetPause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePause();
        }
    }

    //llamado desde Back Button del Pause Panel y desde Pause Button
    public void ChangePause()
    {
        _onPause = !_onPause;
        SetPause();
    }

    void SetPause()
    {
        if (_onPause)
        {
            _pausePanel.SetActive(true);
            _pauseButton.SetActive(false);

            Time.timeScale = 0f;

            //silencia música
            _music.pitch = 0f;

            _player.Freeze();
        }
        else
        {
            _pausePanel.SetActive(false);
            _pauseButton.SetActive(true);
            _controlsPanel.SetActive(false);

            Time.timeScale = 1f;

            //normaliza música
            _music.pitch = 1f;

            _player.Unfreeze();
        }
    }
}
