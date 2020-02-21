using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : MonoBehaviour
{
    private bool flag = false;
    [SerializeField]
    private float speed = 1f;
    void Update()
    {
        if (flag)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
    public void SlideStart()
    {
        flag = true;
    }
}
