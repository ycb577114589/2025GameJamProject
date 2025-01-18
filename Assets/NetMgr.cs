using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetData
{
    public float rx;
    public float ry;
    public float rz;
    
    public float ax;
    public float ay;
    public float az;
    public int id;
}

public class NetMgr : MonoBehaviour
{
    // 玩家id -》 当前第几个包
    public Dictionary<int,int> dicPlayerIdToFrameCount= new Dictionary<int, int>();

    public List<NetData> listPlayerFrameMessage = new List<NetData>();

    public Transform testRotation;

    public Vector3 nextRotation= Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
    }

    void Update()
    {
        // 运行时设置物体的旋转为 90 度绕 Y 轴
        testRotation.rotation = Quaternion.Euler(nextRotation);
    }
    public void AddMessage(string message)
    {
        
        // 假设 message.data 是一个 JSON 字符串
        string jsonData = message;

        // 使用 JsonUtility 解析 JSON 数据
        NetData data = JsonUtility.FromJson<NetData>(jsonData);
        Debug.Log(data.rx+ "  "+ data.ry+ "  "+data.rz);
        nextRotation = new Vector3(-data.rz,-data.rx,data.ry);
    }
}
