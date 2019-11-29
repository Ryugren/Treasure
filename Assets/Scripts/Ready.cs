using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//コクブ制作
//Date : 11/29

public class Ready : MonoBehaviour
{
    //入力操作の受付
    public bool isContollorl = false;

    //Debug用
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        IsReadyToEnd();
    //    }
    //}

    //Ready状態を終了させる
    public void IsReadyToEnd()
    {
        isContollorl = true;
        Destroy(this.gameObject);
    }
}
