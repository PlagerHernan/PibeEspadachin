using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Thing")
        {
            StartCoroutine("DestroyOnCollision");
        }
    }

    IEnumerator DestroyOnCollision()
	{
        yield return new WaitForSeconds (0.10f);
        //desaparece pero sigue destruyendo things (los que están detrás del primer thing)
        GetComponent<SpriteRenderer> ().enabled = false; 
        yield return new WaitForSeconds (0.20f);
        GameObject.Destroy (gameObject);
	}

    //llamado desde player.cs
    void DestroySlash()
    {
        StartCoroutine("DestroySlashCoroutine");
    }

    IEnumerator DestroySlashCoroutine()
	{
        yield return new WaitForSeconds (1.5f);
        GameObject.Destroy (gameObject);	
	}

    
    private void OnBecameInvisible() 
    {
        GameObject.Destroy (gameObject);
    }
}
