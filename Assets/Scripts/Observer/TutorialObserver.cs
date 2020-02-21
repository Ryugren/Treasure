using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialObserver : BaseObserver
{
    [SerializeField]
    private AudioSource correctSE = null;
    [SerializeField]
    private AudioSource[] voices = null;
    private bool instructFlag = false;
    [SerializeField]
    private Text instructions = null;
    private bool isChecked = false;
    private TutorialStatus state = TutorialStatus.First;
    private float coolTimeCount = 0f;
    [SerializeField, ColorUsage(false)]
    private Color minColor = Color.gray;
    private Color maxColor;
    private float maxTime = 3f;
    private float lightTimeCount = 0f;
    //
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private PositionChecker doorPositionChecker = null;
    private Vector3 checkCurrentPosition = Vector3.zero;
    [SerializeField]
    private SwitchSymbol symbol = null;
    [SerializeField]
    private BearTrap bearTrap = null;
    [SerializeField]
    private BearTrap breakBearTrap = null;
    [SerializeField]
    private SlideDoor slideDoor = null;
    public enum TutorialStatus
    {
        First,
        LeftTurnCamera,
        RightTurnCamera,
        IsLookedDoor,
        Move,
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
        input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.IndexTrigger, false);
        maxColor = RenderSettings.ambientSkyColor;
    }
    private void FixedUpdate()
    {
        if (coolTimeCount > 0) coolTimeCount -= Time.deltaTime;
    }
    public override void Action()
    {
        base.Action();
        if (coolTimeCount > 0) return;
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
            case TutorialStatus.IsLookedDoor:
                IsLookedDoor();
                break;
            case TutorialStatus.Move:
                Move();
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
        if (input.RC.IndexTrigger.Axis > 0.5f)
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
    private void Cooling()
    {
        coolTimeCount = maxTime;
    }
    private void First()
    {
        input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.IndexTrigger, true);
        instructions.text = "操作確認を始めます";
        state = TutorialStatus.LeftTurnCamera;
        Cooling();
    }
    private void LeftTurnCamera()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.HandTrigger, false);
            instructions.text = "まずは左の中指を\nにぎってください";
            isChecked = true;
        }
        else if (input.LC.HandTrigger.Axis > 0.5f)
        {
            Cooling();
            correctSE.Play();
            isChecked = false;
            state = TutorialStatus.RightTurnCamera;
        }
    }
    private void RightTurnCamara()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.HandTrigger, false);
            instructions.text = "次に右の中指を\nにぎってください";
            isChecked = true;
        }
        else if (input.RC.HandTrigger.Axis > 0.5f)
        {
            Cooling();
            correctSE.Play();
            isChecked = false;
            state = TutorialStatus.IsLookedDoor;
        }
    }
    private void IsLookedDoor()
    {
        if (!isChecked)
        {
            instructions.text = "扉と向かい合ってください";
            isChecked = true;
        }
        else if (player.AngleVisionNumber == 4)
        {
            correctSE.Play();
            isChecked = false;
            state = TutorialStatus.Move;
        }
    }
    private void Move()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.HandTrigger, true);
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.HandTrigger, true);
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.Stick, false);
            instructions.text = "左スティックを\n倒して進んでください";
            isChecked = true;
        }
        else if (doorPositionChecker.Flag)
        {
            Cooling();
            correctSE.Play();
            isChecked = false;
            state = TutorialStatus.LightDown;
        }
    }
    private void LightDown()
    {
        if (!isChecked)
        {
            instructions.text = "照明を暗くします";
            lightTimeCount = maxTime;
            isChecked = true;
        }
        else if (lightTimeCount < 0)
        {
            RenderSettings.ambientSkyColor = minColor;
            isChecked = false;
            state = TutorialStatus.LightOn;
        }
        else
        {
            Vector3 aPos = checkCurrentPosition;
            Vector3 bPos = new Vector3(doorPositionChecker.transform.position.x, checkCurrentPosition.y, doorPositionChecker.transform.position.z);
            player.transform.position = bPos + (bPos - aPos) * lightTimeCount / maxTime;
            RenderSettings.ambientSkyColor = minColor + (maxColor - minColor) * lightTimeCount / maxTime;
            lightTimeCount -= Time.deltaTime;
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
            Cooling();
            correctSE.Play();
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
        else if (symbol.Parameters[0].IsBreaked)
        {
            isChecked = false;
            correctSE.Play();
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
        else if (bearTrap.IsBreaked)
        {
            isChecked = false;
            state = TutorialStatus.TreadTrap;
        }
    }
    private void TreadTrap()
    {
        input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.Stick, true);
        instructions.text = "罠を踏んだことで\nダメージを受けました";
        state = TutorialStatus.BeamOn;
        Cooling();
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
            Cooling();
            correctSE.Play();
            isChecked = false;
            state = TutorialStatus.BeamAtTarget;
        }
    }
    private void BeamAtTarget()
    {
        if (!isChecked)
        {
            instructions.text = "罠に向かって\nビームを当ててください";
            slideDoor.SlideStart();
            isChecked = true;
        }
        else if (breakBearTrap.IsBreaked)
        {
            correctSE.Play();
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
