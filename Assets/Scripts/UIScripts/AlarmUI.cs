using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlarmUI : MonoBehaviour
{
    //何秒後に着くか
    [SerializeField]
    private float returnTime = 5f;
    private float timeCount = 0f;
    private int isReversed = 1;
    private Image image;
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private float updateTime = 30f;
    private float currentTime { get { return player.GM.Parameter.MaxPlayTime - player.GM.Parameter.CurrentPlayTime; } }
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!image.enabled && currentTime <= updateTime)
        {
            image.enabled = true;
        }
        if (image.enabled)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, timeCount / returnTime);
            timeCount += Time.deltaTime * isReversed;
            if (isReversed == -1)
            {
                if (timeCount <= 0)
                {
                    timeCount = 0;
                    isReversed = 1;
                }
            }
            else if (isReversed == 1)
            {
                if (timeCount >= returnTime)
                {
                    timeCount = returnTime;
                    isReversed = -1;
                }
            }
        }
    }
}
