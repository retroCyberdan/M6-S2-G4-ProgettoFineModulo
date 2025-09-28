using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Fog Settings")]
    public Color fogColor = Color.gray;
    public float fogDensity = 0.04f;
    public FogMode fogMode = FogMode.Exponential; // Exponential = più fitta con distanza
    public float linearStart = 5f;
    public float linearEnd = 20f;

    [Header("Particle Fog")]
    public bool useParticleFog = true;
    public Material particleMaterial; // assegna un materiale trasparente bianco/grigio
    public Vector3 fogAreaSize = new Vector3(50f, 10f, 50f);
    public float particleDensity = 200f;

    void Start()
    {
        // --- Unity Built-in Fog ---
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogDensity = fogDensity;

        if (fogMode == FogMode.Linear)
        {
            RenderSettings.fogStartDistance = linearStart;
            RenderSettings.fogEndDistance = linearEnd;
        }

        // --- Particle Fog (opzionale) ---
        if (useParticleFog)
            CreateParticleFog();
    }

    void CreateParticleFog()
    {
        GameObject fogObj = new GameObject("ParticleFog");
        fogObj.transform.SetParent(transform);
        fogObj.transform.position = Vector3.zero;

        ParticleSystem ps = fogObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startSize = new ParticleSystem.MinMaxCurve(5f, 10f);
        main.startSpeed = 0.1f;
        main.startLifetime = 20f;
        main.maxParticles = (int)particleDensity;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = particleDensity;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = fogAreaSize;

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = particleMaterial;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
    }
}