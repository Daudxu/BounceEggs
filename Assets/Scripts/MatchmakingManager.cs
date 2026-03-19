using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
public class MatchmakingManager : MonoBehaviour
{
    Lobby lobby;
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
            // return await LobbyService.Instance.QuickJoinLobbyAsync();
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            return lobby;
        }catch(Exception e){
            Debug.Log(e);
            return null;
        }
    }

    private async Task<Lobby> CreateLobby()
    {
        // return await LobbyService.Instance.CreateLobbyAsync();
          return null;
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
