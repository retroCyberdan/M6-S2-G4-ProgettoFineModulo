using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    public float minIntensity = 0.25f;
    public float maxIntensity = 1f;

    private Light _lightComponent;

    void Start()
    {
        _lightComponent = GetComponent<Light>();
    }

    void Update()
    {
        _lightComponent.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * 2f, 0));
    }
}
