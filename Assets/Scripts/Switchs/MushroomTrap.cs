using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomTrap : SuperSwitchTarget
{
    [SerializeField]
    private GameObject particle = null;
    [SerializeField]
    private GameObject hitter = null;
    private bool flag1 = false;
    public void Awake()
    {
        particle.SetActive(false);
    }
    public override void Activate(GameManager gm)
    {
        if (flag1) return;
        base.Activate(gm);
        particle.SetActive(true);
        hitter.SetActive(true);
        flag1 = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            other.gameObject.GetComponent<DamageHitter>().Player.GM.Parameter.IsBlindfolded = true;
        }
    }
}
