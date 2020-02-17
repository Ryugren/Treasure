using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialObserver : BaseObserver
{
    [SerializeField]
    private AudioSource voice = null;
    public override void Action()
    {
        base.Action();
    }
    public override void CountTimer()
    {
        base.CountTimer();
    }
    public override void GameStart()
    {
        if (input.LC.IndexTrigger.GetDown && input.LC.IndexTrigger.Axis > 0.5f)
        {
            voice.Play();
            Parameter.StartGameFlag = true;
        }
    }
    public override void GameOver()
    {
    }
    public override void GameEnd()
    {
        SceneManager.LoadScene("MainGame");
    }
}
