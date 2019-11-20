using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ビーム、右アーム
    /// </summary>
    [SerializeField]
    private GameObject beamArm = null;
    /// <summary>
    /// ライト、左アーム
    /// </summary>
    [SerializeField]
    private GameObject lightArm = null;
    [SerializeField]
    private InputManager inputManager = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //コントローラ（腕）の位置
        beamArm.transform.position = inputManager.RC.Position;
        lightArm.transform.position = inputManager.LC.Position;
    }
}
