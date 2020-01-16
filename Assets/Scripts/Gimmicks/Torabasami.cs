using UnityEngine;
public class Torabasami : SuperGimmicks
{
    public bool IsBreaked { get; private set; } = false; 
    public override void Activate(GameManager gm)
    {
        if (IsBreaked) return;
        base.Activate(gm);
        IsBreaked = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsBreaked) return;
        if (other.name == "Foot")
        {
            Foot foot = other.GetComponent<Foot>();
            foot.Player.Damage(5);
            foot.Player.Slow();
            IsBreaked = true;
        }
    }
}
