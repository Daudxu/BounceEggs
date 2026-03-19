using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Networking.Transport;
using Unity.Services.Lobbies.Http;
public class MatchmakingManager : MonoBehaviour
{
    Lobby lobby;

    [Header("Settings")]
    [SerializeField] private string _joinCode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void PlayButtonCallback()
    {
        await AuthenticateAsync();
        lobby = (await QuickJoinLobby()) ?? (await CreateLobby());
    }

    private async Task<Lobby> QuickJoinLobby() 
    {
        try{
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(lobby.Data["_joinCode"].Value);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4, 
                (ushort) allocation.RelayServer.Port, 
                allocation.AllocationIdBytes, 
                allocation.Key,
                allocation.ConnectionData,
                allocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();
            return lobby;
        }catch(Exception e){
            Debug.Log(e);
            return null;
        }
    }

    private async Task<Lobby> CreateLobby()
    {
        try{
            int maxPlayers = 2;
            string lobbyName = "MyCoolLobby";
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.Data = new Dictionary<string, DataObject>{
                { "_joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode)}
            };
            return lobby;
        }catch(Exception e){
            Debug.Log(e);
            return null;
        }
    }

    async Task AuthenticateAsync() 
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            var playerID = AuthenticationService.Instance.PlayerId;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


}
