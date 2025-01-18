using UnityEngine;

public class AirResistance : MonoBehaviour
{
    public float dragCoefficient = 0.1f;  // 阻力系数，控制空气阻力的大小
    public GameObject player;
    Rigidbody rb;

    void Start()
    {
        // 获取物体的刚体组件
        rb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 获取物体当前速度
        Vector3 velocity = rb.linearVelocity;
        
        // 计算速度的反向方向（阻力方向）
        Vector3 airResistance = -velocity.normalized * dragCoefficient * velocity.sqrMagnitude;
        
        // 施加空气阻力（使用 ForceMode.Force 可以让阻力持续作用）
        rb.AddForce(airResistance, ForceMode.Force);
    }
}
