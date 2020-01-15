using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArm : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private Light lightComponent = null;
    private bool releaseFlag = true;

    [SerializeField] private float distance = 10.5f;
    [SerializeField] private GameObject parentObject = null;
    private Ray ray;
    private RaycastHit hit;
    void Start()
    {
        ray = new Ray(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward);
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
        else if(inputManager.LC.IndexTrigger.GetUp)
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
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag == "Gimmick")
            {
                SuperGimmicks sg = hit.collider.GetComponent<SuperGimmicks>();
                sg.Activate(gameManager);
            }
            //hit.collider.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        Debug.DrawRay(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward * distance, Color.red);
    }
}
