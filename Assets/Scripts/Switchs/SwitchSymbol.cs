using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSymbol : MonoBehaviour
{
    [SerializeField]
    private SuperSwitchTarget ssTarget = null;
    public bool IsBreaked { get; private set; } = false;
    // Start is called before the first frame update
    public void Activate(GameManager gm)
    {
        if (IsBreaked) return;
        ssTarget.Activate(gm);
        IsBreaked = true;
    }
}
