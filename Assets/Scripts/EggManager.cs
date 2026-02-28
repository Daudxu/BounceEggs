using UnityEngine;
using Unity.Netcode;

public class EggManager : NetworkBehaviour
{

    public static EggManager instance;
    [Header("Elements")]
    [SerializeField] private GameObject eggPrefab;

    private void Awake()
    {
         if (instance == null)
         {
            instance = this;
         }
         else
         {
            Destroy(gameObject);
         }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                SpawnEgg();
                break;
        }
    }

    private void SpawnEgg()
    {
        if (!IsServer) return;
        if (eggPrefab == null)
        {
            Debug.LogError("EggManager: eggPrefab 未赋值！请在 Inspector 中把 Egg 预制体拖到 EggManager 的 Egg Prefab 字段。");
            return;
        }
        var eggObj = Instantiate(eggPrefab, Vector2.up * 5, Quaternion.identity);
        if (eggObj.TryGetComponent<NetworkObject>(out var netObj))
            netObj.Spawn();
        eggObj.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReuseEgg()
    {
        if (!IsServer) return;
        if(transform.childCount <= 0)  return;
        transform.GetChild(0).GetComponent<Egg>().Reset();
    }
}
