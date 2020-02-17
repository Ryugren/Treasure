using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObserver : MonoBehaviour
{
    [SerializeField]
    protected GameManager GM = null;
    protected GameManager.ParameterBase Parameter { get { return GM.Parameter; } }
    [SerializeField]
    protected InputManager input = null;
    public virtual void Action()
    {
        BeamEnergyRecharge();
    }
    public virtual void CountTimer()
    {
        Parameter.CurrentPlayTime += Time.deltaTime;
        if (Parameter.MaxPlayTime - Parameter.CurrentPlayTime <= 0)
        {
            Parameter.EndGameFlag = true;
            Parameter.CurrentPlayTime = Parameter.MaxPlayTime;
        }
    }
    /// <summary>
    /// ゲームスタート
    /// </summary>
    public virtual void GameStart()
    {
    }
    public virtual void GameOver()
    {
    }
    public virtual void GameEnd()
    {
    }
    /// <summary>
    /// ビームの再装填
    /// </summary>
    private void BeamEnergyRecharge()
    {
        if (Parameter.IsRecharged)
        {
            Parameter.BeamEnergy += Time.deltaTime / Parameter.MaxBeamEnergyRechargeTime;
            if (Parameter.BeamEnergy >= 1f)
            {
                Parameter.IsRecharged = false;
                Parameter.BeamEnergy = 1f;
            }
        }
    }
}
