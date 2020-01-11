using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamArm : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private float distance = 10.5f;
    [SerializeField] private GameObject parentObject = null;
    [SerializeField] private GameObject childrenObject = null;
    private bool raycastFlag = false;

    private Ray ray;
    private RaycastHit hit;

    void Awake()
    {
        childrenObject.SetActive(false);
    }
    void Start()
    {
        ray = new Ray(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward);
    }

    void Update()
    {
        //起動
        if (inputManager.RC.IndexTrigger.Axis > 0.5f && gameManager.Parameter.BeamFlag == true)
        {
            gameManager.UseEnergy();

            childrenObject.SetActive(true);
            raycastFlag = true;
            BeamFiring();
        }
        //起動しない
        else
        {
            childrenObject.SetActive(false);
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
        if (Physics.Raycast(ray, out hit, distance))
        {
            if(hit.collider.tag == "Gimmick")
            {
                SuperGimmicks sg = hit.collider.GetComponent<SuperGimmicks>();
                sg.Activate(gameManager);
            }
            //hit.collider.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        Debug.DrawRay(parentObject.transform.position, parentObject.transform.rotation * Vector3.forward * distance, Color.red);
    }
}
