using UnityEngine;

public class DistanceForce : MonoBehaviour
{
    public GameObject targetObject; // 目标对象
    public float maxForce = 10f; // 最大的力
    public float meter = 10f; //范围内给吸力；

    void Update()
    {
        targetObject = MainGame.instance.gameObject;
        // 计算两个对象之间的距离
        float distance = Vector3.Distance(transform.position, targetObject.transform.position);
        if (distance < meter)
        {
            // 计算力的大小，这里使用简单的反比关系，距离越近力越大
            // 这里假设力的大小在距离为 0 时为 maxForce，距离为 10 时为 0
            float forceMagnitude = maxForce * (1 - (distance / meter));
            // 计算力的方向，从当前对象指向目标对象
            Vector3 forceDirection = (targetObject.transform.position - transform.position).normalized;
            // 施加力
            targetObject.GetComponent<Rigidbody>().AddForce(-forceDirection * forceMagnitude);
        }
    }
}