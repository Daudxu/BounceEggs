using UnityEngine;
using Unity.Netcode;
public class PlayerStateManager : NetworkBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer[] renderers;
    [SerializeField] private Collider2D playerCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable()
    {
        EnableClientRpc();
    }

    [ClientRpc]
    private void EnableClientRpc()
    {
        playerCollider.enabled = true;
         foreach(SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = 1f;
            renderer.color = color;
        }
    }

    public void Disable()
    {
        DisableClientRpc();
    }

    [ClientRpc]
    private void DisableClientRpc()
    {
        playerCollider.enabled = false;
        foreach(SpriteRenderer renderer in renderers)
        {
            Color color = renderer.color;
            color.a = .2f;
            renderer.color = color;
        }
    }


}
