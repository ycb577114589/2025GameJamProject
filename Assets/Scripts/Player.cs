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

    public void CreateObject(GameObject prefab, Vector3 pos, PlayerType type)
    {
        playerType = type;
        gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        playerInput = gameObject.GetComponent<PlayerInput>();
    }
    private bool InputIsLegal(float inputValue)
    {
        return Mathf.Abs(inputValue) > 0.01f;

    }
    public void Update()
    {
        if(playerInput==null)
        {
            return;
        }
        if(InputIsLegal(inputRotation.x-playerInput.beforeRotation.x)||InputIsLegal(inputRotation.y-playerInput.beforeRotation.y)||InputIsLegal(inputRotation.z-playerInput.beforeRotation.z))
        { 
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, quant, 20 * Time.deltaTime);
            Debug.Log("test rottation " +  gameObject.transform.rotation+"  id = " +id);
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
