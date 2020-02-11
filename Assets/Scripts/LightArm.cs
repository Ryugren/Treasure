using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArm : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private Light lightComponent = null;
    private bool releaseFlag = true;
    private Ray ray;
    private RaycastHit hit;
    private int mask;
    [SerializeField]
    private AudioSource lightSE = null;
    void Awake()
    {
        mask = LayerMask.GetMask("Key");
    }

    void Update()
    {
        if (player.Input.LC.IndexTrigger.Axis > 0.5f)
        {
            if (releaseFlag)
            {
                lightComponent.enabled = !lightComponent.enabled;
                releaseFlag = false;
                lightSE.Play();
            }
        }
        else if (player.Input.LC.IndexTrigger.GetUp)
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
        if (!player.GM.Parameter.StartGameFlag || player.GM.Parameter.EndGameFlag) return;
        ray = new Ray(transform.position, transform.rotation * Vector3.forward);
        if (Physics.Raycast(ray, out hit, lightComponent.range, mask))
        {
            if (hit.collider.tag == "Key")
            {
                SwitchSymbol sg = hit.collider.GetComponent<SwitchSymbol>();
                sg.Activate(player.GM);
            }
        }
        Debug.DrawRay(transform.position, transform.rotation * Vector3.forward * float.MaxValue, Color.red);
    }
}