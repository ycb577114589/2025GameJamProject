using System.Collections.Generic;
using UnityEngine;

// 定义 objItem 类，用于存储旋转信息
[System.Serializable]
public class objItem
{
    public float rotationSpeed = 50.0f; // 旋转速度，可以在 Unity 编辑器中调整
    public GameObject obj;
}

// 主类 RotateAroundY 继承自 MonoBehaviour，用于控制对象旋转
public class RotateAroundY : MonoBehaviour
{
    [SerializeField]
    private List<objItem> objList = new List<objItem>();

    void Update()
    {
        foreach (var item in objList)
        {
            if (item.obj!= null)
            {
                item.obj.transform.Rotate(Vector3.up, item.rotationSpeed * Time.deltaTime);
            }
        }
    }
}