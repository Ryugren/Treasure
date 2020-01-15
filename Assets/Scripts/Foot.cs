using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour
{
    [SerializeField] private Player player = null;
    public Player Player { get { return player; } }
}