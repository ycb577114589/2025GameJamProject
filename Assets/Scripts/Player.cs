using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum PlayerType
{
    Player1,
    Player2,
    Other,
}

public class Player
{
    public int id;
    public GameObject gameObject;
    public PlayerInput playerInput;
    public Quaternion quant;

    private PlayerType playerType;

    private Vector3 inputRotation;
    private Vector3 acc;
    private GameObject parent;

    public float constDeltaTime = 0.2f;

    public float curDeltaTime = 0f;

    private Quaternion lastQuen = Quaternion.identity;
    private bool IsDestroy;

    public void CreateObject(GameObject prefab, Vector3 pos, PlayerType type, GameObject parent)
    {
        gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        if(type == PlayerType.Other)
        {
            gameObject.GetComponent<FireProjectileContorller>().target = MainGame.instance.gameObject.transform;
             
        }
        gameObject.name = "Player" + id;
        playerType = type;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.position = pos;
        this.parent = parent;
        playerInput = gameObject.GetComponent<PlayerInput>();
        lastQuen = quant;
        if(type == PlayerType.Other)
        {
            if(MainGame.instance.audiencePos.Count > 0 )
            {
                gameObject.transform.position = MainGame.instance.audiencePos[MainGame.instance.curPos].position;
                if(MainGame.instance.curPos + 1 < MainGame.instance.audiencePos.Count )
                {
                    MainGame.instance.curPos ++;
                }
            }
        }
    }
    private bool InputIsLegal(float inputValue)
    {
        var diff = Mathf.Abs(inputValue);
        while(diff > 180)
            diff = 360 - diff;
        return Mathf.Abs(diff) > MainGame.instance.yuzhi;
    }
    
    public void Update()
    {
        if(playerType == PlayerType.Other)
        {
            OtherUpdate();
            return;
        }
        PlayerUpdate();
    }

    void PlayerUpdate()
    {
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, quant, 20 * Time.deltaTime);
        Vector3 pos;
 
        if(playerType == PlayerType.Player1)
        {
            Vector3 dir = gameObject.transform.right;
            dir.y = 0;
            pos = parent.transform.position - dir.normalized * 2;
        }else if(playerType == PlayerType.Player2)
        {
            pos = parent.transform.position - new Vector3(0, 2, 0);
        }
        else
        {
            pos = gameObject.transform.position;
        }
        gameObject.transform.position = pos;

        if(MainGame.instance.GetStatus() == GameState.Start)
        {
            // if(playerType == PlayerType.Player1)
            // {
            //     MainGame.instance.health += 1;
            //     float degree = Quaternion.Angle(lastQuen, quant);
            //     if(degree > 8)
            //     {
            //         lastQuen = quant;
            //         MainGame.instance.health += (int)(degree / 4.0f);
            //     }
            // }
            return;
        }
        if(MainGame.instance.GetStatus() != GameState.Playing)
        {
            return;
        }
        if(playerInput == null)
        {
            return;
        }
        int legalNum = 0;
        Vector3 diff = gameObject.transform.rotation.eulerAngles - playerInput.beforeRotationQuat.eulerAngles;
        if(InputIsLegal(diff.x))
        {
            legalNum++;
        }
        if(InputIsLegal(diff.y))
        {
            legalNum++;
        }
        if(InputIsLegal(diff.z))
        {
            legalNum++;
        }
        if(legalNum >= 1)
        {
            playerInput.AddForceToBall(gameObject.transform.rotation, playerType, id);
        }
        playerInput.beforeRotationQuat = gameObject.transform.rotation;
    }

    private float lastBullet = 0;

    void OtherUpdate()
    {
        if(Time.time - lastBullet < 0.4f)
        {
            return;
        }
        lastBullet = Time.time;
        float angle = Quaternion.Angle(lastQuen, quant);
        if(angle > 60)
        {
            lastQuen = quant;
            gameObject.GetComponent<FireProjectileContorller>().FireProjectile(angle / 90.0f);
        }else{
            lastQuen = Quaternion.Lerp(lastQuen, quant, 0.01f * Time.deltaTime);
        }
    }

    public void AddMessage(NetData data)
    {
        inputRotation = new Vector3(-data.rz,-data.rx,data.ry);
        quant = Quaternion.Euler(inputRotation);
        acc = new Vector3(data.ax, data.ay, data.az);
    }
};
