using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeUI : MonoBehaviour
{
    GameManager.ParameterBase PB = new GameManager.ParameterBase();
    public Image HpGauge;
   
    void Update()
    {
        HpGauge.fillAmount = PB.Life / PB.MaxLife;
    }
}
