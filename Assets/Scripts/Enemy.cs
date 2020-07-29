using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDestroyable
{
	[SerializeField] Stone _stone;
	GameObject player;
	Animator _animator;
	Rigidbody2D _rigidbody;

	public Enemy SetPosition(float x, float y,float z)
    {
        transform.position = new Vector3(x,y,z);
        return this;
    }

	public Enemy SetScale(float x, float y, float z)
    {
        transform.localScale = new Vector3(x, y, z);
        return this;
    }

	public float pursuitRadius = 4f;
	public float attackRadius = 2.5f;
	public float velocity = 2f;
	Vector3 initialPosition;
	Vector3 direction;
	bool _attacking;
	Vector3 target;
	bool _die;

	bool _canAttack = true;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		_animator = GetComponent<Animator> ();
		_rigidbody = GetComponent<Rigidbody2D> ();

		initialPosition = transform.position;
		target = initialPosition;
	}

	void FixedUpdate () 
	{
		bool _canSleep = true;
		bool _limited = false;

		//si player y enemy están vivos
		if (player != null && !_die) 
		{
			RaycastHit2D hit = Physics2D.Raycast (transform.position, 
				player.transform.position - transform.position, 
				pursuitRadius, 
				1 << LayerMask.NameToLayer ("Default")); 
				// Editor: dejar a Player y Limit for Enemy en Layer Default. Todo lo demás poner en Layer Ignore Raycast

			//debuguea Raycast
			Debug.DrawRay (transform.position, player.transform.position - transform.position, Color.green);

			if (hit.collider != null) 
			{
				//print ("raycast colisionando contra: " + hit.collider.name);

				if (hit.collider.tag == "Player")
				{
					target = player.transform.position;
				}
				//si colisiona contra Limit for Enemy
				else 
				{
					target = initialPosition;
					_limited = true;
				}
			} 
			else 
			{
				target = initialPosition;
			}

			direction = (target - transform.position).normalized;

			_animator.SetFloat ("movX", direction.x);
			_animator.SetFloat ("movY", direction.y);

			float distance = Vector3.Distance (player.transform.position, transform.position);

			//si player entra en el radio de persecusión, se mueve hacia player
			if (distance < pursuitRadius && distance > attackRadius && !_limited) 
			{
				_animator.SetBool ("walking", true);
				_rigidbody.MovePosition (transform.position + direction * velocity * Time.deltaTime);

				_canSleep = false;
			} 
			//si player entra en el radio de ataque, se detiene y ataca
			else if (distance < attackRadius  && !_limited)
			{
				_animator.Play ("Enemy_Walk", -1, 0); //congela animación de caminar

				//si ya terminó el ataque (con su WaitForSeconds), ataca
				if (!_attacking)
				{
					StartCoroutine ("Attack");
				}

				_canSleep = false;
			}
			//si está en posicion inicial (o casi, para evitar bug temblor) y player no está cerca, se duerme
			else if (Vector3.Distance (initialPosition, transform.position) <= 0.2f && _canSleep) 
			{
				_animator.SetBool ("walking", false);
			}
			//si no, vuelve a posicion inicial
			else if(_canSleep)
			{
				_animator.SetBool ("walking", true);
				_rigidbody.MovePosition (transform.position + direction * velocity/2 * Time.deltaTime);
			}

		}

		//linea hasta target
		Debug.DrawLine(transform.position, target, Color.red);
	}

	//llamado desde Game.cs
	void DisableAttack()
	{
		_canAttack = false;
	}
	//llamado desde Game.cs
	void EnableAttack()
	{
		_canAttack = true;
	}

	IEnumerator Attack()
	{
		if(_canAttack)
		{
			_attacking = true;

			Transform stonesTransform =  GameObject.Find("Stones").GetComponent<Transform>();
			var newStone = Instantiate(_stone, transform.position, transform.rotation, stonesTransform).SetScale(transform.localScale);

			newStone.GetComponent<Rigidbody2D> ().AddForce (direction * velocity/1.1f * Time.deltaTime, ForceMode2D.Impulse);
			yield return new WaitForSeconds (1.5f);
			_attacking = false;
		}
	}

	void OnDrawGizmos()
	{
		if (player != null) 
		{
			Gizmos.DrawWireSphere (transform.position, pursuitRadius);
			Gizmos.DrawWireSphere (transform.position, attackRadius);
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		//muerte
		if (other.gameObject.tag == "Attack")
		{
			_die = true;
			_animator.SetBool("walking", false);

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
