using UnityEngine;

/// <summary>
/// 单面屏幕边界墙：挂到墙物体上，根据屏幕边缘自动定位，适配各种手机宽高
/// 左右墙：Left/Right；顶部墙：Top
/// </summary>
public class ScreenBoundsWall : MonoBehaviour
{
    public enum Edge { Left, Right, Top }

    [Header("墙所在边缘")]
    [SerializeField] private Edge edge = Edge.Left;

    [Header("可选：指定相机，不填则用 Main Camera")]
    [SerializeField] private Camera targetCamera;

    [Header("边距")]
    [SerializeField] private float padding = 0.5f;  // 墙稍微超出屏幕，避免穿模

    void Start()
    {
        ApplyBounds();
    }

    /// <summary>
    /// 根据相机可视范围设置墙的位置和大小
    /// </summary>
    public void ApplyBounds()
    {
        var cam = targetCamera != null ? targetCamera : Camera.main;
        if (cam == null) return;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector3 camPos = cam.transform.position;

        var box = GetComponent<BoxCollider2D>();

        switch (edge)
        {
            case Edge.Left:
                transform.position = camPos + new Vector3(-halfWidth - padding, 0, 0);
                if (box != null) box.size = new Vector2(padding * 2, halfHeight * 2 + padding * 2);
                break;
            case Edge.Right:
                transform.position = camPos + new Vector3(halfWidth + padding, 0, 0);
                if (box != null) box.size = new Vector2(padding * 2, halfHeight * 2 + padding * 2);
                break;
            case Edge.Top:
                transform.position = camPos + new Vector3(0, halfHeight + padding, 0);
                if (box != null) box.size = new Vector2(halfWidth * 2 + padding * 2, padding * 2);
                break;
        }
    }
}
