using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private GameObject followObj;
    private Vector3 offset = new Vector3(0, 5, -5);
    void Start()
    {
        MainGame findResult = GameObject.FindAnyObjectByType<MainGame>();
        if(findResult)
        {
            followObj = findResult.gameObject;
            offset = transform.position - followObj.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(followObj)
        {
            transform.position = followObj.transform.position + offset;
        }
    }
}
