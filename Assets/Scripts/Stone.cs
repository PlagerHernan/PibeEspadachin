using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IDestroyable
{
	Animator _animator;

	public Stone SetScale(Vector3 scale)
    {
        transform.localScale = scale;
        return this;
    }

	// Use this for initialization
	void Start () 
	{
		_animator = this.GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			Player player = GameObject.Find("Player").GetComponent<Player>();
			//si player no está herido, lo lastima 
			if (!player.Injured)
			{
				player.SendMessage("Damage");
			}

			GameObject.Destroy (gameObject); 
		}
		else if (col.gameObject.tag == "Attack")
		{
			StartCoroutine("DestroyObject");
		}
		else if (col.gameObject.tag == "Limit")
		{
			GameObject.Destroy (gameObject);
		}
	}

	public IEnumerator DestroyObject()
	{
		_animator.SetTrigger("attacked");

		GetComponent<CircleCollider2D>().enabled = false; 

		yield return new WaitForSeconds (1f);

		GameObject.Destroy(gameObject);
	}

	void OnBecameInvisible()
	{
		GameObject.Destroy (gameObject); 
	}
}
