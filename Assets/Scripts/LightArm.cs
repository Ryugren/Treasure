using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArm : MonoBehaviour
{
    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private Light lightComponent = null;

    void Update()
    {
        if (inputManager.LC.IndexTrigger.Axis > 0.5f)
        {
            lightComponent.enabled = true;
        }
        else
        {
            lightComponent.enabled = false;
        }
    }
}
