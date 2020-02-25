using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSymbol : MonoBehaviour
{
    [System.Serializable]
    public class BaseParameter
    {
        [SerializeField]
        private SuperSwitchTarget ssTarget = null;
        public SuperSwitchTarget SSTarget { get{ return ssTarget; } }
        public bool IsBreaked { get; set; } = false;
    }
    [SerializeField]
    private BaseParameter parameter = null;
    public BaseParameter Parameter { get { return parameter; } }
    // Start is called before the first frame update
    public void Activate(GameManager gm)
    {
        if (parameter.IsBreaked) return;
        parameter.SSTarget.Activate(gm);
        parameter.IsBreaked = true;
    }
}