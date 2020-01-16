using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MudUI : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager = null;
    private GameManager.ParameterBase pb { get { return gameManager.Parameter; } }
    private Image mySelfImage = null;
    private float timeCount = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        mySelfImage = GetComponent<Image>();
        mySelfImage.color = new Color(1, 1, 1, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Parameter.IsBlindfolded)
        {
            timeCount = pb.MaxBlindfoldTime;
            mySelfImage.color = new Color(1, 1, 1, pb.MaxAlphaBlindfold);
            gameManager.Parameter.IsBlindfolded = false;
        }
        else if(timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            if (timeCount <= 0)
            {
                mySelfImage.color = new Color(1, 1, 1, 0f);
            } 
            else if (timeCount <= pb.MaxBlindfoldTime - pb.ChangeBlindfoldTime)
            {
                mySelfImage.color = new Color(1, 1, 1, pb.MaxAlphaBlindfold * timeCount / (pb.MaxBlindfoldTime - pb.ChangeBlindfoldTime));
            }
        }
    }
}
