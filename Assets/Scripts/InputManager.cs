using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// OculusRiftの位置
    /// </summary>
    public Vector3 Position { get; private set; }
    /// <summary>
    /// OculusRiftの向き
    /// </summary>
    public Quaternion Rotation{ get; private set; }
    /// <summary>
    /// 左コントローラー（プライマリトリガー）
    /// </summary>
    public ControllerInput LC { get; private set; } = new ControllerInput();
    /// <summary>
    /// 右コントローラー（セカンドトリガー）
    /// </summary>
    public ControllerInput RC { get; private set; } = new ControllerInput();
    // Start is called before the first frame update
    [SerializeField]
    private bool testControl = false;

    [SerializeField]
    private Camera mainCamera = null;

    // Update is called once per frame
    void Update()
    {
        if (!testControl)
        {
            VRControl();
        }
        else
        {
            //マウス代用
            TestControl(LC, RC);
        }
    }
    public enum ButtonLock
    {
        HandTrigger,
        IndexTrigger,
        Button1,
        Button2,
        Stick,
        All
    }
    public enum Hands
    {
        Right,
        Left
    }
    public void SafetyLock(Hands hand, ButtonLock changeLock, bool flag)
    {
        ControllerInput input = hand == Hands.Right ? RC : LC;
        switch (changeLock)
        {
            case ButtonLock.HandTrigger:
                input.HandTrigger.IsLock = flag;
                break;
            case ButtonLock.IndexTrigger:
                input.IndexTrigger.IsLock = flag;
                break;
            case ButtonLock.Button1:
                input.Button1.IsLock = flag;
                break;
            case ButtonLock.Button2:
                input.Button2.IsLock = flag;
                break;
            case ButtonLock.Stick:
                input.StickIsLock = flag;
                break;
            case ButtonLock.All:
                input.HandTrigger.IsLock = flag;
                input.IndexTrigger.IsLock = flag;
                input.Button1.IsLock = flag;
                input.Button2.IsLock = flag;
                break;
        }
    }
    private void TestControl(ControllerInput LC, ControllerInput RC)
    {
        //左ボタン1 : Z-Key
        LC.Button1.Get = Input.GetKey(KeyCode.Z);
        LC.Button1.GetDown = Input.GetKeyDown(KeyCode.Z);
        LC.Button1.GetUp = Input.GetKeyUp(KeyCode.Z);

        //左ボタン2 : X-Key
        LC.Button2.Get = Input.GetKey(KeyCode.X);
        LC.Button2.GetDown = Input.GetKeyDown(KeyCode.X);
        LC.Button2.GetUp = Input.GetKeyUp(KeyCode.X);

        //右ボタン1 : C-Key
        RC.Button1.Get = Input.GetKey(KeyCode.C);
        RC.Button1.GetDown = Input.GetKeyDown(KeyCode.C);
        RC.Button1.GetUp = Input.GetKeyUp(KeyCode.C);

        //右ボタン2 : V-Key
        RC.Button2.Get = Input.GetKey(KeyCode.V);
        RC.Button2.GetDown = Input.GetKeyDown(KeyCode.V);
        RC.Button2.GetUp = Input.GetKeyUp(KeyCode.V);

        //マウス位置
        Vector3 cpos = Input.mousePosition;
        cpos.z = 10f;
        Vector3 screenToWorldPointPosition = mainCamera.ScreenToWorldPoint(cpos);
        //回転量
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel") * 100;
        //左ハンドトリガー ： Q
        LC.HandTrigger.Get = Input.GetKey(KeyCode.Q);
        LC.HandTrigger.GetDown = Input.GetKey(KeyCode.Q);
        LC.HandTrigger.GetUp = Input.GetKey(KeyCode.Q);
        if (Input.GetKey(KeyCode.Q))
        {
            LC.HandTrigger.Axis = 1;
        }
        else
        {
            LC.HandTrigger.Axis = 0;
        }
        //右ハンドトリガー ：  E
        RC.HandTrigger.Get = Input.GetKey(KeyCode.E);
        RC.HandTrigger.GetDown = Input.GetKey(KeyCode.E);
        RC.HandTrigger.GetUp = Input.GetKey(KeyCode.E);
        if (Input.GetKey(KeyCode.E))
        {
            RC.HandTrigger.Axis = 1;
        }
        else
        {
            RC.HandTrigger.Axis = 0;
        }
        //左インデックストリガー ： 左クリック
        LC.IndexTrigger.Get = Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift);
        LC.IndexTrigger.GetDown = Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift);
        LC.IndexTrigger.GetUp = Input.GetMouseButtonUp(0) && !Input.GetKey(KeyCode.LeftShift);
        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            LC.IndexTrigger.Axis = 1;
        }
        else
        {
            LC.IndexTrigger.Axis = 0;
        }
        //右インデックストリガー ： Shift + 左クリック
        RC.IndexTrigger.Get = Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift);
        RC.IndexTrigger.GetDown = Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift);
        RC.IndexTrigger.GetUp = Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift);
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift))
        {
            RC.IndexTrigger.Axis = 1;
        }
        else
        {
            RC.IndexTrigger.Axis = 0;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            //コントローラー位置：マウス位置
            LC.Position = screenToWorldPointPosition;
            //コントローラー角度：ホイール
            LC.Angle += scrollWheel;
            LC.Rotation = Quaternion.AngleAxis(LC.Angle, Vector3.up);
        }
        else
        {
            //コントローラー位置：マウス位置
            RC.Position = screenToWorldPointPosition;
            //コントローラー角度：ホイール
            RC.Angle += scrollWheel;
            RC.Rotation = Quaternion.AngleAxis(RC.Angle, Vector3.up);
        }

        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxis("Vertical");
        LC.AxisStick = new Vector2(axisX, axisY);
        /*if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            RC.AxisStick = Vector2.left;
        }
        else if(Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
        {
            RC.AxisStick = Vector2.right;
        }
        else
        {
            RC.AxisStick = Vector2.zero;
        }*/
    }
    private void VRControl()
    {
        //本体
        if (!XRDevice.isPresent)
        {
            Debug.LogWarning("デバイスが接続されていません");
        }
        else
        {
            Position = InputTracking.GetLocalPosition(XRNode.CenterEye);
            Rotation = InputTracking.GetLocalRotation(XRNode.CenterEye);
        }
        //OVRInput.Controller c = OVRInput.GetConnectedControllers();
        //右手（セカンドトリガー）
        //if (c != OVRInput.Controller.RTrackedRemote)
        //{
        //    Debug.LogWarning("右コントローラーが接続されていません");
        //}
        //else
        //{
            UpdateRController(RC);
        //}
        //左手（プライマリトリガー）
        //if (c != OVRInput.Controller.LTrackedRemote)
        //{
        //    Debug.LogWarning("左コントローラーが接続されていません");
        //}
        //else
        //{
            UpdateLController(LC);
        //}
    }

    private void UpdateRController(ControllerInput input)
    {
        input.Button1.Get = OVRInput.Get(OVRInput.RawButton.A);
        input.Button1.GetDown = OVRInput.GetDown(OVRInput.RawButton.A);
        input.Button1.GetUp = OVRInput.GetUp(OVRInput.RawButton.A);

        input.Button2.Get = OVRInput.Get(OVRInput.RawButton.B);
        input.Button2.GetDown = OVRInput.GetDown(OVRInput.RawButton.B);
        input.Button2.GetUp = OVRInput.GetUp(OVRInput.RawButton.B);

        input.HandTrigger.Get = OVRInput.Get(OVRInput.RawButton.RHandTrigger);
        input.HandTrigger.GetDown = OVRInput.GetDown(OVRInput.RawButton.RHandTrigger);
        input.HandTrigger.GetUp = OVRInput.GetUp(OVRInput.RawButton.RHandTrigger);

        input.IndexTrigger.Get = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        input.IndexTrigger.GetDown = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
        input.IndexTrigger.GetUp = OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger);

        input.HandTrigger.Axis = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger);
        input.IndexTrigger.Axis = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
        input.AxisStick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        input.Position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTrackedRemote);
        input.Rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
    }
    private void UpdateLController(ControllerInput input)
    {
        input.Button1.Get = OVRInput.Get(OVRInput.RawButton.X);
        input.Button1.GetDown = OVRInput.GetDown(OVRInput.RawButton.X);
        input.Button1.GetUp = OVRInput.GetUp(OVRInput.RawButton.X);

        input.Button2.Get = OVRInput.Get(OVRInput.RawButton.Y);
        input.Button2.GetDown = OVRInput.GetDown(OVRInput.RawButton.Y);
        input.Button2.GetUp = OVRInput.GetUp(OVRInput.RawButton.Y);

        input.HandTrigger.Get = OVRInput.Get(OVRInput.RawButton.LHandTrigger);
        input.HandTrigger.GetDown = OVRInput.GetDown(OVRInput.RawButton.LHandTrigger);
        input.HandTrigger.GetUp = OVRInput.GetUp(OVRInput.RawButton.LHandTrigger);

        input.IndexTrigger.Get = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
        input.IndexTrigger.GetDown = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger);
        input.IndexTrigger.GetUp = OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger);

        input.HandTrigger.Axis = OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger);
        input.IndexTrigger.Axis = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
        input.AxisStick = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

        input.Position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTrackedRemote);
        input.Rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTrackedRemote);
    }

    /// <summary>
    /// 音を振動に変えて、右のコントローラーから流します
    /// </summary>
    /// <param name="audioClip">音源データ</param>
    public void RightVibration(AudioClip audioClip)
    {
        OVRHaptics.RightChannel.Mix(new OVRHapticsClip(audioClip));
    }
    /// <summary>
    /// 音を振動に変えて、左のコントローラーから流します
    /// </summary>
    /// <param name="audioClip">音源データ</param>
    public void LeftVibration(AudioClip audioClip)
    {
        OVRHaptics.LeftChannel.Mix(new OVRHapticsClip(audioClip));
    }
    public class ControllerInput
    {
        public class GetButton
        {
            public bool IsLock { get; set; }
            private bool buttonGet;
            /// <summary>押している時</summary>
            public bool Get { get { return buttonGet && !IsLock; } set { buttonGet = value; } }
            private bool buttonGetDown;
            /// <summary>押した時</summary>
            public bool GetDown { get { return buttonGetDown && !IsLock; } set { buttonGetDown = value; } }
            private bool buttonGetUp;
            /// <summary>離した時</summary>
            public bool GetUp { get { return buttonGetUp && !IsLock; } set { buttonGetUp = value; } }
        }
        public class GetTrigger : GetButton
        {
            private float axis;
            public float Axis { get { return axis * (IsLock ? 0 : 1); } set { axis = value; } }
        }
        /// <summary>ハンドトリガー</summary>
        public GetTrigger HandTrigger { get; set; } = new GetTrigger();
        /// <summary>インデックストリガー</summary>
        public GetTrigger IndexTrigger { get; set; } = new GetTrigger();
        /// <summary>A or Xボタン</summary>
        public GetButton Button1 { get; set; } = new GetButton();
        /// <summary>B or Yボタン</summary>
        public GetButton Button2 { get; set; } = new GetButton();
        public bool StickIsLock { get; set; }
        private Vector2 axisStick;
        /// <summary>スティックの入力</summary>
        public Vector2 AxisStick { get { return axisStick * (StickIsLock ? 0 : 1); } set { axisStick = value; } }
        /// <summary>コントローラの位置</summary>
        public Vector3 Position { get; set; }
        /// <summary>コントローラの角度</summary>
        public Quaternion Rotation { get; set; }
        public float Angle { get; set; }
    }
}
