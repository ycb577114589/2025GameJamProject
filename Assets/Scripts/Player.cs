using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player
{
    public int id;
    public GameObject gameObject;
    public PlayerInput playerInput;
    public Quaternion quant;

    public Vector3 acc;


    private GameObject up;
    private GameObject down;
    private GameObject left;

    private Vector3 inputRotation ;
    public void CreateObject(GameObject prefab, Vector3 pos)
    {
        gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        up = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        up.transform.position = pos + new Vector3(0, 0, 1);
        up.transform.localScale *= 20;
        playerInput = gameObject.GetComponent<PlayerInput>();
    }
    private bool InputIsLegal(float inputValue)
    {
        return Mathf.Abs(inputValue) > 0.01f;
    }
    public void Update()
    {
        if(InputIsLegal(inputRotation.x-playerInput.beforeRotation.x)||InputIsLegal(inputRotation.y-playerInput.beforeRotation.y)||InputIsLegal(inputRotation.z-playerInput.beforeRotation.z))
        { 
            gameObject.transform.rotation = quant;
            Vector3 newAcc = gameObject.transform.rotation * acc * 15;
            up.transform.position = gameObject.transform.position + newAcc;
        }
    }
    
    public void AddMessage(NetData data)
    {
        inputRotation = new Vector3(-data.rz,-data.rx,data.ry);
        quant = Quaternion.Euler(inputRotation);
        acc = new Vector3(data.ax, data.ay, data.az);
    }
};
