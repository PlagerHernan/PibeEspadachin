using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] protected bool _musicState = true;
    [SerializeField] protected bool _soundState = true;

    protected SaveSystem _saveSystem;
    protected GameSettings _gameSettings;

    protected bool _lastMusicState;
    protected bool _lastSoundState;

    //propiedades de Settings
    public bool MusicOn { get => _musicState; set => _musicState = value; }
    public bool SoundOn { get => _soundState; set => _soundState = value; }

    virtual protected void Awake()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
    }

    //cierre forzado
    virtual protected void OnApplicationQuit() 
    {
        SaveSettingsInfo();
    }

    //Reinicia escena o vuelve al menu
    virtual protected void OnDisable()
    {
        SaveSettingsInfo();
    }

    //Guardado de settings
    public void SaveSettingsInfo()
    {
        _gameSettings.musicOn = _musicState;
        _gameSettings.soundFXOn = _soundState;

        var oldGameSettings = _saveSystem.GetGameSettings();
        if (!oldGameSettings.Equals(_gameSettings))
            _saveSystem?.SetGameSettings(_gameSettings);
    }

    //Carga de settings
    protected void LoadSettingsInfo()
    {
        _gameSettings = _saveSystem.GetGameSettings();
        _musicState = _gameSettings.musicOn;
        _soundState = _gameSettings.soundFXOn;
    }
}