using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 鸡蛋：碰到 Player 时反弹并闪色
/// </summary>
public class Egg : MonoBehaviour
{
    [Header("Egg settings")]
    [SerializeField] private float bounceVelocity;     // 反弹速度

    [Header("Player Selector")]
    [SerializeField] private Color collisionColor = Color.yellow;  // 碰撞时闪一下的颜色
    [SerializeField] private float flashDuration = 0.2f;           // 闪色持续时间
     
    /// <summary>鸡蛋被击中时触发，用于切换玩家回合。</summary>
    public static event Action onHit;
    public static event Action onFallInWater;

    private Rigidbody2D rig;
    private bool isActive = true;
    private SpriteRenderer spriteRenderer;  // 子物体 "Egg Renderer" 上的 SpriteRenderer
    private Color originalColor;            // 原始颜色，用于闪色后恢复

    private float gravityScale;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        // 鸡蛋图片在子物体上，按名称查找
        var eggRenderer = transform.Find("Egg Renderer");
        spriteRenderer = eggRenderer != null ? eggRenderer.GetComponent<SpriteRenderer>() : null;
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        isActive = true;
        gravityScale = rig.gravityScale;
        rig.gravityScale = 0;
        StartCoroutine(WaitAndFall());
    }

    IEnumerator WaitAndFall()
    {
        yield return new WaitForSeconds(2f);
        rig.gravityScale = gravityScale;
    }

    void Update()
    {
    }

    /// <summary>
    /// 碰撞检测：仅与 Player 碰撞时反弹并闪色
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other)
    {
        // 如果碰撞对象是 PlayerController，则反弹并闪色
        if (other.collider.TryGetComponent(out PlayerController playerController))
        {
            // 如果 SpriteRenderer 不为空，则闪色
            if (spriteRenderer != null)
                StartCoroutine(FlashColor());
            // 反弹，沿碰撞法线方向反弹 根据bounceVelocity的速度反弹
            Bounce(other.GetContact(0).normal);
            onHit?.Invoke();
        }
    }

    /// <summary>
    /// 闪色效果：短暂变色后恢复
    /// </summary>
    private IEnumerator FlashColor()
    {
        // 短暂变色后恢复
        spriteRenderer.color = collisionColor;
        // 等待 flashDuration 时间后恢复原始颜色    WaitForSeconds 是 Unity 引擎提供的等待时间类
        yield return new WaitForSeconds(flashDuration);
        // 恢复原始颜色
        spriteRenderer.color = originalColor;
    }

    private void OnTriggerEnter2D(Collider2D Collider)
    {
         if(!isActive) return;
         
         if(Collider.CompareTag("Water"))
         {
            isActive = false;
            onFallInWater?.Invoke();
           
         }

    }

    /// <summary>
    /// 沿碰撞法线方向反弹
    /// </summary>
    private void Bounce(Vector2 normal)
    {
        rig.linearVelocity = normal * bounceVelocity;
    }

    public void Reset()
    {
        transform.position = Vector2.up * 5;
        rig.linearVelocity = Vector2.zero;
        rig.angularVelocity = 0;
        rig.gravityScale = 0;
        isActive = true;
        StartCoroutine(WaitAndFall());
    }
}
