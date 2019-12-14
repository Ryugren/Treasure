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

    [SerializeField]
    private GameManager GM = null;
    //public float amountcount;
    public Image BeamUiobj;
    //public bool triger;

    void Update()
    {
        BeamUiobj.fillAmount = GM.Parameter.BeamEnergy;
        /*
        amountcount = BeamUiobj.fillAmount;
        if (triger) { BeamUiobj.fillAmount -= 1.0f / GM.Parameter.MaxBeamLimitTime * Time.deltaTime; }
        if (BeamUiobj.fillAmount == 0)
        { triger = false; }

        if (triger == false)
        { BeamUiobj.fillAmount += 1.0f / GM.Parameter.MaxBeamEnergyRechargeTime * Time.deltaTime; }
        if (BeamUiobj.fillAmount == 1)
        { triger = true; }*/
    }
}
