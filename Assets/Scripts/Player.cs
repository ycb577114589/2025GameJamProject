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

    public float constDeltaTime = 0.2f;

    public float curDeltaTime = 0f;
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
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, quant, 20 * Time.deltaTime);
        var direct = playerInput.CalForceToBall(gameObject.transform.rotation, playerType, id);
        gameObject.transform.position = parent.transform.position - direct.normalized * 3;

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
        if(playerInput == null)
        {
            return;
        }
        if(InputIsLegal(inputRotation.x - playerInput.beforeRotationVec.x)||InputIsLegal(inputRotation.y - playerInput.beforeRotationVec.y)||InputIsLegal(inputRotation.z-playerInput.beforeRotationVec.z))
        {
            playerInput.AddForceToBall(gameObject.transform.rotation, inputRotation, playerType, id);
        }
    }

    public void AddMessage(NetData data)
    {
        inputRotation = new Vector3(-data.rz,-data.rx,data.ry);
        quant = Quaternion.Euler(inputRotation);
        acc = new Vector3(data.ax, data.ay, data.az);
    }
};
