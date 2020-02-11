using UnityEngine;
public class Torabasami : SuperGimmicks
{
    public bool IsBreaked { get; private set; } = false;
    [SerializeField]
    private Animator animator = null;
    public override void Activate(GameManager gm)
    {
        if (IsBreaked) return;
        base.Activate(gm);
        IsBreaked = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsBreaked) return;
        if (other.gameObject.tag.Equals("Player"))
        {
            DamageHitter hitter = other.GetComponent<DamageHitter>();
            hitter.Player.Damage(5);
            hitter.Player.Slow();
            animator.SetBool("Hit", true);
            IsBreaked = true;
        }
    }
}
