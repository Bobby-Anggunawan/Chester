using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DuelFirestore : MonoBehaviour
{

    public GameObject debugText;

    FirebaseFirestore _db;
    FirebaseFirestore db
    {
        get
        {
            if (_db == null) _db = FirebaseFirestore.DefaultInstance;
            return _db;
        }
    }
    public static string roomID = "FUywMc5yoeofxfFl1lQ7";
    public static bool meIsBlue = true;
    public int lastFirestoreEventIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Listen Realtime Update Dari Musuh
        try
        {
            Query query = db.Collection("roomEvents").WhereEqualTo("roomID", roomID);
            ListenerRegistration listener = query.Listen(snapshot =>
            {
                debugText.GetComponent<TextMeshPro>().SetText($"Mulai Listen\n{roomID}\n{DateTime.Now}");

                foreach (DocumentChange change in snapshot.GetChanges())
                {
                    if (change.ChangeType == DocumentChange.Type.Added)
                    {
                        var dokumenBaru = change.Document.ToDictionary();
                        int functionCode = Convert.ToInt32(dokumenBaru["function"]);

                        int eventIndex = Convert.ToInt32(dokumenBaru["eventIndex"]);

                        if (eventIndex > lastFirestoreEventIndex) lastFirestoreEventIndex = eventIndex;

                        //SUMMON MONSTER
                        if (functionCode == 1)
                        {
                            string monsterID = (string)dokumenBaru["monsterID"];
                            int selectedSlot = Convert.ToInt32(dokumenBaru["selectedSlot"]);

                            debugText.GetComponent<TextMeshPro>().SetText($"{monsterID}\n{selectedSlot}");

                            duelGamePlay.GameplayEvent.monsterSummoned(monsterID, selectedSlot);
                        }
                        //MONSTER MOVING
                        else if (functionCode == 2)
                        {

                            int destinationX = Convert.ToInt32(dokumenBaru["destinationX"]);
                            int destinationY = Convert.ToInt32(dokumenBaru["destinationY"]);

                            int startX = Convert.ToInt32(dokumenBaru["startX"]);
                            int startY = Convert.ToInt32(dokumenBaru["startY"]);

                            Vector2Int destination = new Vector2Int(destinationX, destinationY);
                            Vector2Int start = new Vector2Int(startX, startY);

                            debugText.GetComponent<TextMeshPro>().SetText($"Monster di {destinationX}, {destinationY} pindah");

                            duelGamePlay.GameplayEvent.monsterMoving(destination, start);
                        }
                        //MONSTER ATTACK
                        else if (functionCode == 3)
                        {

                            int destinationX = Convert.ToInt32(dokumenBaru["destinationX"]);
                            int destinationY = Convert.ToInt32(dokumenBaru["destinationY"]);

                            int startX = Convert.ToInt32(dokumenBaru["startX"]);
                            int startY = Convert.ToInt32(dokumenBaru["startY"]);

                            int attackIndex = Convert.ToInt32(dokumenBaru["attackIndex"]);

                            Vector2Int destination = new Vector2Int(destinationX, destinationY);
                            Vector2Int start = new Vector2Int(startX, startY);

                            debugText.GetComponent<TextMeshPro>().SetText($"Monster di {destinationX}, {destinationY} menyerang");

                            duelGamePlay.GameplayEvent.monsterAttacking(destination, start, attackIndex);
                        }
                    }
                }
            });
        }
        catch (Exception e)
        {
            debugText.GetComponent<TextMeshPro>().SetText($"{e.GetType()}\n{e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void publishSummonMonster(string monsterID, int selectedSlot)
    {
        DocumentReference docRef = db.Collection("roomEvents").Document();
        lastFirestoreEventIndex++;
        Dictionary<string, object> eventDuel = new Dictionary<string, object>
        {
            {"eventIndex", lastFirestoreEventIndex },
            {"function", 1 },
            {"monsterID",  monsterID},
            {"roomID", roomID },
            {"selectedSlot", selectedSlot}
        };

        docRef.SetAsync(eventDuel).ContinueWithOnMainThread(task => {
            debugText.GetComponent<TextMeshPro>().SetText($"{monsterID} berhasil dikirim");
        });

    }

    public void publishMovingMonster(Vector2Int destination, Vector2Int start)
    {
        DocumentReference docRef = db.Collection("roomEvents").Document();
        lastFirestoreEventIndex++;
        Dictionary<string, object> eventDuel = new Dictionary<string, object>
        {
            {"eventIndex", lastFirestoreEventIndex },
            {"function", 2 },
            {"roomID", roomID },
            {"destinationX", destination.x},
            {"destinationY", destination.y},
            {"startX", start.x},
            {"startY", start.y}
        };

        docRef.SetAsync(eventDuel).ContinueWithOnMainThread(task => {
            debugText.GetComponent<TextMeshPro>().SetText($"gerakan {destination.x}, {destination.y} berhasil dikirim");
        });
    }

    public void publishAttackMonster(Vector2Int destination, Vector2Int start, int attackIndex)
    {
        DocumentReference docRef = db.Collection("roomEvents").Document();
        lastFirestoreEventIndex++;
        Dictionary<string, object> eventDuel = new Dictionary<string, object>
        {
            {"eventIndex", lastFirestoreEventIndex },
            {"function", 3 },
            {"roomID", roomID },
            {"destinationX", destination.x},
            {"destinationY", destination.y},
            {"startX", start.x},
            {"startY", start.y},
            {"attackIndex", attackIndex}
        };

        docRef.SetAsync(eventDuel).ContinueWithOnMainThread(task => {
            debugText.GetComponent<TextMeshPro>().SetText($"serangan {destination.x}, {destination.y} berhasil dikirim");
        });
    }

}
