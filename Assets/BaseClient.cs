using UnityEngine;
using WebSocketSharp;

public class WebSocketClientExample : MonoBehaviour
{
    public GameObject node;
    private WebSocket ws;
    public NetMgr netMgr;
    void Start()
    {
        try
        {
            // 创建一个 WebSocket 客户端，连接到 ws://localhost:9090/Chat
            ws = new WebSocket("ws://yylogo.com:9001");

            // 注册消息接收事件
            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Message received from server: " + e.Data);
                netMgr.AddMessage(e.Data);
            };

            // 注册连接打开事件
            ws.OnOpen += (sender, e) =>
            {
                Debug.Log("Connected to the server");
                // 发送消息给服务端
                ws.Send("Hello, Server!");
                node.SetActive(false);
            };

            // 注册连接关闭事件
            ws.OnClose += (sender, e) =>
            {
                Debug.Log("Disconnected from the server: " + e.Reason);
            };

            // 开始连接
            ws.Connect();
        }
        catch (WebSocketException ex)
        {
            Debug.LogError("WebSocket Exception occurred: " + ex.Message);
            Debug.LogError("StackTrace: " + ex.StackTrace);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("General Exception occurred: " + ex.Message);
            Debug.LogError("StackTrace: " + ex.StackTrace);
        }
    }

    void OnDestroy()
    {
        if (ws!= null)
        {
            // 关闭连接
            ws.Close();
        }
    }
}