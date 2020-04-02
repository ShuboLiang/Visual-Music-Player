using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;

    private Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, (AudioVisable._audioBandBuffer[band]) * scaleMultiplier + startScale, transform.localScale.z);
        Color color = new Color(AudioVisable._audioBandBuffer[band], AudioVisable._audioBandBuffer[band], AudioVisable._audioBandBuffer[band]);
        material.SetColor("_EmissionColor", color);
    }
}