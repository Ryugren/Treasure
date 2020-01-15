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
            other.GetComponent<Foot>().Player.Damage(5);
        }
    }
}
