using UnityEngine;

public class Bubble : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public ParticleSystem m_particleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Boom();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Reset();
        }
    }
    public void Reset()
    {
        if (meshRenderer)
        {
            meshRenderer.enabled = true;
        }

        if (m_particleSystem)
        {
            m_particleSystem.Stop();
            m_particleSystem.time = 0;
        }
    }
    public void Boom()
    {
        if (meshRenderer)
        {
            meshRenderer.enabled = false;
        }

        if (m_particleSystem)
        {
            m_particleSystem.Play();
            // Destroy(m_particleSystem, 3);
        }
    }
}
