using Firebase.Firestore;
using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MachmakingCode : MonoBehaviour
{

    ListenerRegistration listenWaitPlayer;
    bool startFirestore = false;
    bool openingDuelPage = false;

    public GameObject waitTime;

    long startCountTime;

    // Start is called before the first frame update
    void Start()
    {
        startCountTime = DateTime.Now.Ticks;

        StartCoroutine(searchOpponent());
    }

    // Update is called once per frame
    void Update()
    {
        long tickThisFrame = DateTime.Now.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(tickThisFrame - startCountTime);

        waitTime.GetComponent<TextMeshProUGUI>().SetText(Convert.ToInt32(elapsedSpan.TotalSeconds).ToString());

        if (startFirestore) {
            DocumentReference roomDoc = repo.db.Collection("duel").Document(DuelFirestore.roomID);
            listenWaitPlayer = roomDoc.Listen(snapshot => {
                if (snapshot.ContainsField("redPlayerID"))
                {
                    openingDuelPage = true;
                }
            });

            startFirestore = false;
        }
        if (openingDuelPage) {
            openingDuelPage = false;
            openDuelPage();
        }
    }

    void openDuelPage() {
        //debugText.GetComponent<TextMeshProUGUI>().SetText(pesan);
        try
        {
            listenWaitPlayer.Stop();
            listenWaitPlayer.Dispose();
            StopAllCoroutines();
        }
        catch
        {
            try
            {
                listenWaitPlayer.Dispose();
                StopAllCoroutines();
            }
            catch {
                StopAllCoroutines();
            }
        }
        SceneManager.LoadScene("Duel");
    }

    IEnumerator searchOpponent()
    {
        string getUserID = PlayGamesPlatform.Instance.GetUserId();
        UnityWebRequest uwr = UnityWebRequest.Get($"https://chester-game.uc.r.appspot.com/matchmaking/{getUserID}");
        yield return uwr.SendWebRequest();

        
        if (uwr.result == UnityWebRequest.Result.Success)
        {
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(uwr.downloadHandler.text);

            DuelFirestore.roomID = (string)jsonData["roomID"];
            DuelFirestore.meIsBlue = Convert.ToBoolean(jsonData["isBlue"]);


            if (DuelFirestore.meIsBlue == false)
            {
                openingDuelPage = true;
            }
            else {
                //start FORESTORE=========================
                //tunggu pemain lain
                startFirestore = true;
            }
        }
    }
}
