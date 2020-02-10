using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
   
    public Text countText;

    void Update()
    {
        countText.text = ((int)(player.GM.Parameter.MaxPlayTime - player.GM.Parameter.CurrentPlayTime)).ToString();
    }
}
