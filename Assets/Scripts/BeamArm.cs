using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamArm : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float distance = 10.5f;
    private Ray ray;
    private RaycastHit hit;
    private GameObject parentObject;

    void Start()
    {
        parentObject = transform.parent.gameObject;
        ray = new Ray(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward);
    }

    void Update()
    {
        if (inputManager.RC.HandTrigger.Get)
        {
            if (inputManager.RC.HandTrigger.Axis > 0.5f)
            {
                if (gameManager.BeamEnergyIsUsed() == true)
                {
                    parentObject.SetActive(true);
                    BeamFiring();
                }
            }
        }
    }
    /// <summary>
    /// レイを発射
    /// </summary>
    void BeamFiring()
    {
        if (Physics.Raycast(ray, out hit, distance))
        {
            hit.collider.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        Debug.DrawRay(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward * distance, Color.red);
    }
}
