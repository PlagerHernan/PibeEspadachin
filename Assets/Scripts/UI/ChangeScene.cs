using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ChangeScene : MonoBehaviour, IPointerUpHandler
{

    [SerializeField] int _targetScene;

    /* public void LoadNewScene(int targetScene)
    {
        SceneManager.LoadScene(targetScene);
    } */

    public void OnPointerUp (PointerEventData evenData)
	{
        SceneManager.LoadScene(_targetScene);
	}
}
