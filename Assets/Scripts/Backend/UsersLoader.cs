using System.Threading;
using UnityEngine;

public class UsersLoader : MonoBehaviour
{
    private RestService restService;

    private string baseUri;

    private UserResponse _user;
    
    private void Start()
    {
        // TODO: Load api URIs from config file
//        baseUri = "https://api.mountainpier.ru";
        baseUri = "http://localhost:8548";
        
        if (restService == null)
            restService = GameObject.FindObjectOfType<RestService>();
        
//        GetUserByUsername("matthewpoletin");
        GetUserById(1);
    }

    private void GetUsers()
    {
        string uri = baseUri + "/users";
        restService.Request<Page<UserResponse>>(RestService.Method.Get, uri, null, Debug.Log);
    }

    private void GetUserById(int userId)
    {
        if (restService == null)
        {
            Debug.Log("RestService not found");
        }
        else
        {
            string uri = baseUri + "/users/" + userId;
            restService.Request<UserResponse>(RestService.Method.Get, uri, null, Debug.Log);        
        }
    }

    private void GetUserByUsername(string username)
    {
        if (restService == null)
        {
            Debug.Log("RestService not found");
        }
        else
        {
//            string uri = baseUri + "/user/by?username=" + username;
            string uri = baseUri + "/users/";
            restService.Request<UserResponse>(RestService.Method.Get, uri, null, LoadUserCallback);        
        }
    }

    void LoadUserCallback(UserResponse result)
    {
        this._user = result;
    }
    
}
