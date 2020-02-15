using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Image redImage = null;
    [SerializeField] private Image whiteImage = null;
    private int state = 0;
    private float redNowAlpha;
    private float redDiffAlpha;
    [SerializeField]private float redMaxAlpha = 0.9f;
    [SerializeField]private float redMinAlpha = 0.1f;
    private float whiteNowAlpha;
    private float whiteDiffAlpha;
    [SerializeField]private float whiteMaxAlpha = 0.3f;
    [SerializeField]private float whiteMinAlpha = 0.1f;
    private float nowTime = 0f;
    [SerializeField]private float maxTime = 1f;

    void Awake()
    {
        redNowAlpha = 0f;
        whiteNowAlpha = 0f;
    }
    void Update()
    {
        switch (state)
        {
            case 1:
                FadeIn();
                break;
            case 2:
                FadeOut();
                break;
        }
    }
    public void IsDamaged()
    {
        state = 1;
    }
    public void FadeIn()
    {
        if (nowTime < maxTime)
        {
            //赤い方
            redDiffAlpha = redMaxAlpha - redMinAlpha;
            float t = nowTime / maxTime;
            redNowAlpha = redMinAlpha + redDiffAlpha * t;
            redImage.color = new Color(1, 1, 1, redNowAlpha);
            //白い方
            whiteDiffAlpha = whiteMaxAlpha - whiteMinAlpha;
            whiteNowAlpha = whiteMinAlpha + whiteDiffAlpha * t;
            whiteImage.color = new Color(1, 1, 1, whiteNowAlpha);
        }
        else
        {
            nowTime = 0;
            state = 2;
        }

    }
    private void FadeOut()
    {
        if (nowTime < maxTime)
        {
            //赤い方
            redDiffAlpha = redMaxAlpha - redMinAlpha;
            float t = 1 - nowTime / maxTime;
            redNowAlpha = redMaxAlpha - redDiffAlpha * t;
            redImage.color = new Color(1, 1, 1, redNowAlpha);
            //白い方
            whiteDiffAlpha = whiteMaxAlpha - whiteMinAlpha;
            whiteNowAlpha = whiteMaxAlpha - whiteDiffAlpha * t;
            whiteImage.color = new Color(1, 1, 1, whiteNowAlpha);
        }
        else
        {
            redImage.color = new Color(1, 1, 1, 0);
            whiteImage.color = new Color(1, 1, 1, 0);
            nowTime = 0;
            state = 0;
        }
    }
}
