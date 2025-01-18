using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

[System.Serializable]
public class CharacterType
{
    public int id;
    public int type;
}

public class NetMgr : MonoBehaviour
{
    public List<GameObject> playerPrefab;
    private Dictionary<int, Player> dictPlayer = new Dictionary<int, Player>();
    private int Player1Id = -1;
    private int Player2Id = -1;

    private static GameObject DontDesObj = null;
    private GameObject followObj = null;
    private int audienceIdx;
    void Awake()
    {
        if(!DontDesObj)
        {
            DontDestroyOnLoad(gameObject);
            DontDesObj = gameObject;
        }else{
            Destroy(gameObject);
        }
    }

    public static NetMgr GetNetMgr()
    {
        return DontDesObj.GetComponent<NetMgr>();
    }

    public void SetFollowObj(GameObject obj)
    {
        followObj = obj;
        Player1Id = -1;
        Player2Id = -1;
    }

    void Update()
    {
        // 运行时设置物体的旋转为 90 度绕 Y 轴
        foreach(int i in dictPlayer.Keys)
        {
            if(!dictPlayer[i].gameObject)
            {
                if(!followObj)
                {
                    continue;
                }
                PlayerType playerType = PlayerType.Other;
                GameObject prefab = null;
                Vector3 spawnPos = Vector3.zero;
                if(Player1Id == -1)
                {
                    playerType = PlayerType.Player1;
                    Player1Id = i;
                    SendCharacterType(i, 1);
                    prefab = playerPrefab[0];
                    spawnPos = followObj.transform.position;
                }
                else if(Player2Id == -1)
                {
                    playerType = PlayerType.Player2;
                    Player2Id = i;
                    SendCharacterType(i, 2);
                    prefab = playerPrefab[0];
                    spawnPos = followObj.transform.position;
                }
                else if(audienceIdx < MainGame.instance.audiencePos.Count){
                    SendCharacterType(i, 3);
                    prefab = playerPrefab[1];
                    spawnPos = MainGame.instance.audiencePos[audienceIdx++].position;
                }else{
                    continue;
                }
                dictPlayer[i].CreateObject(prefab, spawnPos, playerType, followObj);
            }
            dictPlayer[i].Update();
        }
    }

    void SendCharacterType(int id, int type)
    {
        CharacterType characterType = new CharacterType();
        characterType.id = id;
        characterType.type = type;
        string json = JsonUtility.ToJson(characterType);
        // Debug.Log("Send: " + json);
        GetComponent<WebSocketClientExample>().ws.Send(json);
    }

    public void AddMessage(string message)
    {
        // Debug.Log("Receive: " + message);
        NetData data = JsonUtility.FromJson<NetData>(message);
        if(!dictPlayer.ContainsKey(data.id))
        {
            dictPlayer.Add(data.id, new Player{id = data.id, gameObject = null});
        }
        dictPlayer[data.id].AddMessage(data);
    }
}
