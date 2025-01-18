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
    public Quaternion quant;

    private PlayerType playerType;


    public void CreateObject(GameObject prefab, Vector3 pos, PlayerType type)
    {
        gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        playerType = type;
    }

    public void Update()
    {
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, quant, 20 * Time.deltaTime);
    }

    public void AddMessage(NetData data)
    {
        quant = Quaternion.Euler(new Vector3(-data.rz,-data.rx,data.ry));
    }
};
