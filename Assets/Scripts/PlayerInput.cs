using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 first = Vector3.zero;
    public Vector3 sec = Vector3.zero;
    
    public GameObject player;
    private Rigidbody rb;
    
    public Transform testObj;
    // 要应用的力的大小
    public float forceMagnitudeVerticle = 10f;
     
    public float forceMagnitudeHorizontal = 10f;
    public GameObject testobj ;
    public Quaternion beforeRotationQuat ;


    void Start()
    {
        // 获取物体的 Rigidbody 组件
        rb = player.GetComponent<Rigidbody>(); 
    }

    public Vector3 CalForceToBall(Quaternion inputRotation, PlayerType playerType, int id)
    {
        var direction = inputRotation * Vector3.right;
        var convertDirection = Vector3.zero;
        if(playerType == PlayerType.Player1)
        {
            convertDirection = new Vector3(direction.x, 0, direction.z);
        }
        else if(playerType== PlayerType.Player2)
        {
            convertDirection = new Vector3(0,direction.y,0);
            if(direction.y < 0)
                convertDirection.y = 0;
        }
        return convertDirection;
    }

    public Vector3 AddForceToBall(Quaternion inputRotation, PlayerType playerType, int id)
    {
        if(rb == null)
        {
            return Vector3.zero;
        }
        Quaternion rotation = inputRotation;
        
        Vector3 forwardDirection = rotation * Vector3.right;
        
        first = forwardDirection;

        sec = beforeRotationQuat * Vector3.right;

        var direction = first + sec;
        direction.Normalize();
        if(direction.magnitude > 2)
        {
            return Vector3.zero;
        }
        var convertDirection = Vector3.zero;
        var convertForce = Vector3.zero;
        if(playerType == PlayerType.Player1)
        {
            convertDirection = new Vector3(direction.x, 0, direction.z);
            float diff = Vector3.Dot(direction, convertDirection);
            convertForce = convertDirection * forceMagnitudeVerticle * diff;
        }
        else if(playerType== PlayerType.Player2)
        {
            float diff = Vector3.Dot(direction, Vector3.up);
            convertDirection = new Vector3(0, diff, 0);
            if(direction.y < 0)
                convertDirection.y = -convertDirection.y;
            convertForce = convertDirection * forceMagnitudeHorizontal;
        }
        Debug.Log("force: " + convertForce);
        // 使用刚体施加力
        rb.AddForce(convertForce, ForceMode.Impulse);
        // 测试当前方向的力
        // var test = inputRotation * Vector3.forward ;
        // rb.AddForce( test * forceMagnitude, ForceMode.Impulse);
        // Debug.LogError("当前拍的志向"+test+"  id = " +id);
        // testobj.transform.position = this.transform.position + direction *100;

        beforeRotationQuat = inputRotation;
        return convertDirection;
    }
}