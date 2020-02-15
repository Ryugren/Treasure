using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ビーム、右アーム
    /// </summary>
    [SerializeField]
    private GameObject beamController = null;
    /// <summary>
    /// ライト、左アーム
    /// </summary>
    [SerializeField]
    private GameObject lightController = null;
    [SerializeField]
    private GameManager gameManager = null;
    public GameManager GM { get { return gameManager; } }
    [SerializeField]
    private InputManager inputManager = null;
    public InputManager Input { get { return inputManager; } }
    [SerializeField]
    private Rigidbody rb = null;
    private float damageTimeCount = 0f;
    [SerializeField]
    private int angleVisionCutNumber = 4;
    private int angleVisionNumber = 0;
    private int fadeState = 0;
    private float turnFlagCount = 0;
    private int turnDirection = 0;
    private float turnTimeCount = 0;
    [SerializeField]
    private TextureNoise[] tns = null;
    private float turnFadeOutTime { get { return gameManager.Parameter.TurnTime.FadeOut; } }
    private float turnFadeOutTimeStop { get { return gameManager.Parameter.TurnTime.FadeOutStop; } }
    private float turnFadeInTime { get { return gameManager.Parameter.TurnTime.FadeIn; } }
    private float turnFadeInTimeStop { get { return gameManager.Parameter.TurnTime.FadeInStop; } }
    private float turnEndTime { get { return gameManager.Parameter.TurnTime.End; } }
    private float totalTurnTime { get { return turnFadeOutTime + turnFadeOutTimeStop + turnFadeInTime + turnFadeInTimeStop + turnEndTime; } }
    // Start is called before the first frame update
    [SerializeField]
    private AudioSource damageSE = null;
    [SerializeField]
    private AudioSource shortNoiseSE = null;
    [SerializeField]
    private DamageFlash df = null;
    // Update is called once per frame
    private void Awake()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        angleVisionNumber = angleVisionCutNumber / 2;
        transform.rotation = Quaternion.AngleAxis(360 / angleVisionCutNumber * angleVisionNumber, Vector3.up);
    }
    void Update()
    {
        if (rb.constraints == RigidbodyConstraints.FreezeAll && gameManager.Parameter.StartGameFlag)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            return;
        }
        if (damageTimeCount > 0)
        {
            damageTimeCount -= Time.deltaTime;
        }
        //旋回
        CameraMove();
        //移動
        Vector3 moveAxis = new Vector3(inputManager.LC.AxisStick.x, 0, inputManager.LC.AxisStick.y) * gameManager.Parameter.MoveSpeed;
        rb.velocity = transform.rotation * moveAxis;
        //コントローラ（腕）の位置
        beamController.transform.position = transform.rotation * (inputManager.RC.Position + new Vector3(gameManager.Parameter.HandPosition.x, gameManager.Parameter.HandPosition.y, gameManager.Parameter.HandPosition.z)) + transform.position;
        beamController.transform.localRotation = inputManager.RC.Rotation;
        lightController.transform.position = transform.rotation * (inputManager.LC.Position + new Vector3(-gameManager.Parameter.HandPosition.x, gameManager.Parameter.HandPosition.y, gameManager.Parameter.HandPosition.z)) + transform.position;
        lightController.transform.localRotation = inputManager.LC.Rotation;
        //テスト処理
    }
    void CameraMove()
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
                    shortNoiseSE.Play();
                }
                else if (inputManager.RC.HandTrigger.Axis > 0.5f)
                {
                    turnDirection = 1;
                    fadeState = 1;
                    turnFlagCount = turnFadeOutTimeStop;
                    angleVisionNumber = (angleVisionNumber + 1) % angleVisionCutNumber;
                    turnTimeCount = 0;
                    shortNoiseSE.Play();
                }
                break;
            case 1: //フェイドアウト待ち
                if (turnFlagCount > 0)
                {
                    turnFlagCount -= Time.deltaTime;
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
                    turnFlagCount -= Time.deltaTime;
                    foreach (TextureNoise it in tns) 
                    {
                        it.AlphaChanged(1 - (turnFlagCount / turnFadeOutTime));
                    }
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
                    turnFlagCount -= Time.deltaTime;
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
                    turnFlagCount -= Time.deltaTime;
                    foreach (TextureNoise it in tns)
                    {
                        it.AlphaChanged(turnFlagCount / turnFadeInTime);
                    }
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
                    turnFlagCount -= Time.deltaTime;
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
    public void Damage(float value)
    {
        if (damageTimeCount > 0) return;
        gameManager.Damage(value);
        damageTimeCount = 1f;
        damageSE.Play();
        df.IsDamaged();
    }
    public void Slow()
    {
        gameManager.Parameter.SlowMoveFlag = true;
    }
}