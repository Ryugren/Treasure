using UnityEngine;
public class SawBlade : SuperGimmicks
{
    public bool IsBreaked { get; private set; } = false;
    [SerializeField]
    private Animator moveAnim = null;
    [SerializeField]
    private MeshRenderer mr = null;
    private void Update()
    {
        if (mr.enabled && IsBreaked)
        {
            mr.enabled = false;
        }
        if (!IsBreaked && !moveAnim.GetBool("DTP_AnimTrigger_FloorTraps_SawbladeSlit_StartSawing"))
        {
            moveAnim.SetBool("DTP_AnimTrigger_FloorTraps_SawbladeSlit_StartSawing", true);
        }
    }
    public override void Activate(GameManager gm)
    {
        if (IsBreaked) return;
        base.Activate(gm);
        IsBreaked = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsBreaked) return;
        if (other.gameObject.tag.Contains("Player"))
        {
            other.GetComponent<DamageHitter>().Player.Damage(5);
            IsBreaked = true;
        }
    }
}
