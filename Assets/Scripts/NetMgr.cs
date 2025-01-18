using System;
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
    public int type;
}

public class NetMgr : MonoBehaviour
{
    public List<GameObject> playerPrefab;
    private Dictionary<int, Player> dictPlayer = new Dictionary<int, Player>();

    void Update()
    {
        // 运行时设置物体的旋转为 90 度绕 Y 轴
        foreach(int i in dictPlayer.Keys)
        {
            if(!dictPlayer[i].gameObject)
            {
                dictPlayer[i].CreateObject(playerPrefab[0], new Vector3(0, 0, 200 * dictPlayer[i].id));
            }
            dictPlayer[i].Update();
        }
    }

    public void AddMessage(string message)
    {
        Debug.Log("Receive: " + message);
        NetData data = JsonUtility.FromJson<NetData>(message);
        if(!dictPlayer.ContainsKey(data.id))
        {
            dictPlayer.Add(data.id, new Player{id = data.id, gameObject = null});
        }
        dictPlayer[data.id].AddMessage(data);
    }
}
