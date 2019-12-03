using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//コクブ制作
//Date : 11/29

public class ReadyGame : MonoBehaviour
{
    [SerializeField]
    private GameObject readyGameUI = null;
    [SerializeField]
    private InputManager inputManager = null;
    [SerializeField]
    private GameManager gameManager = null;
    private void Update()
    {
        if (inputManager.LC.Button1.GetDown || inputManager.RC.Button1.GetDown)
        {
            readyGameUI.SetActive(false);
            gameManager.GameStart();
        }
    }
}
