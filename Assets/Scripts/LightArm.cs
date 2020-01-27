using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArm : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private Light lightComponent = null;
    private bool releaseFlag = true;

    //[SerializeField] private float distance = 10.5f;
    [SerializeField] private GameObject parentObject = null;
    private Ray ray;
    private RaycastHit hit;
    private int mask;

    void Awake()
    {
        mask = LayerMask.GetMask("Key");
    }

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
        else if (inputManager.LC.IndexTrigger.GetUp)
        {
            releaseFlag = true;
        }
        if (lightComponent.enabled)
        {
            LightFiring();
        }
    }
    /// <summary>
    /// レイを発射
    /// </summary>
    void LightFiring()
    {
        ray = new Ray(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward);
        if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            if (hit.collider.tag == "Key")
            {
                SwitchSymbol sg = hit.collider.GetComponent<SwitchSymbol>();
                sg.Activate(gameManager);
            }
            //hit.collider.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        Debug.DrawRay(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward * float.MaxValue, Color.red);
    }
}