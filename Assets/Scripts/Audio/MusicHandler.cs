using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    SoundManager _soundManager;
    AudioSource _audioSource;
    Manager _manager;

    private void Awake() 
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _audioSource = GetComponent<AudioSource>();
        _manager = FindObjectOfType<Manager>();
    }

    private void Start() 
    {
        //Carga el estado de la música desde el manager. Si está desactivado, pausa la musica. Si no, la reproduce.
        if (_manager.MusicOn)
            _audioSource.Play();
        else
            _audioSource.Pause();
    }

    public void Play()
    {
        _manager.MusicOn = true;
        _audioSource.Play();
    }
    public void Pause()
    {
        _manager.MusicOn = false;
        _audioSource.Pause();
    }
    public void ChangeSong(string name)
    {
        var clip = _soundManager.GetSound(name);

        _audioSource.clip = clip;
        Play();
    }
}
