using UnityEngine;
using Unity.Netcode.Components;

namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    public class ClientNetworkTransform : NetworkTransform
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
 