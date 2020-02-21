using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSymbol : MonoBehaviour
{
    [System.Serializable]
    public class Parameter
    {
        [SerializeField]
        private SuperSwitchTarget ssTarget = null;
        public SuperSwitchTarget SSTarget { get{ return ssTarget; } }
        public bool IsBreaked { get; set; } = false;
    }
    [SerializeField]
    private Parameter[] parameters = null;
    public Parameter[] Parameters { get { return parameters; } }
    // Start is called before the first frame update
    public void Activate(GameManager gm, int number)
    {
        if (number >= parameters.Length) return;
        if (parameters[number].IsBreaked) return;
        parameters[number].SSTarget.Activate(gm);
        parameters[number].IsBreaked = true;
    }
}
