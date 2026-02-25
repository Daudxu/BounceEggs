using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制器：通过鼠标拖拽控制玩家左右移动
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("controller settings")]
    [SerializeField] private float moveSpeed;      // 移动速度，控制拖拽时的灵敏度
    private float ClickedScreenX;                  // 按下鼠标时，鼠标在屏幕上的 X 坐标
    private float ClickedPlayerX;                  // 按下鼠标时，玩家在场景中的 X 坐标
    [SerializeField] private float MaxX;
    void Start()
    {
        Debug.Log("Start");
    }

    void Update()
    {
        ManageController();
    }

    /// <summary>
    /// 处理输入：鼠标按下时记录起点，拖拽时根据鼠标位移更新玩家位置
    /// </summary>
    private void ManageController()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;  // 编辑器或某些平台可能没有鼠标，需判空

        // 左键刚按下：记录拖拽起点
        if (mouse.leftButton.wasPressedThisFrame)
        {
            ClickedScreenX = mouse.position.ReadValue().x;   // 记录鼠标屏幕坐标
            ClickedPlayerX = transform.position.x;           // 记录玩家当前坐标
        }
        // 左键按住并拖拽：根据鼠标位移移动玩家
        else if (mouse.leftButton.isPressed)
        {
            // 计算鼠标在屏幕上的水平位移
            float xDifference = mouse.position.ReadValue().x - ClickedScreenX;
            xDifference /= Screen.width;   // 归一化：转换为占屏幕宽度的比例 (0~1)
            xDifference *= moveSpeed;       // 乘以速度，得到游戏世界中的实际位移

            // 新位置 = 按下时的玩家位置 + 位移量
            float newXPosition = ClickedPlayerX + xDifference;
            // 限制玩家位置在屏幕范围内
            newXPosition = Mathf.Clamp(newXPosition, -MaxX, MaxX);

            transform.position = new Vector2(newXPosition, transform.position.y);  // 只改 X，Y 不变
        }
    }

}
