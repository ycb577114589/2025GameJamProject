using UnityEngine;

public class AirResistance : MonoBehaviour
{
    public float dragCoefficientVertical = 0.1f;  // 阻力系数，控制空气阻力的大小
    public float dragCoefficientHor = 0.1f;  // 阻力系数，控制空气阻力的大小
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
        var tmpVertical = new Vector3(velocity.x,0,velocity.z);
        var tmpHori = new Vector3(0,velocity.y,0);
        // 计算速度的反向方向（阻力方向）
        Vector3 airResistance1= -tmpVertical.normalized * dragCoefficientVertical * tmpVertical.sqrMagnitude;
        // 施加空气阻力（使用 ForceMode.Force 可以让阻力持续作用）
        rb.AddForce(airResistance1, ForceMode.Force);
        
        Vector3 airResistance2= -tmpHori.normalized * dragCoefficientVertical * tmpHori.sqrMagnitude;
        // 施加空气阻力（使用 ForceMode.Force 可以让阻力持续作用）
        rb.AddForce(airResistance2, ForceMode.Force);
    }
}
