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
    private Player player = null;
    public Image BeamUiobj;

    void Update()
    {
        BeamUiobj.fillAmount = player.GM.Parameter.BeamEnergy;
    }
}
