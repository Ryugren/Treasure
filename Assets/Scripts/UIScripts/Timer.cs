using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
   
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
       // minutes = (int)(GM.Parameter.MaxPlayTime - GM.Parameter.CurrentPlayTime);
        countText.text = ((int)(player.GM.Parameter.MaxPlayTime - player.GM.Parameter.CurrentPlayTime)).ToString();
    }
}
