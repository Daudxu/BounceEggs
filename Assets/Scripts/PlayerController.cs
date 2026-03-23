using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制器：通过指针拖拽（鼠标/触摸）控制玩家左右移动
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("controller settings")]
    [SerializeField] private float moveSpeed;      // 移动速度，控制拖拽时的灵敏度
    private float ClickedScreenX;                  // 按下指针时，指针在屏幕上的 X 坐标
    private float ClickedPlayerX;                  // 按下指针时，玩家在场景中的 X 坐标
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
    /// 处理输入：指针按下时记录起点，拖拽时根据指针位移更新玩家位置
    /// </summary>
    private void ManageController()
    {
        bool wasPressedThisFrame;
        bool isPressed;
        float currentScreenX;

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            wasPressedThisFrame = touch.press.wasPressedThisFrame;
            isPressed = touch.press.isPressed;
            currentScreenX = touch.position.ReadValue().x;
        }
        else if (Mouse.current != null)
        {
            var mouse = Mouse.current;
            wasPressedThisFrame = mouse.leftButton.wasPressedThisFrame;
            isPressed = mouse.leftButton.isPressed;
            currentScreenX = mouse.position.ReadValue().x;
        }
        else
        {
            return;  // 没有可用输入设备时直接返回
        }

        // 指针刚按下：记录拖拽起点
        if (wasPressedThisFrame)
        {
            ClickedScreenX = currentScreenX; // 记录指针屏幕坐标
            ClickedPlayerX = transform.position.x;           // 记录玩家当前坐标
        }

        // 指针按住并拖拽：根据指针位移移动玩家
        else if (isPressed)
        {
            // 计算指针在屏幕上的水平位移
            float xDifference = currentScreenX - ClickedScreenX;
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
