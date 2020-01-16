using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudTrap : SuperSwitchTarget
{
    public override void Activate(GameManager gm)
    {
        base.Activate(gm);
        gm.Parameter.IsBlindfolded = true;
    }
}
