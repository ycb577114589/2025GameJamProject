using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 first = Vector3.zero;
    public Vector3 sec = Vector3.zero;
    
    public GameObject player ;
    private Rigidbody rb;
    
    public Transform testObj;
    // 要应用的力的大小
    public float forceMagnitudeVerticle = 10f;
     
    public float forceMagnitudeHorizontal = 10f;
    public GameObject testobj ;
    public Quaternion beforeRotationQuat ;
    public Vector3 beforeRotationVec ;


    void Start()
    {
        // 获取物体的 Rigidbody 组件
        rb = player.GetComponent<Rigidbody>(); 
    }
    public void AddForceToBall(Quaternion inputRotation,Vector3 intputRotationVec,PlayerType playerType,int id)
    {  
        if(rb == null)
        {
            return;
        }
        Quaternion rotation = inputRotation;
        
        Vector3 forwardDirection = rotation * Vector3.right;
        
        first = forwardDirection;

        sec = beforeRotationQuat * Vector3.right;

        var direction = first + sec;
        var converDirection = Vector3.zero;
        if(playerType == PlayerType.Player2)
        {
            converDirection = new Vector3(direction.x,0,direction.z) * forceMagnitudeVerticle; 
        } 
        else if(playerType== PlayerType.Player1)
        {
            converDirection = new Vector3(0,direction.y,0) * forceMagnitudeHorizontal; 
        }
        Debug.LogError("施加力的方向" + converDirection);
        // 使用刚体施加力
        rb.AddForce(converDirection, ForceMode.Impulse);
        // 测试当前方向的力
        // var test = inputRotation * Vector3.forward ;
        // rb.AddForce( test * forceMagnitude, ForceMode.Impulse);
        // Debug.LogError("当前拍的志向"+test+"  id = " +id);
        // testobj.transform.position = this.transform.position + direction *100;
        
        beforeRotationQuat = inputRotation;
        beforeRotationVec = intputRotationVec;
    }
    void Update()
    {
    }
}