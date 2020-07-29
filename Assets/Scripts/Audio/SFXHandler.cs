using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    //Se pasa por inspector un audioclip con sonido click.
    [SerializeField] AudioClip _clickAudioClip;

    Manager _manager;
    SoundManager _soundManager;
    AudioSource _audioSource;

    private void Awake() 
    {
        _manager = FindObjectOfType<Manager>();
        _soundManager = FindObjectOfType<SoundManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() 
    {
        //Carga el estado del sonido desde el manager. Si está desactivado, mutea el AudioSource, sino lo desmutea.
        _audioSource.mute = !_manager.SoundOn;
    }

    public void ClickSound()
    {
        if(_clickAudioClip != null)
            PlaySound(_clickAudioClip.name);
    }
    public void PlaySound(string name)
    {
        var clip = _soundManager.GetSound(name);
        _audioSource.PlayOneShot(clip);
    }

    public void Mute(bool state)
    {
        //Si mutear es falso, setea la propiedad en el manager a true (porque el sonido está en On) y desmutea el AudioSource
        _manager.SoundOn = !state;
        _audioSource.mute = state;
    }
}
