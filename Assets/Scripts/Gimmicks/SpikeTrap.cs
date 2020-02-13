using UnityEngine;
public class SpikeTrap : SuperGimmicks
{
    public bool IsBreaked { get; private set; } = false;
    public override void Activate(GameManager gm)
    {
        if (IsBreaked) return;
        base.Activate(gm);
        IsBreaked = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (IsBreaked) return;
        if (other.gameObject.tag.Contains("Player"))
        {
            other.gameObject.GetComponent<DamageHitter>().Player.Damage(5);
        }
    }
}