using System.Collections;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Bubble bubble;
    void Start()
    {
        bubble = GetComponent<Bubble>();
    }

    public void ColliderWithBall(MainGame mainGame)
    {
        if (mainGame != null)
        {
            mainGame.health += 6;
            bubble.Boom();
            StartCoroutine(DestroyAfterSeconds(3f));
        }
    }

    IEnumerator DestroyAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 销毁当前游戏对象
        Destroy(this.gameObject);
    }
}
