using UnityEngine;

public class FireProjectileContorller : MonoBehaviour
{
    public Transform target; // 要朝向的目标对象
    public GameObject projectilePrefab; // 投掷物的预制体
    public float projectileSpeed = 10f; // 投掷物的速度
    public KeyCode fireKey = KeyCode.Space; // 发射投掷物的按键，默认为空格键

    
    void Update()
    {
        // 使 prefab 始终朝向目标对象
        if (target!= null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void FireProjectile()
    {
        // 实例化投掷物预制体
        GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 给投掷物添加一个向前的力
            rb.linearVelocity = transform.forward * projectileSpeed;
        }
    }
}
