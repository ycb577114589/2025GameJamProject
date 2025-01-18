using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 first = Vector3.zero;
    public Vector3 sec = Vector3.zero;
    
    public GameObject player;

    public Bubble playerBubble;
    private Rigidbody rb;
    
    public Transform testObj;
    // 要应用的力的大小
    public float forceMagnitudeVerticle = 10f;
     
    public float forceMagnitudeHorizontal = 10f;
    public GameObject testobj ;
    public Quaternion beforeRotationQuat ;

    void Start()
    {
        player = GameObject.Find("Ball_Main");
        // 获取物体的 Rigidbody 组件
        rb = player.GetComponent<Rigidbody>(); 
        playerBubble= MainGame.instance.bubble; 
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
            convertForce = convertDirection * forceMagnitudeVerticle;

            playerBubble.PlaySound(1);
        }
        else if(playerType== PlayerType.Player2)
        {
            convertDirection = new Vector3(0, direction.y, 0);
            if(direction.y < 0)
                convertDirection.y = -convertDirection.y;
            convertForce = convertDirection * forceMagnitudeHorizontal;
            playerBubble.PlaySound(2);
        }
        //  Debug.Log("force: " + convertForce);
        // 使用刚体施加力
        rb.AddForce(convertForce, ForceMode.Impulse);
        return convertDirection;
    }
}
