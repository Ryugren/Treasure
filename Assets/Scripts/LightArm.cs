using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArm : MonoBehaviour
{
    [SerializeField] private Player player = null;
    [SerializeField] private Light lightComponent = null;
    private bool releaseFlag = true;
    private RaycastHit hit;
    private int mask;
    [SerializeField]
    private AudioSource lightSE = null;
    void Awake()
    {
        mask = LayerMask.GetMask("Key");
        lightComponent.range = player.GM.Parameter.LightRange;
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
        if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, player.GM.Parameter.LightHitRange, mask))
        {
            if (hit.collider.tag.Contains("Key"))
            {
                SwitchSymbol sg = hit.collider.GetComponent<SwitchSymbol>();
                sg.Activate(player.GM, 0);
            }
        }
        if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, player.GM.Parameter.LightHit2Range, mask))
        {
            if (hit.collider.tag.Contains("Key"))
            {
                SwitchSymbol sg = hit.collider.GetComponent<SwitchSymbol>();
                sg.Activate(player.GM, 1);
            }
        }
    }
    void OnDrawGizmos()
    {
        //　Capsuleのレイを疑似的に視覚化
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hit.point, 1);
    }
}