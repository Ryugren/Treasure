using UnityEngine;
public class SpikeTrap : SuperGimmicks
{
    public bool IsBreaked { get; private set; } = false;

    private RaycastHit hit;
    private int mask;
    [SerializeField]
    private Animator anim = null;
    private void Awake()
    {
        mask = ~(1 << LayerMask.NameToLayer("Cave"));
    }

    public override void Activate(GameManager gm)
    {
        if (IsBreaked) return;
        base.Activate(gm);
        IsBreaked = true;
    }
    private void Update()
    {
        if (Physics.BoxCast(transform.position, Vector3.one * 3f, transform.right, out hit, Quaternion.identity, 20f, mask))
        {
            if (hit.collider.tag.Contains("Player"))
            {
                if (anim.GetBool("Flag")) return;
                Debug.Log(hit.collider.name);
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
    private void OnCollisionStay(Collision collision)
    {
        {
            if (IsBreaked) return;
            if (collision.gameObject.tag.Contains("Player"))
            {
                collision.gameObject.GetComponent<DamageHitter>().Player.Damage(5);
            }
        }
    }
}