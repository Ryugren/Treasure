using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeUI : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
    public Image HpGauge;
   
    void Update()
    {
        HpGauge.fillAmount = player.GM.Parameter.Life / player.GM.Parameter.MaxLife;
    }
}
