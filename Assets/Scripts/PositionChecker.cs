using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    public bool Flag { get; private set; } = false;
    private void OnTriggerEnter(Collider other)
    {
        if (Flag) return;
        if (other.gameObject.tag == "Player")
        {
            Flag = true;
        }
    }
}
