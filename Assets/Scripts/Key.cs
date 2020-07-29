using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;
using UnityEngine.Playables;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject _game;
    public string objectToOpen;

    private void Awake() 
    {
        Assert.IsNotNull(objectToOpen, "Falta asignar objectToOpen en Key");
        Assert.IsNotNull(_game, "Falta asignar Game en Key");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<PlayableDirector>().Play();
            GameObject.Find(objectToOpen).SendMessage("Enable");

            if (objectToOpen == "Chest")
            {
                _game.SendMessage("AddEnemies");
            }
        }
    }

    //llamado desde ChestKeyTimeline
    public void DestroyObject()
    {
        GameObject.Destroy(gameObject);
    }
}
