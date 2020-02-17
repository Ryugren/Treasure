using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Image damageImage = null;
    private int state = 0;
    private float damageNowAlpha;
    private float damageDiffAlpha;
    [SerializeField, Range(0, 1)] private float damageMaxAlpha = 0.9f;
    [SerializeField, Range(0, 1)] private float damageMinAlpha = 0.1f;
    private float nowTime = 0f;
    [SerializeField] private float maxTime = 1f;
    void Awake()
    {
        damageNowAlpha = 0f;
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
            float t = nowTime / maxTime;
            //赤い方
            damageDiffAlpha = damageMaxAlpha - damageMinAlpha;
            damageNowAlpha = damageMinAlpha + damageDiffAlpha * t;
            damageImage.color = new Color(1, 1, 1, damageNowAlpha);
            nowTime += Time.deltaTime;
        }
        else
        {
            damageImage.color = new Color(1, 1, 1, damageMaxAlpha);
            nowTime = 0;
            state = 2;
        }
    }
    private void FadeOut()
    {
        if (nowTime < maxTime)
        {
            float t = 1 - nowTime / maxTime;
            //赤い方
            damageDiffAlpha = damageMaxAlpha - damageMinAlpha;
            damageNowAlpha = damageMaxAlpha + damageDiffAlpha * t;
            damageImage.color = new Color(1, 1, 1, damageNowAlpha);
            nowTime += Time.deltaTime;
        }
        else
        {
            damageImage.color = new Color(1, 1, 1, 0);
            nowTime = 0;
            state = 0;
        }
    }
}