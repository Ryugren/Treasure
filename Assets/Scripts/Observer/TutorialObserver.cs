using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialObserver : BaseObserver
{
    [SerializeField]
    private AudioSource[] voices = null;
    private bool instructFlag = false;
    [SerializeField]
    private Text instructions = null;
    private bool isChecked = false;
    private TutorialStatus state = TutorialStatus.First;
    private float waitTime = 0;
    public enum TutorialStatus
    {
        First,
        LeftTurnCamera,
        RightTurnCamera,
        Move,
        SearchDoor,
        LightDown,
        LightOn,
        LightAtTarget,
        Move2,
        TreadTrap,
        BeamOn,
        BeamAtTarget,
        TryGame
    }
    private void Awake()
    {
        input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.All, true);
        input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.All, true);
        input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.All, false);
    }
    private void FixedUpdate()
    {
        if (waitTime <= 0) return;
        waitTime -= Time.deltaTime;
    }
    public override void Action()
    {
        base.Action();
        switch (state)
        {
            case TutorialStatus.First:
                First();
                break;
            case TutorialStatus.LeftTurnCamera:
                LeftTurnCamera();
                break;
            case TutorialStatus.RightTurnCamera:
                RightTurnCamara();
                break;
            case TutorialStatus.Move:
                Move();
                break;
            case TutorialStatus.SearchDoor:
                SearchDoor();
                break;
            case TutorialStatus.LightDown:
                LightDown();
                break;
            case TutorialStatus.LightOn:
                LightOn();
                break;
            case TutorialStatus.LightAtTarget:
                LightAtTarget();
                break;
            case TutorialStatus.Move2:
                Move2();
                break;
            case TutorialStatus.TreadTrap:
                TreadTrap();
                break;
            case TutorialStatus.BeamOn:
                BeamOn();
                break;
            case TutorialStatus.BeamAtTarget:
                BeamAtTarget();
                break;
            case TutorialStatus.TryGame:
                TryGame();
                break;
        }
    }
    public override void CountTimer()
    {
        //base.CountTimer();
        if (instructFlag)
        {
            Parameter.CurrentPlayTime += Time.deltaTime;
            if (Parameter.MaxPlayTime - Parameter.CurrentPlayTime <= 0)
            {
                Parameter.EndGameFlag = true;
                Parameter.CurrentPlayTime = Parameter.MaxPlayTime;
            }
        }
    }
    public override void GameStart()
    {
        if (input.LC.IndexTrigger.GetDown && input.LC.IndexTrigger.Axis > 0.5f)
        {
            GM.Parameter.StartGameFlag = true;
            isChecked = false;
            state = TutorialStatus.First;
}
    }
    public override void GameOver()
    {
    }
    public override void GameEnd()
    {
        SceneManager.LoadScene("MainGame");
    }
    private void Waiting()
    {
        waitTime = 5f;
    }
    private void First()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.IndexTrigger, true);
            instructions.text = "操作確認を始めます";
            Waiting();
            isChecked = true;
        }
        else if (waitTime <= 0)
        {
            isChecked = false;
            state = TutorialStatus.LeftTurnCamera;
        }
    }
    private void LeftTurnCamera()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.HandTrigger, false);
            instructions.text = "左の中指を\nにぎってください";
            isChecked = true;
        }
        else if (input.LC.HandTrigger.Axis > 0.5f)
        {
            isChecked = false;
            state = TutorialStatus.RightTurnCamera;
        }
    }
    private void RightTurnCamara()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.HandTrigger, false);
            instructions.text = "右の中指を\nにぎってください";
            isChecked = true;
        }
        else if (input.RC.HandTrigger.Axis > 0.5f)
        {
            isChecked = false;
            state = TutorialStatus.Move;
        }
    }
    private void Move()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.Stick, false);
            instructions.text = "左スティックを\n倒して進んでください";
            isChecked = true;
        }
        else if (input.LC.AxisStick.magnitude > 0.5f)
        {
            isChecked = false;
            state = TutorialStatus.SearchDoor;
        }
    }
    private void SearchDoor()
    {
        if (!isChecked)
        {
            instructions.text = "では扉を探してください";
            isChecked = true;
        }
        else if (true)
        {
            isChecked = false;
            state = TutorialStatus.LightDown;
        }
    }
    private void LightDown()
    {
        if (!isChecked)
        {
            instructions.text = "扉と向かい合ってください";
            isChecked = true;
        }
        else if (true)
        {
            isChecked = false;
            state = TutorialStatus.LightOn;
        }
    }
    private void LightOn()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.IndexTrigger, false);
            instructions.text = "左人差し指を\nにぎってください";
            isChecked = true;
        }
        else if (input.LC.IndexTrigger.Axis > 0.5f)
        {
            isChecked = false;
            state = TutorialStatus.LightAtTarget;
        }
    }
    private void LightAtTarget()
    {
        if (!isChecked)
        {
            instructions.text = "壁のシンボルに向かって\nライトを当ててください";
            isChecked = true;
        }
        else if (true)
        {
            isChecked = false;
            state = TutorialStatus.Move2;
        }
    }
    private void Move2()
    {
        if (!isChecked)
        {
            instructions.text = "では扉の向こう側へ\n進んでください";
            isChecked = true;
        }
        else if (true)
        {
            isChecked = false;
            state = TutorialStatus.TreadTrap;
        }
    }
    private void TreadTrap()
    {
        if (!isChecked)
        {
            instructions.text = "罠を踏んだことで\nダメージを受けました";
            isChecked = true;
            Waiting();
        }
        else if (waitTime <= 0)
        {
            isChecked = false;
            state = TutorialStatus.BeamOn;
        }
    }
    private void BeamOn()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.IndexTrigger, false);
            instructions.text = "右人差し指を\nにぎってください";
            isChecked = true;
        }
        else if (input.RC.IndexTrigger.Axis > 0.5f)
        {
            isChecked = false;
            state = TutorialStatus.BeamAtTarget;
        }
    }
    private void BeamAtTarget()
    {
        if (!isChecked)
        {
            instructions.text = "罠に向かって\nビームを当ててください";
            isChecked = true;
        }
        else if (true)
        {
            isChecked = false;
            state = TutorialStatus.TryGame;
        }
    }
    private void TryGame()
    {
        if (!isChecked)
        {
            instructions.text = "操作確認は以上です";
            isChecked = true;
            instructFlag = true;
        }
    }
}
