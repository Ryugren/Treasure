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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AlphaChanged(float volume)
    {
        material.SetFloat("_Alpha", volume);
    }
}
