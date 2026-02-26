using UnityEngine;
using Unity.Netcode;

/// <summary>
/// 玩家着色器：在网络中同步玩家 SpriteRenderer 的颜色。
/// 服务端在 spawn 时广播颜色，或通过 ServerRpc 动态修改。
/// </summary>
public class PlayerColorizer : NetworkBehaviour
{
    [Header("Player Color")]
    [SerializeField] private SpriteRenderer[] renderers;  // 需要着色的 SpriteRenderer（如身体、帽子等），可在 Inspector 中配置

    /// <summary>
    /// 玩家 spawn 时由服务端广播颜色到所有客户端。
    /// </summary>
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if ( !IsServer && IsOwner)
            ColorizeServerRpc(Color.red);
    }

    /// <summary>
    /// 服务端 RPC：由客户端请求修改颜色，服务端转发给所有人。
    /// 可用于玩家选择颜色后同步到网络。
    /// </summary>
    [ServerRpc]
    private void ColorizeServerRpc(Color color)
    {
        ColorizeClientRpc(color);
    }

    /// <summary>
    /// 客户端 RPC：在所有客户端上应用颜色。
    /// </summary>
    [ClientRpc]
    private void ColorizeClientRpc(Color color)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
           renderer.color = color;
        }
    }
}
