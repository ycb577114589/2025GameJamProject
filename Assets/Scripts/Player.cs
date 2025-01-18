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
 
    // public void CreateObject(GameObject prefab, Vector3 pos, PlayerType type, GameObject parent)
    // {
    //     playerType = type;
    //     gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
    //     gameObject.name = "Player" + id;

    //     gameObject.transform.localScale = new Vector3(1, 1, 1);
    //     this.parent = parent;
    //     playerInput = gameObject.GetComponent<PlayerInput>();
    // }
    public void CreateObject(GameObject prefab,GameObject prefab2, Vector3 pos, PlayerType type, GameObject parent)
    {
        if(type != PlayerType.Other)
        {
            gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        } 
        else
        {
            gameObject = GameObject.Instantiate(prefab2, pos,Quaternion.identity);
            gameObject.GetComponent<FireProjectileContorller>().target = MainGame.instance.gameObject.transform;
        }
        gameObject.name = "Player" + id;

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        this.parent = parent;
        playerInput = gameObject.GetComponent<PlayerInput>();
        // gameObject.transform.parent = playerInput.contains;
    }
    private bool InputIsLegal(float inputValue)
    {
        return Mathf.Abs(inputValue) > MainGame.instance.yuzhi;
    }
    
    public void Update()
    {
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, quant, 20 * Time.deltaTime);
        //var direct = playerInput.CalForceToBall(gameObject.transform.rotation, playerType, id);
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
            if(playerType == PlayerType.Player1)
            {
                float F = 1;
            }
            MainGame.instance.health += (int)1;
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
        int isLegalNum = 0;
        if(InputIsLegal(inputRotation.z-playerInput.beforeRotationVec.z))
        {
            isLegalNum ++;
        }
        if(InputIsLegal(inputRotation.y - playerInput.beforeRotationVec.y))
        {
            isLegalNum++;
        }
        if(InputIsLegal(inputRotation.x - playerInput.beforeRotationVec.x))
        {
            isLegalNum++;
        }
        if(isLegalNum >= 1 )
        {
            playerInput.AddForceToBall(gameObject.transform.rotation, inputRotation, playerType, id);
        }
        playerInput.beforeRotationQuat = gameObject.transform.rotation;
        playerInput.beforeRotationVec = inputRotation;
    }

    public void AddMessage(NetData data)
    {
        inputRotation = new Vector3(-data.rz,-data.rx,data.ry);
        quant = Quaternion.Euler(inputRotation);
        acc = new Vector3(data.ax, data.ay, data.az);
    }
};
