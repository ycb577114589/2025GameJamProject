using UnityEngine;

public class InputTest : MonoBehaviour
{
    public Vector3 first = Vector3.zero;
    public Vector3 sec = Vector3.zero;
    
    public GameObject player ;
    private Rigidbody rb;
    
    // 要应用的力的大小
    public float forceMagnitude = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 获取物体的 Rigidbody 组件
        rb = player.GetComponent<Rigidbody>();
        
    }
    public void CheckForward()
    {
        // 获取当前物体的旋转
        Quaternion rotation = transform.rotation;

        // 获取物体的前方向向量
        Vector3 forwardDirection = rotation * Vector3.forward;
        Debug.LogError("正方向为"+forwardDirection);
        first = forwardDirection;

        sec = new Vector3(1,0,0);

        var direction = first + sec;
        Debug.LogError("施加力的方向"+direction);

         // 使用刚体施加力
        rb.AddForce(direction * forceMagnitude, ForceMode.Impulse);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("H 键被按下");
            CheckForward();
        }
        
    }

}
