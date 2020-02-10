using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureNoise : MonoBehaviour
{
    [SerializeField]
    private Material material = null;
    // Start is called before the first frame update
    void Start()
    {
        AlphaChanged(0);
    }
    public void AlphaChanged(float volume)
    {
        if (volume < 0) volume = 0;
        else if (volume > 1) volume = 1;
        material.SetFloat("_Alpha", volume);
    }
}
