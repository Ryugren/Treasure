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

    private void OnCollisionStay(Collision collision)
    {
        {
            if (IsBreaked) return;
            if (collision.gameObject.name == "Foot")
            {
                collision.gameObject.GetComponent<Foot>().Player.Damage(5);
            }
        }
    }
}