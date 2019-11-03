using UnityEngine;

public class Recenter : MonoBehaviour
{
    void Update()
    {
        // Rキーで位置トラッキングをリセットする
        if (Input.GetKeyDown(KeyCode.R))
        {
            OVRManager.display.RecenterPose();
        }
    }
}