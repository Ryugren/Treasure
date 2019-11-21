using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// 
/// Aボタン押したらビームゲージが削られ使い切ったらゲージ回復,
/// かつ回復中はビームゲージは削られない(ビームが打てない)。
///


public class BeamGaugeUI : MonoBehaviour
{

    GameManager.ParameterBase PB = new GameManager.ParameterBase();
    public float amountcount;
    public Image BeamUiobj;
    public bool triger;
    
    void Update()
    {   
        
        if (Input.GetKey(KeyCode.A))
        {
            amountcount = BeamUiobj.fillAmount;
            if (triger)　{ BeamUiobj.fillAmount -= 1.0f / PB.MaxBeamLimitTime * Time.deltaTime;　}
        }
        if(BeamUiobj.fillAmount == 0)
        {　triger = false;　}

        if (triger == false)
        { BeamUiobj.fillAmount += 1.0f / PB.MaxBeamEnergyRechargeTime * Time.deltaTime;　}
        if (BeamUiobj.fillAmount == 1)
        {　triger = true;　}
    }
}
