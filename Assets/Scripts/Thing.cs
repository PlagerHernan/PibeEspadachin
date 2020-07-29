using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour, IDestroyable
{
	SpriteRenderer _spriteRenderer;
	Animator _animator;

	void Awake()
	{
		_spriteRenderer = this.GetComponent<SpriteRenderer> ();
		_animator = this.GetComponent<Animator> ();
	}

	void Start () 
	{
		//REPOSICIONAMIENTO: asigna órden de capa según su posición en y (para cuando hay varios things superpuestos)
		_spriteRenderer.sortingOrder = Mathf.RoundToInt(-this.transform.position.y * 100);
	}

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "Attack") 
		{
			StartCoroutine("DestroyObject");
		}
	}

	public IEnumerator DestroyObject()
	{
		_animator.SetTrigger("attacked");

		yield return new WaitForSeconds (0.5f);

		GetComponent<CapsuleCollider2D>().enabled = false; 

		yield return new WaitForSeconds (0.5f);

		GameObject.Destroy(gameObject);
	}
}
