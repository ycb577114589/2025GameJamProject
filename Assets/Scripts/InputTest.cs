using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 first = Vector3.zero;
    public Vector3 sec = Vector3.zero;
    
    public GameObject player ;
    private Rigidbody rb;
    
    public Transform testObj;
    // 要应用的力的大小
    public float forceMagnitude = 10f;
     

    public Quaternion beforeRotation ;
    void Start()
    {
        // 获取物体的 Rigidbody 组件
        rb = player.GetComponent<Rigidbody>(); 
    }
    public void AddForceToBall(Quaternion inputRotation,int id)
    {  
        
        Quaternion rotation = inputRotation;
        
        Vector3 forwardDirection = rotation * Vector3.forward;
        
        Debug.LogError("本次结束时候拍子的方向"+forwardDirection+ " test 方向" + testObj.rotation+"  id = " +id);
        
        first = forwardDirection;

        sec = beforeRotation * Vector3.forward;

        var direction = first + sec;
        Debug.LogError("施加力的方向"+direction+"  id = " +id);

         // 使用刚体施加力
        // rb.AddForce(direction * forceMagnitude, ForceMode.Impulse);
         // 测试当前方向的力
        var test = inputRotation * Vector3.forward ;
        rb.AddForce( test * forceMagnitude, ForceMode.Impulse);
        Debug.LogError("当前拍的志向"+test+"  id = " +id);

        beforeRotation = inputRotation;
    }
    void Update()
    {
    }
}