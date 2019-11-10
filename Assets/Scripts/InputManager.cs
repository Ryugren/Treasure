using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// 左コントローラー（プライマリトリガー）
    /// </summary>
    public ControllerInput LC { get; private set; }
    /// <summary>
    /// 右コントローラー（セカンドトリガー）
    /// </summary>
    public ControllerInput RC { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!XRDevice.isPresent) Debug.LogWarning("デバイスが接続されていません");
        //右手（セカンドトリガー）
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
        {
            Debug.LogWarning("右コントローラーが接続されていません");
        }
        else
        {
            UpdateController(OVRInput.Controller.RTrackedRemote, RC);
        }
        //左手（プライマリトリガー）
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
        {
            Debug.LogWarning("左コントローラーが接続されていません");
        }
        else
        {
            UpdateController(OVRInput.Controller.LTrackedRemote, LC);
        }
    }
    private void UpdateController(OVRInput.Controller controller, ControllerInput input)
    {
        input.Button1.Get = OVRInput.Get(OVRInput.Button.One, controller);
        input.Button1.GetDown = OVRInput.GetDown(OVRInput.Button.One, controller);
        input.Button1.GetUp = OVRInput.GetUp(OVRInput.Button.One, controller);

        input.Button2.Get = OVRInput.Get(OVRInput.Button.Two, controller);
        input.Button2.GetDown = OVRInput.GetDown(OVRInput.Button.Two, controller);
        input.Button2.GetUp = OVRInput.GetUp(OVRInput.Button.Two, controller);

        input.HandTrigger.Get = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller);
        input.HandTrigger.GetDown = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controller);
        input.HandTrigger.GetUp = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controller);

        input.IndexTrigger.Get = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller);
        input.IndexTrigger.GetDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller);
        input.IndexTrigger.GetUp = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller);

        input.HandTrigger.Axis = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        input.IndexTrigger.Axis = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        input.AxisStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controller);

        input.Position = OVRInput.GetLocalControllerPosition(controller);
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
            /// <summary>押している時</summary>
            public bool Get { get; set; }
            /// <summary>押した時</summary>
            public bool GetDown { get; set; }
            /// <summary>離した時</summary>
            public bool GetUp { get; set; }
        }
        public class GetTrigger : GetButton
        {
            public float Axis { get; set; }
        }
        /// <summary>ハンドトリガー</summary>
        public GetTrigger HandTrigger { get; set; }
        /// <summary>インデックストリガー</summary>
        public GetTrigger IndexTrigger { get; set; }
        /// <summary>A or Xボタン</summary>
        public GetButton Button1 { get; set; }
        /// <summary>B or Yボタン</summary>
        public GetButton Button2 { get; set; }
        /// <summary>スティックの入力</summary>
        public Vector2 AxisStick { get; set; }
        /// <summary>コントローラの位置</summary>
        public Vector3 Position { get; set; }
    }
}
