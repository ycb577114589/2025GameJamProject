using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public void CreateObject(GameObject prefab, Vector3 pos, PlayerType type, GameObject parent)
    {
        playerType = type;
        gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        gameObject.name = "Player" + id;

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        this.parent = parent;
        playerInput = gameObject.GetComponent<PlayerInput>();
    }
    private bool InputIsLegal(float inputValue)
    {
        return Mathf.Abs(inputValue) > 0.01f;
    }
    public void Update()
    {
        if(MainGame.instance.GetStatus() == GameState.Start)
        {
            if(playerType == PlayerType.Player2)
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
        //gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, quant, 20 * Time.deltaTime);
        if(playerType == PlayerType.Player1)
        {
            Vector3 initialPosition = quant * Vector3.forward * 5;
            // Vector3 direction = -gameObject.transform.forward;
            // direction.y = 0;
            // Vector3 pos = direction.normalized * 5;
            // gameObject.transform.position = parent.transform.position + pos;
            // gameObject.transform.rotation = Quaternion.LookRotation(direction);
            // float radians = rotation * Mathf.Deg2Rad;
            // // 计算 x 和 z 坐标，y 坐标为 0，半径为 1
            // float x = Mathf.Cos(radians);
            // float z = Mathf.Sin(radians);
            // // 将计算得到的位置应用到对象的位置
            // gameObject.transform.position = new Vector3(x, 0, z) * 5 + parent.transform.position;
            // gameObject.transform.rotation
            gameObject.transform.position = parent.transform.position + initialPosition;
        }else if(playerType == PlayerType.Player2)
        {
            gameObject.transform.position = parent.transform.position + new Vector3(0, -5f, 0);
        }

        if(playerInput==null)
        {
            return;
        }
        if(InputIsLegal(inputRotation.x-playerInput.beforeRotation.x)||InputIsLegal(inputRotation.y-playerInput.beforeRotation.y)||InputIsLegal(inputRotation.z-playerInput.beforeRotation.z))
        {
            playerInput.AddForceToBall(gameObject.transform.rotation,id);
        }
    }
    
    public void AddMessage(NetData data)
    {
        inputRotation = new Vector3(-data.rz,-data.rx,data.ry);
        quant = Quaternion.Euler(inputRotation);
        acc = new Vector3(data.ax, data.ay, data.az);
    }
};
