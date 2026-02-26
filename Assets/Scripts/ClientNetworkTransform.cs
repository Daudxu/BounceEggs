using UnityEngine;
using Unity.Netcode.Components;

namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    /// <summary>
    /// 客户端权威的 NetworkTransform：由客户端控制位置同步，服务端不校正。
    /// 适用于玩家角色等需要即时响应的物体，减少输入延迟感。
    /// </summary>
    public class ClientNetworkTransform : NetworkTransform
    {
        /// <summary>
        /// 返回 false 表示使用客户端权威：客户端上报的位置直接同步到其他客户端，服务端不覆盖。
        /// </summary>
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
 