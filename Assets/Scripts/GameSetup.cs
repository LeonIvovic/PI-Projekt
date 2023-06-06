using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class GameSetup : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

}
