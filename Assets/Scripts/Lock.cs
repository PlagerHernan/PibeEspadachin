using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Playables;

public class Lock : MonoBehaviour
{
     bool key; 
     
     bool _onGUI;
     GUIStyle style = new GUIStyle();
     Texture2D _texture;

    void Start()
    {
        //GUIStyle
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        _texture = new Texture2D (1, 1);
		_texture.SetPixel (0, 0, new Color(0f, 0f, 0f, 0.5f)); //color negro con transparencia
		_texture.Apply ();
        style.normal.background = _texture;
    }

    //llamado desde Key.cs
    void Enable()
    {
        key = true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.name == "Player")
        {
            //llave conseguida -> se abre
            if (key)
            {
                GetComponent<PlayableDirector>().Play();
                GetComponent<BoxCollider2D>().enabled = false;
            }
            //llave no conseguida -> no se abre
            else
            {
                StartCoroutine("ActivateOnGUI");
            }
        }
    }

    IEnumerator ActivateOnGUI()
    {
        _onGUI = true;
        yield return new WaitForSeconds(2.5f);
        _onGUI = false;
    }

    private void OnGUI() 
    {
        if (!_onGUI)
        {
            return;
        }

        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Está cerrado! Busca la llave para abrir", style);
    }
}
