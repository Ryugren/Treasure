using UnityEngine;
public class Door : SuperSwitchTarget
{
    [SerializeField] private Animator animator = null;
    public override void Activate(GameManager gm)
    {
        base.Activate(gm);
        animator.SetBool("Flag",true);
    }
}
