using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Playing,
    End,
    Deaed
}

public class MainGame : MonoBehaviour
{
    public Transform endPos;
    private Transform startPos;
    [Tooltip("下一个场景")]
    public string NextScene;

    private GameState gameState = GameState.Start;
    private NetMgr netMgr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.Start;
        startPos = transform;
        netMgr = NetMgr.GetNetMgr();
        netMgr.SetFollowObj(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diffToEnd = transform.position - endPos.position;
        if(diffToEnd.sqrMagnitude < 10)
        {
            if(NextScene.Length != 0)
                SceneManager.LoadScene(NextScene);
            else
                Debug.Log("游戏结束");
        }
    }

    void Kill()
    {
        gameState = GameState.Deaed;

        // 死亡表现
        transform.position = startPos.position;
        gameState = GameState.Start;
    }

    void PlayerUpdate()
    {
        ;
    }

    void EndUpdate()
    {
        ;
    }

    void DeadUpdate()
    {
        ;
    }

    void StartUpdate()
    {
        ;
    }
}
