using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeCode : MonoBehaviour
{

    public GameObject namaUser;

    // Start is called before the first frame update
    void Start()
    {
        namaUser.GetComponent<TextMeshProUGUI>().SetText(PlayGamesPlatform.Instance.GetUserDisplayName());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame() {
        SceneManager.LoadScene("Matchmaking");
    }

    //cuma untuk test
    //TODO HAPUS INI
    public void OpenDuel() {
        SceneManager.LoadScene("Duel");
    }

    public void OpenSetting() {
        SceneManager.LoadScene("Setting");
    }
}
