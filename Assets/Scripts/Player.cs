using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ビーム、右アーム
    /// </summary>
    [SerializeField]
    private GameObject beamArm = null;
    /// <summary>
    /// ライト、左アーム
    /// </summary>
    [SerializeField]
    private GameObject lightArm = null;
    [SerializeField]
    private GameManager gameManager = null;
    [SerializeField]
    private InputManager inputManager = null;
    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private int cameraTypeFlag = 1;
    [SerializeField]
    private int angleVisionCutNumber = 4;
    private int angleVisionNumber = 0;
    [SerializeField]
    private float turnFadeOutTimeStop = 0.1f;
    [SerializeField]
    private float turnFadeOutTime = 1f;
    [SerializeField]
    private float turnFadeInTimeStop = 0.1f;
    [SerializeField]
    private float turnFadeInTime = 1f;
    [SerializeField]
    private float turnEndTime = 0.1f;
    private int fadeState = 0;
    private float turnFlagCount = 0;
    private int turnDirection = 0;
    private float turnTimeCount = 0;
    [SerializeField]
    private TextureNoise tn = null;
    private float totalTurnTime{ get { return turnFadeOutTime + turnFadeOutTimeStop + turnFadeInTime + turnFadeInTimeStop + turnEndTime; } }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = gameManager.Parameter.PlayerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.Parameter.StartGameFlag) return;
        if (gameManager.Parameter.EndGameFlag) return;
        //旋回
        switch (cameraTypeFlag)
        {
            case 0:
                CameraMove1();
                break;
            case 1:
                CameraMove2();
                break;
            case 2:
                CameraMove3();
                break;
        }
        //移動
        Vector3 moveAxis = new Vector3(inputManager.LC.AxisStick.x, 0, inputManager.LC.AxisStick.y) * gameManager.Parameter.MoveSpeed;
        rb.velocity = transform.rotation * moveAxis;
        //コントローラ（腕）の位置
        beamArm.transform.position = transform.rotation * (inputManager.RC.Position + new Vector3(0, 0, 1)) + transform.position;
        beamArm.transform.localRotation = inputManager.RC.Rotation;
        lightArm.transform.position = transform.rotation * (inputManager.LC.Position + new Vector3(0, 0, 1)) + transform.position;
        lightArm.transform.localRotation = inputManager.LC.Rotation;
    }

    void CameraMove1()
    {
        transform.Rotate(Vector3.up, inputManager.RC.AxisStick.x * gameManager.Parameter.TrunSpeed);
    }

    void CameraMove2()
    {
        if (fadeState == 0)
        {
            if (inputManager.LC.HandTrigger.Axis > 0.5f)
            {
                turnDirection = -1;
                fadeState = 1;
                turnFlagCount = turnFadeOutTime;
            }
            if (inputManager.RC.HandTrigger.Axis > 0.5f)
            {
                turnDirection = 1;
                fadeState = 1;
                turnFlagCount = turnFadeOutTime;
            }
        }
        else if (fadeState == 1)
        {
            if (turnFlagCount > 0)
            {
                turnFlagCount = turnFlagCount - Time.deltaTime < 0 ? 0 : turnFlagCount - Time.deltaTime;
                tn.AlphaChanged((1 - turnFlagCount) / turnFadeOutTime);
            }
            else
            {
                transform.Rotate(Vector3.up, 360 * turnDirection / angleVisionCutNumber);
                turnFlagCount = turnFadeInTime;
                fadeState = 2;
            }
        }
        else if (fadeState == 2)
        {
            if (turnFlagCount > 0)
            {
                turnFlagCount = turnFlagCount - Time.deltaTime < 0 ? 0 : turnFlagCount - Time.deltaTime;
                tn.AlphaChanged(turnFlagCount / turnFadeInTime);
            }
            else
            {
                turnDirection = 0;
                fadeState = 3;
            }
        }
        else if (fadeState == 3)
        {
            if (inputManager.LC.HandTrigger.Axis <= 0.1f && inputManager.RC.HandTrigger.Axis <= 0.1f)
            {
                fadeState = 0;
            }
        }
    }

    void CameraMove3()
    {
        switch (fadeState)
        {
            case 0: //トリガー待ち
                if (inputManager.LC.HandTrigger.Axis > 0.5f)
                {
                    turnDirection = -1;
                    fadeState = 1;
                    turnFlagCount = turnFadeOutTimeStop;
                    angleVisionNumber = (angleVisionNumber + angleVisionCutNumber - 1) % angleVisionCutNumber;
                    turnTimeCount = 0;
                }
                if (inputManager.RC.HandTrigger.Axis > 0.5f)
                {
                    turnDirection = 1;
                    fadeState = 1;
                    turnFlagCount = turnFadeOutTimeStop;
                    angleVisionNumber = (angleVisionNumber + 1) % angleVisionCutNumber;
                    turnTimeCount = 0;
                }
                break;
            case 1: //フェイドアウト待ち
                if (turnFlagCount > 0)
                {
                    turnFlagCount = turnFlagCount - Time.deltaTime;
                }
                else
                {
                    turnFlagCount = turnFadeOutTime;
                    fadeState = 2;
                }
                break;
            case 2: //フェイドアウト
                if (turnFlagCount > 0)
                {
                    turnFlagCount = turnFlagCount - Time.deltaTime;
                    tn.AlphaChanged(1 - (turnFlagCount / turnFadeOutTime));
                }
                else
                {
                    turnFlagCount = turnFadeInTimeStop;
                    fadeState = 3;
                }
                break;
            case 3: //フェイドイン待ち
                if (turnFlagCount > 0)
                {
                    turnFlagCount = turnFlagCount - Time.deltaTime;
                }
                else
                {
                    turnFlagCount = turnFadeInTime;
                    fadeState = 4;
                }
                break;
            case 4: //フェイドイン
                if (turnFlagCount > 0)
                {
                    turnFlagCount = turnFlagCount - Time.deltaTime;
                    tn.AlphaChanged(turnFlagCount / turnFadeInTime);
                }
                else
                {
                    turnFlagCount = turnEndTime;
                    fadeState = 5;
                }
                break;
            case 5: //回転終了待ち
                if (turnFlagCount > 0)
                {
                    turnFlagCount = turnFlagCount - Time.deltaTime;
                }
                else
                {
                    fadeState = 6;
                }
                break;
            case 6: //トリガー離されるの待ち
                if (inputManager.LC.HandTrigger.Axis <= 0.1f && inputManager.RC.HandTrigger.Axis <= 0.1f)
                {
                    fadeState = 0;
                }
                break;
        }

        if (0 != fadeState && 5 != fadeState)
        {
            turnTimeCount += Time.deltaTime;
            transform.Rotate(Vector3.up, 360 / angleVisionCutNumber * turnDirection * Time.deltaTime / totalTurnTime);
            if (turnTimeCount >= totalTurnTime)
            {
                transform.rotation = Quaternion.AngleAxis(360 / angleVisionCutNumber * angleVisionNumber, Vector3.up);
            }
        }
    }
}