using UnityEngine;
public class Door : SuperSwitchTarget
{
    [SerializeField] private Animator animator;
    public override void Activate(GameManager gm)
    {
        base.Activate(gm);
        animator.Play("DoorOpen");
    }
}
