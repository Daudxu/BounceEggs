using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 鸡蛋：碰到 Player 时反弹并闪色
/// </summary>
public class Egg : MonoBehaviour
{
    [Header("Egg settings")]
    [SerializeField] private float bounceVelocity;     // 反弹速度

    [Header("碰撞效果")]
    [SerializeField] private Color collisionColor = Color.yellow;  // 碰撞时闪一下的颜色
    [SerializeField] private float flashDuration = 0.2f;           // 闪色持续时间

    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;  // 子物体 "Egg Renderer" 上的 SpriteRenderer
    private Color originalColor;            // 原始颜色，用于闪色后恢复

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        // 鸡蛋图片在子物体上，按名称查找
        var eggRenderer = transform.Find("Egg Renderer");
        spriteRenderer = eggRenderer != null ? eggRenderer.GetComponent<SpriteRenderer>() : null;
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
    }

    /// <summary>
    /// 碰撞检测：仅与 Player 碰撞时反弹并闪色
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out PlayerController playerController))
        {
            if (spriteRenderer != null)
                StartCoroutine(FlashColor());
            Bounce(other.GetContact(0).normal);
        }
    }

    /// <summary>
    /// 闪色效果：短暂变色后恢复
    /// </summary>
    private IEnumerator FlashColor()
    {
        spriteRenderer.color = collisionColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    /// <summary>
    /// 沿碰撞法线方向反弹
    /// </summary>
    private void Bounce(Vector2 normal)
    {
        rig.linearVelocity = normal * bounceVelocity;
    }
}
