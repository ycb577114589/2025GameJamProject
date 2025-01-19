using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Globalization;
using System;

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
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 startScale;
    [Tooltip("下一个场景")]
    public string NextScene;
    [Tooltip("是否调试模式")]
    public bool isDebug = false;
    public List<Transform> audiencePos;

    private GameState gameState = GameState.Start;
    private float statusStartTime = 0;
    private NetMgr netMgr;

    public int health;
    public static MainGame instance = null;

    public Bubble bubble;
    public float yuzhi = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        health = 100;
    }
    void Start()
    {
        if(isDebug)
        {
            gameState = GameState.Playing;
        }else{
            gameState = GameState.Start;
            GetComponent<Rigidbody>().useGravity = false;
        }
        statusStartTime = Time.time;

        startPos = transform.position;
        startRot = transform.rotation;
        startScale = transform.localScale;

        netMgr = NetMgr.GetNetMgr();
        netMgr.SetFollowObj(gameObject);
        health = 100;
    }

    public GameState GetStatus()
    {
        return gameState;
    }
    public void OnCollisionEnter(Collision other) 
    {
        if(other.transform.tag=="Finish")    
        {
            if(NextScene.Length != 0)
                SceneManager.LoadScene(NextScene);
            else
                Debug.Log("游戏结束");
        }
        else if(other.transform.tag =="Damage")
        {
            Kill();
        }else if(other.transform.tag == "Bullet")
        {
            Bullet bullet = other.gameObject.GetComponentInChildren<Bullet>(); 
            bullet.ColliderWithBall(this);
        }
    } 
    // Update is called once per frame
    void Update()
    {   
        if(isDebug)
        {
            return;
        }
        switch (gameState)
        {
            case GameState.Start:
                StartUpdate();
                break;
            case GameState.Playing:
                PlayerUpdate();
                break;
            case GameState.End:
                EndUpdate();
                break;
            case GameState.Deaed:
                DeadUpdate();
                break;
            default:
                break;
        }
    }

    void Kill()
    {
        if(gameState !=  GameState.Deaed)
        {
            Debug.Log("be killed"); 
            MainGame.instance.bubble.Boom();
            gameState = GameState.Deaed;
            
            GetComponent<Rigidbody>().useGravity = false;
            
            statusStartTime = Time.time;
        }
    }

    void PlayerUpdate()
    {
        transform.transform.localScale = Vector3.Lerp(transform.transform.localScale, Vector3.one * (1 + (health - 100.0f) / 100), Time.deltaTime);
        float radius = 0.5f;
        float topY = transform.position.y + radius * transform.localScale.magnitude;
        float bottomY = 0 + radius * transform.localScale.magnitude;
        if(transform.position.y < bottomY)
        {
            Kill();
        }
        else if(transform.position.y > topY)
        {
            Kill();
        }
    }

    void EndUpdate()
    {
        
    }

    void DeadUpdate()
    {
        if(isDebug)
        {
            transform.position = startPos;
            transform.rotation = startRot;
            transform.localScale = startScale; 
            gameState = GameState.Playing;
            GetComponent<Rigidbody>().useGravity = true;
            statusStartTime = Time.time;
            health = 100;
            return;
        }
        int lastTime = 2;
        if(Time.time - statusStartTime > lastTime)
        {
            gameState = GameState.Start;
            GetComponent<Rigidbody>().useGravity = false;
            statusStartTime = Time.time;
            health = 100;
            transform.position = startPos;
            transform.rotation = startRot;
            transform.localScale = startScale; 
        }
    }

    void StartUpdate()
    {
        if(bubble == null)
        {
            bubble = this.transform.Find("Bubble").GetComponent<Bubble>();
        }
        MainGame.instance.bubble.Reset();
        int lastTime = 2;
        if(Time.time - statusStartTime > lastTime)
        {
            gameState = GameState.Playing;
            GetComponent<Rigidbody>().useGravity = true;
            statusStartTime = Time.time;
        }
        transform.transform.localScale = Vector3.one * (1 + (health - 100.0f) / 100);
    }

    void HurtDamage(int hurtHp)
    {
        health = health -hurtHp;
        if(health <= 0)
        {
            Kill();
        }
        transform.localScale = Vector3.one * (1 + (health - 100.0f) / 100);
    }
}