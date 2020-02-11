using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUI : MonoBehaviour
{
    [SerializeField]
    private Player player = null;
    [SerializeField]
    private GameObject startObject = null;
    [SerializeField]
    private GameObject endObject = null;
    // Update is called once per frame
    private void Awake()
    {
        startObject.SetActive(true);
        endObject.SetActive(false);
    }
    void Update()
    {
        if (startObject.activeSelf && player.GM.Parameter.StartGameFlag)
        {
            startObject.SetActive(false);
        }
        if (!endObject.activeSelf && player.GM.Parameter.EndGameFlag)
        {
            endObject.SetActive(true);
        }
    }
}
