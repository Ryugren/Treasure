using UnityEngine;
public class BearTrap : SuperGimmicks
{
    public bool IsBreaked { get; private set; } = false;
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private SkinnedMeshRenderer mr = null;
    private void Update()
    {
        if (mr.enabled && IsBreaked)
        {
            mr.enabled = false;
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
            DamageHitter hitter = other.GetComponent<DamageHitter>();
            hitter.Player.Damage(5);
            //hitter.Player.Slow();
            animator.SetBool("Hit", true);
            IsBreaked = true;
        }
    }
}
