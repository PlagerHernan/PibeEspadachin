using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Paths")]
    [SerializeField] string _sounds = "Audio/SFX";
    [SerializeField] string _music = "Audio/Music";

    [SerializeField] Dictionary<string, AudioClip> _soundList = new Dictionary<string, AudioClip>();
    [SerializeField] Dictionary<string, AudioClip> _musicList = new Dictionary<string, AudioClip>();

    void Awake()
    {
        _soundList = LoadSoundsFromStorage(_sounds);
        _musicList = LoadSoundsFromStorage(_music);
    }

    #region Metodos privados
    //Carga los clips que se encuentran en la carpeta de recursos
    Dictionary<string, AudioClip> LoadSoundsFromStorage(string pathInResources)
    {
        AudioClip[] tempAudios = Resources.LoadAll<AudioClip>(pathInResources);
        Dictionary<string, AudioClip> tempDictionary = new Dictionary<string, AudioClip>();

        foreach (var item in tempAudios)
        {
            tempDictionary.Add(item.name, item);
        }

        return tempDictionary;
    }
    #endregion

    #region Metodos publicos
    /// <summary>
    /// Devuelve un clip de audio correspondiente al nombre.
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    public AudioClip GetSound(string sound)
    {
        string[] soundSelection = sound.Split('_');

        if (soundSelection[0] == "m")
            return _musicList[sound];
        else
            return _soundList[sound];
    }
    #endregion
}
