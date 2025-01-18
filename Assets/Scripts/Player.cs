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

    public void CreateObject(GameObject prefab, Vector3 pos, PlayerType type, GameObject parent)
    {
        playerType = type;
        gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        gameObject.name = "Player" + id;
        if(type == PlayerType.Player1 || type == PlayerType.Player2)
        {
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            if(type == PlayerType.Player2)
            {
                gameObject.transform.localPosition = new Vector3(0, -1f, 0);
            }
        }else{
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        playerInput = gameObject.GetComponent<PlayerInput>();
    }
    private bool InputIsLegal(float inputValue)
    {
        return Mathf.Abs(inputValue) > 0.01f;
    }
    public void Update()
    {
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, quant, 20 * Time.deltaTime);
        if(playerType == PlayerType.Player1)
        {
            float rotation = gameObject.transform.eulerAngles.y;
            float radians = rotation * Mathf.Deg2Rad;
            // 计算 x 和 z 坐标，y 坐标为 0，半径为 1
            float x = Mathf.Cos(radians);
            float z = Mathf.Sin(radians);
            // 将计算得到的位置应用到对象的位置
            gameObject.transform.localPosition = new Vector3(x, 0, z);
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
