using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MudUI : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
    public GameManager.ParameterBase PB { get { return player.GM.Parameter; } }
    [SerializeField]
    private Image[] mySelfImages = null;
    private float timeCount = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < mySelfImages.Length; ++i)
        {
            mySelfImages[i].color = new Color(1, 1, 1, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GM.Parameter.IsBlindfolded)
        {
            timeCount = PB.MaxBlindfoldTime;
            for (int i = 0; i < mySelfImages.Length; ++i)
            {
                mySelfImages[i].color = new Color(1, 1, 1, PB.MaxAlphaBlindfold);
            }
            player.GM.Parameter.IsBlindfolded = false;
        }
        else if(timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0)
            {
                for (int i = 0; i < mySelfImages.Length; ++i)
                {
                    mySelfImages[i].color = new Color(1, 1, 1, 0f);
                }
            } 
            else if (timeCount <= PB.MaxBlindfoldTime - PB.ChangeBlindfoldTime)
            {
                for (int i = 0; i < mySelfImages.Length; ++i)
                {
                    mySelfImages[i].color = new Color(1, 1, 1, PB.MaxAlphaBlindfold * timeCount / (PB.MaxBlindfoldTime - PB.ChangeBlindfoldTime));
                }
            }
        }
    }
}
