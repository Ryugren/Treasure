using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapMover : MonoBehaviour
{
    [SerializeField]
    private SpikeTrap parent = null;

    private RaycastHit hit;
    private int mask;
    [SerializeField]
    private Animator anim = null;
    private void Awake()
    {
        mask = ~(1 << LayerMask.NameToLayer("Cave"));
    }
    private void Update()
    {
        if (!parent.IsBreaked && Physics.BoxCast(transform.position, Vector3.one * 3f, transform.right, out hit, Quaternion.identity, 20f, mask))
        {
            if (hit.collider.tag.Contains("Player"))
            {
                if (anim.GetBool("Flag")) return;
                anim.SetBool("Flag", true);
                return;
            }
        }
        if (!anim.GetBool("Flag")) return;
        anim.SetBool("Flag", false);
    }
    void OnDrawGizmos()
    {
        //　Cubeのレイを疑似的に視覚化
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(hit.point, Vector3.one * 3f);
    }
}
