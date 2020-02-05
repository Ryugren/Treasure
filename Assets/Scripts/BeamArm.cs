using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamArm : MonoBehaviour
{
    [SerializeField] private Player player = null;
    //[SerializeField] private float distance = 10.5f;
    [SerializeField] private GameObject parentObject = null;
    [SerializeField] private ParticleSystem childrenParticle = null;
    private bool raycastFlag = false;

    private Ray ray;
    private RaycastHit hit;
    private int mask;

    void Awake()
    {
        mask = LayerMask.GetMask("Gimmick");
        childrenParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    void Update()
    {
        if (!player.GM.Parameter.StartGameFlag || player.GM.Parameter.EndGameFlag) return;
        //起動
        if (player.Input.RC.IndexTrigger.Axis > 0.5f && player.GM.Parameter.BeamFlag == true)
        {
            player.GM.UseEnergy();

            childrenParticle.Play(true);
            raycastFlag = true;
            BeamFiring();
        }
        //起動しない
        else
        {
            childrenParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            raycastFlag = false;
        }
        //罠チェック
        if (raycastFlag == true)
        {
            BeamFiring();
        }
    }
    /// <summary>
    /// レイを発射
    /// </summary>
    void BeamFiring()
    {
        ray = new Ray(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward);
        if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            if (hit.collider.tag == "Gimmick")
            {
                SuperGimmicks sg = hit.collider.GetComponent<SuperGimmicks>();
                sg.Activate(player.GM);
            }
            //hit.collider.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        Debug.DrawRay(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward * float.MaxValue, Color.red);
    }
}