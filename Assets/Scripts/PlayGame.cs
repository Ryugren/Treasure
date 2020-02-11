using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager = null;
    [SerializeField]
    private InputManager inputManager = null;
    private float timeCount = 0f;
    [SerializeField]
    private GameObject[] players = null;
    [SerializeField]
    private GameObject cave = null;
    [SerializeField]
    private AudioSource loginSE = null;
    [SerializeField]
    private AudioSource longNoiseSE = null;
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (!players[i].activeSelf) continue;
            players[i].SetActive(false);
        }
        cave.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameManager.Parameter.StartGameFlag)
        {
            ReadyGame();
        }
        if (gameManager.Parameter.EndGameFlag)
        {
            ResetGame();
        }
    }
    private void ReadyGame()
    {
        if (inputManager.LC.IndexTrigger.Axis > 0.5f)
        {
            loginSE.Play();
            gameManager.GameStart();
            for (int i = 0; i < players.Length; ++i)
            {
                if (players[i].activeSelf) continue;
                players[i].SetActive(true);
            }
            cave.SetActive(true);
        }
    }

    private void ResetGame()
    {
        if (cave.activeSelf)
        {
            cave.SetActive(false);
            longNoiseSE.Play();
        }
        for (int i = 0; i < players.Length; ++i)
        {
            if (!players[i].activeSelf) continue;
            players[i].SetActive(false);
        }
        if (inputManager.LC.IndexTrigger.Axis > 0.5f)
        {
            if (timeCount < 3f)
            {
                timeCount += Time.deltaTime;
                return;
            }
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            timeCount = 0;
        }
    }
}
