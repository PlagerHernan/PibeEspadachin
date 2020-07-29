using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;

public class Player : MonoBehaviour 
{
	//GameObjects
	GameObject game;

	//salud
	public GameObject healthBar;
	float playerHealth;
	bool _injured = false;
	public bool Injured
	{
		get { return _injured; }
		set { _injured = value; }
	}

	//movimiento
	bool _freeMovement = true;
	Vector2 move;
	public float speed = 4f;
	Vector2 _lastOrientation = Vector2.down; //por defecto player mira hacia abajo

	//slash attack
	public GameObject slashPrefab;
	public float secondsForSlash;
	bool _slashLoaded = false;

	//componentes
	Animator playerAnimator;
	Rigidbody2D _rigidbody;
	SpriteRenderer _spriteRenderer;
	CircleCollider2D _attackCollider;

	void Awake()
	{
		Assert.IsNotNull (slashPrefab, "Falta asignar slashPrefab en Player");
		Assert.IsNotNull (healthBar, "Falta asignar healthBar en Player");

		game = GameObject.Find("Game");

		playerAnimator = GetComponent<Animator> ();
		_rigidbody = GetComponent<Rigidbody2D> ();
		_spriteRenderer = GetComponent<SpriteRenderer> ();
		_attackCollider = GetComponentInChildren<CircleCollider2D> ();
	}

	// Use this for initialization
	void Start () 
	{
		_attackCollider.enabled = false; //espada desactivada

		playerHealth = 1f; //salud llena
		//healthBar.rectTransform.localScale = new Vector3 (playerHealth, 1f, 1f); //configura barra de salud 
	}
	
	// Update is called once per frame
	void Update () 
	{
		Movement();
		Animations();
		Attack(); 
	}

	void FixedUpdate()
	{
		//movimiento player
		if (_freeMovement) 
		{
			_rigidbody.MovePosition(_rigidbody.position + move * speed * Time.deltaTime);
		}

		//ataque rayo 
		SlashAttack();
	}

	private void OnTriggerStay2D(Collider2D other) 
	{
		//reposicionamiento
		if (other.gameObject.tag == "Thing")
		{
			//si player está arriba de thing, se coloca en una capa inferior (se ve atrás)
			if (this.transform.position.y > other.gameObject.transform.position.y)
			{
				_spriteRenderer.sortingLayerName = "BackThing";
			} 
			//si no, se coloca en una capa superior (se ve adelante)
			else
			{
				_spriteRenderer.sortingLayerName = "FrontThing";	
			}
		}
	}

	void Movement()
	{
		if (_freeMovement) 
		{
			//input moviemiento (concretado en FixedUpdate())
			move = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		}
	}

	void Animations()
	{
		//animaciones idle y walk
		if (move != Vector2.zero) //si se mueve 
		{
			playerAnimator.SetFloat("movX", move.x);
			playerAnimator.SetFloat("movY", move.y);
			playerAnimator.SetBool ("walking", true);
		} 
		else //si no se mueve
		{
			playerAnimator.SetBool ("walking", false);
		}
	}

	void Attack()
	{
		//reubicación collider attack (según posición de player)
		if (move != Vector2.zero) //si se mueve, modifica posición collider
		{
			_attackCollider.offset = new Vector2(move.x/2, move.y/2);
		}

		AnimatorStateInfo info = playerAnimator.GetCurrentAnimatorStateInfo (0);
		bool _animAttacking = info.IsName ("Player_Attack");

		//si no está atacando, ataca (se asegura que termine la animación antes de empezar nueva)
		if (Input.GetKeyDown (KeyCode.Space) && !_animAttacking) 
		{
			playerAnimator.SetTrigger ("attacking");
			_attackCollider.enabled = true; //activa espada 
		} 
		else if (!_animAttacking)
		{
			_attackCollider.enabled = false; //desactiva espada
		}
	}

	void SlashAttack()
	{
		//guardo la última orientación de player, antes de que vuelva a cero al detenerse
		if (move != Vector2.zero) //si se mueve 
		{
			_lastOrientation = move;
		}

		//CARGA
		//mientras se tenga presionado shift y no esté ya atacando
		if (Input.GetKeyDown(KeyCode.LeftShift) && !playerAnimator.GetBool("slashing")) 
		{
			playerAnimator.SetBool("slashing", true);
			_freeMovement = false;
			_rigidbody.velocity = Vector2.zero;
			//si se mantiene presionado shift durante 1.7 segundos, invoca al método
			Invoke("WaitForSlash", 1.7f);
		} 
		//DESCARGA
		//cuando se suelta shift
		else if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			CancelInvoke("WaitForSlash");
			//toda animación de slash termina con animación de espada
			playerAnimator.SetBool("slashing", false);
			playerAnimator.SetTrigger ("attacking");
			_freeMovement = true;

			//ATAQUE
			//si se cargó lo suficiente
			if (_slashLoaded) 
			{
				float _angle = Mathf.Atan2 (_lastOrientation.y, _lastOrientation.x) * Mathf.Rad2Deg;

				GameObject slash = Instantiate (slashPrefab, this.transform.position, Quaternion.AngleAxis(_angle, Vector3.forward)); 
				slash.GetComponent<Rigidbody2D> ().AddForce (_lastOrientation * speed * Time.deltaTime, ForceMode2D.Impulse);

				_slashLoaded = false;
				slash.SendMessage("DestroySlash");
			}	
		}
	}
	void WaitForSlash()
	{
		_slashLoaded = true;
	}

	//llamado desde Stone.cs
	void Damage()
	{
		StartCoroutine("DamageCoroutine");
	}
	IEnumerator DamageCoroutine()
	{
		_injured = true;
		
		//resta un cuarto de salud (nunca más de 1 ni menos de 0)
		playerHealth = Mathf.Clamp (playerHealth - 0.25f, 0f, 1f); 
		//actualiza barra de salud
		healthBar.GetComponent<RectTransform>().localScale = new Vector3(playerHealth, 1f, 1f);

		playerAnimator.SetTrigger("shock");

		game.SendMessage("DisableAttackEnemies", 2f);

		//si player ha muerto
		if (healthBar.GetComponent<RectTransform>().localScale.x == 0f)
		{
			game.SendMessage("Exit");
			yield break;
		}

		yield return new WaitForSeconds (1f);
		
		_injured = false;
	}

	//llamado desde Signals
	public void Freeze()
	{
		_freeMovement = false;
		_rigidbody.velocity = Vector2.zero;
		move = Vector2.zero;
		playerAnimator.enabled = false;
	}
	//llamado desde Signals
	public void Unfreeze()
	{
		_freeMovement = true;
		playerAnimator.enabled = true;
	}
}

