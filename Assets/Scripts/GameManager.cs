using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BaseObserver observer = null;
    [SerializeField]
    private ParameterBase parameter = null;
    /// <summary>
    /// ゲームのパラメーター
    /// </summary>
    public ParameterBase Parameter { get { return parameter; } }
    private void Awake()
    {
        Application.targetFrameRate = 72;
    }
    // Start is called before the first frame update
    void Start()
    {
        //初期化
        parameter.Life = parameter.MaxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Parameter.StartGameFlag)
        {
            observer.GameStart();
        }
        if (Parameter.EndGameFlag)
        {
            observer.GameEnd();
        }
        if (!parameter.StartGameFlag || parameter.EndGameFlag) return;
        observer.GameOver();
        observer.CountTimer();
        observer.Action();
        parameter.SlowCountTimer();
    }
    public void Damage(float amount)
    {
        parameter.Life -= amount;
    }
    /// <summary>
    /// ビームエネルギーを使う
    /// </summary>
    /// <returns></returns>
    public void UseEnergy()
    {
        if (parameter.BeamEnergy > 0)
        {
            parameter.BeamEnergy -= Time.deltaTime / parameter.MaxBeamLimitTime;
        }
        else
        {
            parameter.BeamEnergy = 0;
            parameter.IsRecharged = true;
        }
    }
    [System.Serializable]
    public class TurnTimes
    {
        [SerializeField, Tooltip("フェイドアウト開始の待ち時間")]
        private float fadeOutStop = 0.1f;
        public float FadeOutStop { get { return fadeOutStop; } }
        [SerializeField, Tooltip("フェイドアウト開始からの必要時間")]
        private float fadeOut = 0.25f;
        public float FadeOut { get { return fadeOut; } }
        [SerializeField, Tooltip("フェイドイン開始の待ち時間")]
        private float fadeInStop = 0.5f;
        public float FadeInStop { get { return fadeInStop; } }
        [SerializeField, Tooltip("フェイドイン開始からの必要時間")]
        private float fadeIn = 0.25f;
        public float FadeIn { get { return fadeIn; } }
        [SerializeField, Tooltip("フェイドイン終了後の経過時間")]
        private float end = 0.1f;
        public float End { get { return end; } }
    }
    [System.Serializable]
    public class ParameterBase
    {
        [SerializeField, Tooltip("移動速度")]
        private float moveSpeed = 5;
        /// <summary>
        /// 移動速度
        /// </summary>
        public float MoveSpeed
        {
            get
            {
                if (slowTimeCount <= 0)
                {
                    return moveSpeed;
                }
                else if (slowTimeCount <= maxSlowTime)
                {
                    return moveSpeed * SlowMagnification;
                }
                else
                {
                    return 0;
                }
            }
        }

        [SerializeField, Tooltip("旋回にかかる時間")]
        private TurnTimes turnTime = null;
        /// <summary>
        /// 旋回にかかる時間
        /// </summary>
        public TurnTimes TurnTime { get { return turnTime; } }

        [SerializeField, Tooltip("プレイ時間")]
        private float maxPlayTime = 180f;
        /// <summary>
        /// プレイ時間
        /// </summary>
        public float MaxPlayTime { get { return maxPlayTime; } }

        [SerializeField, Tooltip("最大体力")]
        private float maxLife = 100;
        /// <summary>
        /// 最大体力
        /// </summary>
        public float MaxLife { get { return maxLife; } }

        [SerializeField, Tooltip("ビーム継続時間")]
        private float maxBeamLimitTime = 3f;
        /// <summary>
        /// ビーム継続時間
        /// </summary>
        public float MaxBeamLimitTime { get { return maxBeamLimitTime; } }

        [SerializeField, Tooltip("ビーム再装填時間")]
        private float maxBeamEnergyRechargeTime = 5f;
        /// <summary>
        /// ビーム再装填時間
        /// </summary>
        public float MaxBeamEnergyRechargeTime { get { return maxBeamEnergyRechargeTime; } }

        [SerializeField, Tooltip("鈍足継続時間")]
        private float maxStopTime = 5f;
        /// <summary>
        /// 鈍足停止時間
        /// </summary>
        public float MaxStopTime { get { return maxStopTime; } }

        [SerializeField, Tooltip("鈍足継続時間")]
        private float maxSlowTime = 5f;
        /// <summary>
        /// 鈍足継続時間
        /// </summary>
        public float MaxSlowTime { get { return maxSlowTime; } }

        [SerializeField, Range(0f, 1f), Tooltip("鈍足時の減速率")]
        private float slowMagnification = 0.75f;
        /// <summary>
        /// 鈍足時の減速率
        /// </summary>
        public float SlowMagnification { get { return slowMagnification; } }

        /// <summary>
        /// 鈍足現在時間
        /// </summary>
        private float slowTimeCount = 0f;

        [SerializeField, Tooltip("視界妨害時間")]
        private float maxBlindfoldTime = 5f;
        /// <summary>
        /// 視界妨害時間
        /// </summary>
        public float MaxBlindfoldTime { get { return maxBlindfoldTime; } }

        [SerializeField, Tooltip("アルファ値変化開始時間")]
        private float changeBlindfoldTime = 5f;
        /// <summary>
        /// 視界妨害のアルファ値変化時間
        /// </summary>
        public float ChangeBlindfoldTime { get { return changeBlindfoldTime; } }

        [SerializeField, Range(0f, 1f), Tooltip("視界妨害の泥のアルファ値")]
        private float maxAlphaBlindfold = 0.5f;
        /// <summary>
        /// 視界妨害の泥の最大アルファ値
        /// </summary>
        public float MaxAlphaBlindfold { get { return maxAlphaBlindfold; } }

        /// <summary>
        /// 現在体力
        /// </summary>
        public float Life { get; set; }

        /// <summary>
        /// 経過時間
        /// </summary>
        public float CurrentPlayTime { get; set; }

        [SerializeField, Tooltip("ライトの距離")]
        private float lightRange = 100f;
        /// <summary>
        /// ライト距離
        /// </summary>
        public float LightRange { get { return lightRange; } }

        [SerializeField, Range(0f, 1f), Tooltip("ライトの当たる距離1")]
        private float lightHitRange = 0.1f;
        /// <summary>
        /// ライト反応距離1
        /// </summary>
        public float LightHitRange { get { return lightRange * lightHitRange; } }

        [SerializeField, Range(0f, 1f), Tooltip("ライト反応距離2（胞子が出る距離）")]
        private float lightHit2Range = 0.3f;
        /// <summary>
        /// ライト反応距離2（胞子が出る距離）
        /// </summary>
        public float LightHit2Range { get { return lightRange * lightHit2Range; } }

        [SerializeField]
        private Vector3 playerPosition = Vector3.zero;
        public Vector3 PlayerPosition { get { return playerPosition; } }
        [SerializeField]
        private Vector3 handPosition = Vector3.zero;
        public Vector3 HandPosition
        {
            get { return handPosition; }
        }
        /// <summary>
        /// 泥フラグ
        /// </summary>
        public bool IsBlindfolded { get; set; } = false;
        /// <summary>
        /// 低速フラグ
        /// </summary>
        public bool SlowMoveFlag { get; set; } = false;
        [SerializeField]
        private Player player = null;
        /// <summary>
        /// プレイヤーの向き
        /// </summary>
        public float Direction { get { return player.transform.rotation.y; } }
        /// <summary>
        /// エネルギー量
        /// </summary>
        public float BeamEnergy { get; set; } = 1f;
        public bool IsRecharged { get; set; } = false;
        /// <summary>
        /// ゲームが始まったかどうか
        /// </summary>
        public bool StartGameFlag { get; set; } = false;
        /// <summary>
        /// ビーム撃てるかのフラグ
        /// </summary>
        public bool BeamFlag { get { return !IsRecharged; } }
        /// <summary>
        /// ゲームが終わったかどうか
        /// </summary>
        public bool EndGameFlag { get; set; } = false;
        public void SlowCountTimer()
        {
            if (SlowMoveFlag)
            {
                slowTimeCount = MaxSlowTime + MaxStopTime;
                SlowMoveFlag = false;
            }
            else if (slowTimeCount > 0)
            {
                slowTimeCount -= Time.deltaTime;
            }
        }
    }
}