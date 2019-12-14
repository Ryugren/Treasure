using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private GameManager GM = null;
   
    public Text countText;
    //private int minutes;
    //private float CT;
    void Start()
    {
        //CT = PB.MaxPlayTime;
    }

    void Update()
    {
        //CT -= Time.deltaTime;
        //minutes = (int)CT;
        countText.text = (int)(GM.Parameter.MaxPlayTime - GM.Parameter.CurrentPlayTime) + " S".ToString();
    }
}
