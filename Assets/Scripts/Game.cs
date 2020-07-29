using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    GameObject _player;
    GameObject[] enemies;
    [SerializeField] GameObject _enemy;
    [SerializeField] Enemy _prefabEnemy;
    GameObject _text;
    Image _panel;
    GameObject _lifeBar;
    Color gameOverColor;
	Color levelCompletedColor;
    [SerializeField] AudioSource _music;
    Pause _pause;
    [SerializeField] Button _buttonPause;

    private void Awake() 
    {
        _pause = FindObjectOfType<Pause>();
        _player = GameObject.Find("Player");
        _text = GameObject.Find("UI Text");
        _panel = GameObject.Find("Panel").GetComponent<Image>();
        _lifeBar = GameObject.Find("LifeBar");
    }

    void Start()
    {
        _panel.enabled = false;
        
        gameOverColor = new Color (1f, 0f, 0f, 0.3f); //red
		levelCompletedColor = new Color (0f, 0f, 1f, 0.3f); //blue
    }

    //llamado desde Key.cs
    void AddEnemies()
    {
        Transform enemiesTransform =  GameObject.Find("Enemies").GetComponent<Transform>();

        Instantiate(_prefabEnemy, enemiesTransform).SetPosition(-3f, 27f, 0f).SetScale(2f,2f,0f);
        Instantiate(_prefabEnemy, enemiesTransform).SetPosition(2f, 25f, 0f).SetScale(2f,2f,0f);
        Instantiate(_prefabEnemy, enemiesTransform).SetPosition(7f, 26f, 0f).SetScale(2f,2f,0f);
        Instantiate(_prefabEnemy, enemiesTransform).SetPosition(4f, 28f, 0f).SetScale(2f,2f,0f);
    }

    //llamado desde Player.cs y Warp.cs
    void DisableAttackEnemies(float time)
    {
        StartCoroutine("DisableAttackEnemiesCoroutine", time);
    }
    IEnumerator DisableAttackEnemiesCoroutine(float time)
    {
        enemies = GameObject.FindGameObjectsWithTag ("Enemy");

        //avisa a enemies que no sigan atacando
		for (int i = 0; i < enemies.Length; i++) 
		{
			if (enemies [i] != null)
			{
				enemies [i].SendMessage ("DisableAttack");
			}
		}

        yield return new WaitForSeconds (time);

        //avisa a enemies que ya pueden seguir atacando
		for (int i = 0; i < enemies.Length; i++) 
		{
			if (enemies [i] != null)
			{
				enemies [i].SendMessage ("EnableAttack");
			}
		}
    }
    /*void EnableAttackEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag ("Enemy");
        //avisa a enemies que ya pueden seguir atacando
		for (int i = 0; i < enemies.Length; i++) 
		{
			if (enemies [i] != null)
			{
				enemies [i].SendMessage ("EnableAttack");
			}
		}
    }*/

    //llamado desde Player.DamageCoroutine() o desde ClickExitToMenu.cs
	void Exit()
	{
		StartCoroutine(ChangeScene(0, "Juego Terminado", gameOverColor)); //escena 0: menú
	}
    //llamado desde ChestTimeline
    public void Win()
	{
        print("Game.Win()");
		StartCoroutine(ChangeScene(0, "Ganaste!", levelCompletedColor)); //escena 0: menú
	}
    IEnumerator ChangeScene (int scene, string text, Color color)
	{
        //Time.timeScale = 0.5f;

        //silencia música
		_music.pitch = 0f;

        //desactiva pausa y botón pausa
        _pause.enabled = false;
        _buttonPause.gameObject.SetActive(false);

        //desactiva barra
		_lifeBar.SetActive(false);
		
        //aparece texto
        _text.transform.GetChild(0).GetComponent<Text>().text = text; 
        _text.transform.GetChild(1).GetComponent<Text>().text = text;
        _text.GetComponent<Animator> ().Play ("Fade");
        
        //aparece panel
		_panel.enabled = true;
        _panel.color = color;

        if (text == "Juego Terminado")
        {
            yield return new WaitForSeconds (1f);
            GameObject.Destroy(_player);
        }

		yield return new WaitForSeconds (5f); 

		SceneManager.LoadScene (scene);
	}
}
