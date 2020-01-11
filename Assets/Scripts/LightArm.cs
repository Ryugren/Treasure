using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArm : MonoBehaviour
{
    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private Light lightComponent = null;
    private bool releaseFlag = true;

    void Update()
    {
        if (inputManager.LC.IndexTrigger.Axis > 0.5f)
        {
            if (releaseFlag)
            {
                lightComponent.enabled = !lightComponent.enabled;
                releaseFlag = false;
            }
        }
        else if(inputManager.LC.IndexTrigger.GetUp)
        {
            releaseFlag = true;
        }
    }
}
