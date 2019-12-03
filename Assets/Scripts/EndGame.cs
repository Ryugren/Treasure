using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private GameObject endGameUI = null;
    [SerializeField]
    private GameManager gameManager = null;
    [SerializeField]
    private InputManager inputManager = null;
    private float timeCount = 0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Parameter.EndGameFlag)
        {
            endGameUI.SetActive(true);
            ResetGame();
        }
    }

    private void ResetGame()
    {
        if (inputManager.RC.IndexTrigger.Get)
        {
            if (timeCount < 10f)
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
