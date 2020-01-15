using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSymbol : MonoBehaviour
{
    [SerializeField]
    private SuperSwitchTarget ssTarget = null;
    // Start is called before the first frame update
    public void Activate(GameManager gm)
    {
        ssTarget.Activate(gm);
    }
}
