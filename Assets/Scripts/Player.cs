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
    private GameManager gameManager = null;
    [SerializeField]
    private InputManager inputManager = null;
    [SerializeField]
    private Rigidbody rb = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //コントローラ（腕）の位置
        beamArm.transform.position = inputManager.RC.Position;
        beamArm.transform.rotation = inputManager.RC.Rotation * transform.rotation;
        lightArm.transform.position = inputManager.LC.Position;
        lightArm.transform.rotation = inputManager.LC.Rotation * transform.rotation;
        //旋回
        transform.Rotate(Vector3.up, inputManager.RC.AxisStick.x * gameManager.Parameter.TrunSpeed);
        //移動
        Vector3 moveAxis = new Vector3(inputManager.LC.AxisStick.x, 0, inputManager.LC.AxisStick.y) * gameManager.Parameter.MoveSpeed;
        rb.velocity = transform.rotation * moveAxis;
    }
}
