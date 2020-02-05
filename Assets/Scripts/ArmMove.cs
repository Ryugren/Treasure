using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject targetObject = null;
    [SerializeField]
    private GameObject hand = null;
    [SerializeField]
    private Vector3 handAddAngle = Vector3.zero;
    [SerializeField]
    private GameObject arm = null;
    [SerializeField]
    private Vector3 armAddAngle = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        arm.transform.LookAt(targetObject.transform, Vector3.up);
        arm.transform.Rotate(armAddAngle);
        hand.transform.localEulerAngles = -arm.transform.localEulerAngles + new Vector3(-targetObject.transform.localEulerAngles.z, targetObject.transform.localEulerAngles.y, targetObject.transform.localEulerAngles.x);
        hand.transform.Rotate(handAddAngle);
    }
}
