using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundButton : MonoBehaviour, IPointerUpHandler
{
    SFXHandler _sfxHandler;
    Toggle _soundToggle;

    private void Awake() 
    {
        _sfxHandler = FindObjectOfType<SFXHandler>();
        _soundToggle = GetComponent<Toggle>();
    }

    private void Start() 
    {
        _soundToggle.isOn = !FindObjectOfType<Manager>().SoundOn;
    }

    public void OnPointerUp (PointerEventData evenData)
	{
        //Fijarse temas tiempos con el toggle
        _sfxHandler.Mute(!_soundToggle.isOn);
	}
}
