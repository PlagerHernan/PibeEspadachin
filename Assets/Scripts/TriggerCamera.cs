using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Playables;
using UnityEngine.Assertions;
using Cinemachine;

public class TriggerCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _targetCamera;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            /*GetComponent<PlayableDirector>().Play();
            //desactiva trigger (sólo lo necesito una vez)
            GetComponent<BoxCollider2D>().enabled = false;*/

            _targetCamera.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
       if (other.gameObject.tag == "Player")
        {
            _targetCamera.gameObject.SetActive(false);
        } 
    }
}
