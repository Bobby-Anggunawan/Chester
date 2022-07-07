using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class loginPage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            StartCoroutine(postID(PlayGamesPlatform.Instance.GetUserId()));

        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    IEnumerator postID(string id) {
        UnityWebRequest uwr = UnityWebRequest.Post($"https://chester-game.uc.r.appspot.com/user/login/{id}", "");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success) {
            SceneManager.LoadScene("Home");
        }
    }
}
