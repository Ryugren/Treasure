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
        input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.IndexTrigger, false);
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
        if (input.LC.IndexTrigger.Axis > 0.5f)
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
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.IndexTrigger, true);
            instructions.text = "操作確認を始めます";
            voices[0].Play();
            isChecked = true;
        }
        else if (!voices[0].isPlaying)
        {
            state = TutorialStatus.LeftTurnCamera;
            isChecked = false;
        }
    }
    private void LeftTurnCamera()
    {
        if (!isChecked)
        {
            instructions.text = "";
            voices[1].Play();
            isChecked = true;
        }
        else if (!voices[1].isPlaying)
        {
            instructions.text = "左の中指を\nにぎってください";
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.HandTrigger, false);
        }
        if (input.LC.HandTrigger.Axis > 0.5f)
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
            voices[2].Play();
            isChecked = true;
            instructions.text = "";
        }
        else if (!voices[2].isPlaying)
        {
            instructions.text = "右の中指を\nにぎってください";
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.HandTrigger, false);
        }
        if (input.RC.HandTrigger.Axis > 0.5f)
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
            instructions.text = "";
            voices[3].Play();
            isChecked = true;
        }
        else if (!voices[3].isPlaying)
        {
            instructions.text = "扉と向かい合ってください";
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.HandTrigger, false);
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.HandTrigger, false);
        }
        if (!voices[3].isPlaying && player.AngleVisionNumber == 4)
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
            instructions.text = "";
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.HandTrigger, true);
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.HandTrigger, true);
            voices[4].Play();
            isChecked = true;
        }
        else if (!voices[4].isPlaying)
        {
            instructions.text = "左スティックを\n倒して進んでください";
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.Stick, false);
        }
        if (doorPositionChecker.Flag)
        {
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
            checkCurrentPosition = player.transform.position;
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.Stick, true);
            voices[5].Play();
            isChecked = true;
        }
        else if (lightTimeCount >= 0)
        {
            Vector3 aPos = checkCurrentPosition;
            Vector3 bPos = new Vector3(doorPositionChecker.transform.position.x, checkCurrentPosition.y, doorPositionChecker.transform.position.z);
            player.transform.position = bPos + (aPos - bPos) * lightTimeCount / maxTime;
            RenderSettings.ambientSkyColor = minColor + (maxColor - minColor) * lightTimeCount / maxTime;
            lightTimeCount -= Time.deltaTime;
        }
        else
        {
            RenderSettings.ambientSkyColor = minColor;
            isChecked = false;
            state = TutorialStatus.LightOn;
        }
    }
    private void LightOn()
    {
        instructions.text = "左人差し指を\nにぎってください";
        input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.IndexTrigger, false);
        Cooling();
        state = TutorialStatus.LightAtTarget;
    }
    private void LightAtTarget()
    {
        if (!isChecked)
        {
            instructions.text = "壁のシンボルに向かって\nライトを当ててください";
            isChecked = true;
        }
        if (!voices[5].isPlaying && symbol.Parameter.IsBreaked)
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
            instructions.text = "";
            voices[6].Play();
            isChecked = true;
        }
        else if (!voices[6].isPlaying)
        {
            instructions.text = "では扉の向こう側へ\n進んでください";
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.Stick, false);
        }
        if (bearTrap.IsBreaked)
        {
            isChecked = false;
            state = TutorialStatus.TreadTrap;
        }
    }
    private void TreadTrap()
    {
        if (!isChecked)
        {
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.Stick, true);
            instructions.text = "罠を踏んだことで\nダメージを受けました";
            voices[7].Play();
            isChecked = true;
        }
        if (!voices[7].isPlaying)
        {
            isChecked = false;
            state = TutorialStatus.BeamOn;
        }
    }
    private void BeamOn()
    {
        input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.IndexTrigger, false);
        instructions.text = "右人差し指を\nにぎってください";
        voices[8].Play();
        Cooling();
        state = TutorialStatus.BeamAtTarget;
    }
    private void BeamAtTarget()
    {
        if (!isChecked)
        {
            instructions.text = "罠に向かって\nビームを当ててください";
            slideDoor.SlideStart();
            isChecked = true;
        }
        if (!voices[8].isPlaying && breakBearTrap.IsBreaked)
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
            voices[9].Play();
            instructions.text = "";
            isChecked = true;
        }
        else if(!voices[9].isPlaying)
        {
            instructions.text = $"{Mathf.CeilToInt(GM.Parameter.MaxPlayTime - GM.Parameter.CurrentPlayTime)}秒後、本番が始まります";
            input.SafetyLock(InputManager.Hands.Right, InputManager.ButtonLock.All, false);
            input.SafetyLock(InputManager.Hands.Left, InputManager.ButtonLock.All, false);
            instructFlag = true;
        }
    }
}
