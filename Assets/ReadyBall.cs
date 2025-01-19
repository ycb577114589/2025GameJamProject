using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyBall : MonoBehaviour
{
    private Rigidbody rb; 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Push();
    }

    public void Push()
    {
        rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
        GetComponent<Rigidbody>().useGravity = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("OutdoorsScene_level1");
        }
    }
}
