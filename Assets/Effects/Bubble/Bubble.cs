using UnityEngine;

public class Bubble : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public ParticleSystem m_particleSystem;

    public AudioClip sound1;  // 第一个音效
    public AudioClip sound2;  // 第二个音效
    public AudioClip soundBroken;  // 第二个音效
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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
        // 比如按下空格键播放第一个音效
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlaySound(1);  // 播放第一个音效
        }

        // 比如按下回车键播放第二个音效
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlaySound(2);  // 播放第二个音效
        }
    }
    // 播放不同的音效
    public void PlaySound(int soundNumber)
    {
        if (soundNumber == 1)
        {
            audioSource.clip = sound1;  // 设置为第一个音效
        }
        else if (soundNumber == 2)
        {
            audioSource.clip = sound2;  // 设置为第二个音效
        } 
        else if (soundNumber == 3)
        {
            audioSource.clip = soundBroken;  // 设置为第二个音效
        }


        audioSource.Play();  // 播放当前音效
    }    public void Reset()
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
            PlaySound(3);
            // Destroy(m_particleSystem, 3);
        }
    }
}
