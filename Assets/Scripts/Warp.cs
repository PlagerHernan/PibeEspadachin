using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;
using Cinemachine;
using Cinemachine.Utility;
using System.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Warp : MonoBehaviour 
{
	GameObject _game;
	public GameObject targetExit;
	//public GameObject targetMap;
	public CinemachineStateDrivenCamera targetCamera;
	public CinemachineStateDrivenCamera currentCamera;
	GameObject text;
	public string nextMap;
	public bool warpEnabled; //Editor: todos los warps habilitados excepto hacia la casa (con llave)

	//GUI
	Collider2D _targetMapCollider;
	bool _onGUI = false;
	bool _fadeIn;
	bool _fadeOut;
	float _alpha = 0.0f;
	public float fadeTime = 1f; 
	public float waitTime = 2f; 
	

	void Awake()
	{
		Assert.IsNotNull (targetCamera, "Falta asignar targetCamera en Warp");
		Assert.IsNotNull (currentCamera, "Falta asignar currentCamera en Warp");

		//invisibiliza las flechas
		GetComponent<SpriteRenderer> ().enabled = false;
		transform.GetChild(0).GetComponent<SpriteRenderer> ().enabled = false;

		//_targetMapCollider = targetMap.GetComponent<Collider2D> ();
	}

	void Start () 
	{
		text = GameObject.Find("UI Text");
		_game = GameObject.Find("Game");
	}

	//llamado desde doorTimeline
	public void EnableWarp()
	{
		warpEnabled = true;
	}

	IEnumerator OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && warpEnabled)
		{
			//cambia limites de cinemachine
			/*CinemachineConfiner cmConfiner = currentCamera.GetComponent<CinemachineConfiner> ();
			Collider2D _boundingShape = cmConfiner.m_BoundingShape2D;
			Debug.Log (_boundingShape);

			cmConfiner.InvalidatePathCache ();
			_boundingShape = null;
			_boundingShape = _targetMapCollider;
			Debug.Log (_boundingShape);*/ 

			_game.SendMessage("DisableAttackEnemies", 3f);

			//congelamiento movimiento player 
			other.GetComponent<Player> ().enabled = false;

			//pantalla oscura 
			ActiveOnGUI();

			//cambio de cámara
			targetCamera.gameObject.SetActive (true);
			currentCamera.gameObject.SetActive (false);

			yield return new WaitForSeconds (waitTime);

			//pantalla transparente
			_fadeOut = true;

			//descongelamiento movimiento player 
			other.GetComponent<Player> ().enabled = true;

			//reposición de player en la salida del target
			other.transform.position = targetExit.transform.GetChild (0).position;

			text.transform.GetChild(0).GetComponent<Text>().text = nextMap; 
			text.transform.GetChild(1).GetComponent<Text>().text = nextMap;
			text.GetComponent<Animator> ().Play ("Fade");
		}
	}

	void ActiveOnGUI()
	{
		_onGUI = true;
		_fadeIn = true;
	}

	//pantalla oscura al cambiar de mapa
	void OnGUI()
	{
		if (!_onGUI)
		{
			return;
		}

		//configura color de GUI
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, _alpha);

		//crea una textura (con el color ya configurado)
		Texture2D _texture = new Texture2D (1, 1);
		_texture.SetPixel (0, 0, Color.black);
		_texture.Apply ();

		//dibuja la textura
		GUI.DrawTexture (new Rect (0f, 0f, Screen.width, Screen.height), _texture);

		//incrementa opacidad
		if (_fadeIn)
		{
			_alpha = Mathf.Lerp(_alpha, 1.1f, fadeTime * Time.deltaTime); 
			
			if (_alpha >= 1f)
			{
				_fadeIn = false;
			}
		} 
		//disminuye opacidad
		else if (_fadeOut) 
		{
			_alpha = Mathf.Lerp(_alpha, -0.1f, fadeTime * Time.deltaTime);

			if (_alpha <= 0f)
			{
				_fadeOut = false;
				_onGUI = false;
			}
		}
	}
}
