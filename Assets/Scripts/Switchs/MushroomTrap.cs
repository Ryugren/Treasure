using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomTrap : SuperSwitchTarget
{
    [SerializeField]
    private GameObject particle = null;
    public void Awake()
    {
        particle.SetActive(false);
    }
    public override void Activate(GameManager gm)
    {
        base.Activate(gm);
        particle.SetActive(true);
    }
}
