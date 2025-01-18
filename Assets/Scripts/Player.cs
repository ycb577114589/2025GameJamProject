using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player
{
    public int id;
    public GameObject gameObject;
    public Quaternion quant;

    public Vector3 acc;


    private GameObject up;
    private GameObject down;
    private GameObject left;

    public void CreateObject(GameObject prefab, Vector3 pos)
    {
        gameObject = GameObject.Instantiate(prefab, pos,Quaternion.identity);
        up = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        up.transform.position = pos + new Vector3(0, 0, 1);
        up.transform.localScale *= 20;
    }

    public void Update()
    {
        gameObject.transform.rotation = quant;
        Vector3 newAcc = gameObject.transform.rotation * acc * 15;
        up.transform.position = gameObject.transform.position + newAcc;
    }

    public void AddMessage(NetData data)
    {
        quant = Quaternion.Euler(new Vector3(-data.rz,-data.rx,data.ry));
        acc = new Vector3(data.ax, data.ay, data.az);
    }
};
