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
        return Mathf.Abs(inputValue) > 1f;
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
            float rotation = gameObject.transform.eulerAngles.y;
            float radians = rotation * Mathf.Deg2Rad;
            // 计算 x 和 z 坐标，y 坐标为 0，半径为 1
            float x = Mathf.Cos(radians);
            float z = Mathf.Sin(radians);
            // 将计算得到的位置应用到对象的位置
            gameObject.transform.position = new Vector3(x, 0, z) * 5 + parent.transform.position;
        }
        else if(playerType == PlayerType.Player2)
        {
            gameObject.transform.position = parent.transform.position + new Vector3(0, -5f, 0);
        }

        if(playerInput==null)
        {
            return;
        }
        if(InputIsLegal(inputRotation.x-playerInput.beforeRotationVec.x)||InputIsLegal(inputRotation.y-playerInput.beforeRotationVec.y)||InputIsLegal(inputRotation.z-playerInput.beforeRotationVec.z))
        {
            Debug.LogError("player test inputRotation "+inputRotation+ " beforeRotation" + playerInput.beforeRotationVec);
            playerInput.AddForceToBall(gameObject.transform.rotation, inputRotation,playerType, id);
        }
    }
    
    public void AddMessage(NetData data)
    {
        inputRotation = new Vector3(-data.rz,-data.rx,data.ry);
        quant = Quaternion.Euler(inputRotation);
        acc = new Vector3(data.ax, data.ay, data.az);
    }
};
