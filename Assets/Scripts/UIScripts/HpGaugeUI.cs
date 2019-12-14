using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeUI : MonoBehaviour
{
    [SerializeField]
    private GameManager GM = null;
    public Image HpGauge;
   
    void Update()
    {
        HpGauge.fillAmount = GM.Parameter.Life / GM.Parameter.MaxLife;
    }
}
