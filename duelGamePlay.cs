using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;

public class duelGamePlay : MonoBehaviour
{

    public static CardData fetchAllCardData = new CardData();

    //kartu dalam tumpukan deck, belum di draw
    //kartu paling atas ada di bagian akhir, kartu paling bawah di awal/index 0;
    public static List<string> cardInDeck = new List<string>()
    {
        "0006", "0005", "0003", "0007", "0006", "0007", "0001", "0001", "0007", "0003", "0002", "0004",
        "0003", "0006", "0006", "0006", "0004", "0001", "0005", "0006", "0004", "0003", "0007", "0004",
        "0002", "0004", "0007", "0005", "0002", "0006", "0003", "0006", "0001", "0002", "0006", "0005",
    };
    //kartu dalam graveyard
    //kartu paling atas ada di bagian akhir, kartu paling bawah di awal/index 0;
    public static List<string> cardInGrave = new List<string>() {};

    public static List<HandData> cardInHand = new List<HandData>() {};


    /// <summary>
    /// Berisi info tentang creature di arena, seperti hp, jumlah authority, dll
    /// </summary>
    static public List<Panel3Data> cardInP3 = new List<Panel3Data>()
    {
        new Panel3Data("Icons/rookCardPlaceholder", CardData.Card.Role.rook, false, new Vector2Int(0, 0)),
        new Panel3Data("Icons/knightCardPlaceholder", CardData.Card.Role.knight, false, new Vector2Int(1, 0)),
        new Panel3Data("Icons/bishopCardPlaceholder", CardData.Card.Role.bishop, false, new Vector2Int(2, 0)),
        new Panel3Data("Icons/bishopCardPlaceholder", CardData.Card.Role.bishop, false, new Vector2Int(3, 0)),
        new Panel3Data("Icons/knightCardPlaceholder", CardData.Card.Role.knight, false, new Vector2Int(4, 0)),
        new Panel3Data("Icons/rookCardPlaceholder", CardData.Card.Role.rook, false, new Vector2Int(5, 0)),
        new Panel3Data("Icons/rookCardPlaceholder", CardData.Card.Role.rook, true, new Vector2Int(0, 5)),
        new Panel3Data("Icons/knightCardPlaceholder", CardData.Card.Role.knight, true, new Vector2Int(1, 5)),
        new Panel3Data("Icons/bishopCardPlaceholder", CardData.Card.Role.bishop, true, new Vector2Int(2, 5)),
        new Panel3Data("Icons/bishopCardPlaceholder", CardData.Card.Role.bishop, true, new Vector2Int(3, 5)),
        new Panel3Data("Icons/knightCardPlaceholder", CardData.Card.Role.knight, true, new Vector2Int(4, 5)),
        new Panel3Data("Icons/rookCardPlaceholder", CardData.Card.Role.rook, true, new Vector2Int(5, 5)),
    };

    /// <summary>
    /// hanya berisi lokasi creature pada cardInP3
    /// </summary>
    static public Panel2Data[,] CardInP2 = new Panel2Data[,] {
        { new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data()},
        { new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data()},
        { new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data()},
        { new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data()},
        { new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data()},
        { new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data(), new Panel2Data()},
    };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public class HandData
    {
        public HandData(string id, int duplicate)
        {
            this.id = id;
            this.duplicate = duplicate;
        }

        public string id;
        public int duplicate;
    }


    public class Panel3Data {
        public Panel3Data(string placeholderImage, CardData.Card.Role role, bool isBlue, Vector2Int startCoordinate) {
            this.id = "0001";
            this.isOccupied = false;
            this.placeholderImage = placeholderImage;
            this.role = role;
            this.isBlue = isBlue; //tim biru atau merah
            this.startCoordinate = startCoordinate;
        }

        public string id;
        public bool isOccupied;
        public string placeholderImage;
        public CardData.Card.Role role;
        public bool isBlue;
        public GameObject monsterPrefab;

        /// <summary>
        /// Untuk mengatur dimana pion akan muncul pertama kali saat disummon di panel 2
        /// </summary>
        public Vector2Int startCoordinate;

        //GAMEPLAY MONSTER
        public int authority = 0;
        public int damageCounterCount = 0; //total damage counter pada creature ini(kayak di pokemon)
    }

    public class Panel2Data {

        public int panel3Index;
        public bool isOccupied = false;

        public string getRoleIcon() {

            var getPanel3Data = cardInP3[panel3Index];

            string roleTemp = "";
            if (getPanel3Data.role == CardData.Card.Role.knight) roleTemp = "horse";
            else if (getPanel3Data.role == CardData.Card.Role.rook) roleTemp = "rook";
            else if (getPanel3Data.role == CardData.Card.Role.bishop) roleTemp = "bishop";

            if (getPanel3Data.isBlue) return $"Icons/{roleTemp}White";
            else return $"Icons/{roleTemp}Black";
        }
    }


    /// <summary>
    /// aksi yang dilakukan suatu user yang membangkitkan event di kelas GameplayEvent
    /// </summary>
    public class UserAction {
        public static void summonMonster(string id) {
            Panel3.isHandlingSummon = true;
            Panel3.cardId = id;
            Panel3.isActive = true;
        }

        public static void setAuthority(string id) {
            Panel3.isHandlingAuthority = true;
            Panel3.cardId = id;
            Panel3.isActive = true;
        }

        /// <summary>
        /// dipanggil kalau berhasil summon monster dari hand
        /// </summary>
        public static void removeCardInHand(string id) {
            for (int x = 0; x < cardInHand.Count; x++) {
                if (cardInHand[x].id == id) {
                    if(cardInHand[x].duplicate == 1) cardInHand.RemoveAt(x);
                    else cardInHand[x].duplicate--;

                    break;
                }
            }
            Panel1.handleUpdate = true;
        }

        public static void cardEnterGraveyard(string id) {
            cardInGrave.Add(id);
        }

        public static void drawCard() {
            if (cardInDeck != null) {
                var insertedCardID = cardInDeck[cardInDeck.Count-1];
                cardInDeck.RemoveAt(cardInDeck.Count - 1);

                bool found = false;
                for (int x = 0; x < cardInHand.Count; x++) {
                    if (cardInHand[x].id == insertedCardID)
                    {
                        cardInHand[x].duplicate++;
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    cardInHand.Add(new HandData(insertedCardID, 1));
                }

                Panel1.handleUpdate = true;
            }
        }
        
    }

    /// <summary>
    /// event yang dikirimkan ke user lewat firestore
    /// </summary>
    public class GameplayEvent
    {
        /// <summary>
        /// fungsi untuk memunculkan monster di arena
        /// </summary>
        /// <param name="monsterID">id kartu monster yang dipanggil</param>
        /// <param name="selectedSlot">index di list panel 3 yang dipilih untuk tempat monster</param>
        public static void monsterSummoned(string monsterID, int selectedSlot) {
            var getData = duelGamePlay.cardInP3[selectedSlot];
            var monster = duelGamePlay.fetchAllCardData.findCardById(monsterID);

            //===============================
            getData.isOccupied = true;
            getData.id = monster.id;
            duelGamePlay.cardInP3[selectedSlot] = getData;
            duelGamePlay.CardInP2[getData.startCoordinate.y, getData.startCoordinate.x].isOccupied = true;
            duelGamePlay.CardInP2[getData.startCoordinate.y, getData.startCoordinate.x].panel3Index = selectedSlot;
            Panel2.updatePawn = true;
            Panel3.updateCards = true;
            //===============================

            //ANIMASI========================
            var prefab = Resources.Load<GameObject>(monster.monsterPrefabAssets);
            prefab.GetComponent<MonsterHandler>().isTeamBlue = getData.isBlue;

            Quaternion rotasiObjek = Quaternion.identity;
            if (!getData.isBlue) rotasiObjek.eulerAngles = new Vector3(0, 180, 0);

            //koordinat monster berbeda dengan koordinat di panel. tidak tahu kenapa harus dibuat beda begini
            duelGamePlay.cardInP3[selectedSlot].monsterPrefab = Instantiate(prefab, new Vector3(    getData.startCoordinate.x * repo.MyConstant.gridSize,
                                                                                                    0,
                                                                                                    (5 - getData.startCoordinate.y) * repo.MyConstant.gridSize),
                                                                                                    rotasiObjek);

            if (getData.isBlue)
            {
                duelGamePlay.cardInP3[selectedSlot].monsterPrefab.GetComponent<MonsterHandler>().mesh.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.blue);
            }
            else {
                duelGamePlay.cardInP3[selectedSlot].monsterPrefab.GetComponent<MonsterHandler>().mesh.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
            }
            //===============================
        }

        /// <summary>
        /// fungsi untuk menggerakkan monster
        /// </summary>
        /// <param name="destination">x=horizontal y=vertical</param>
        /// <param name="start">x=horizontal y=vertical</param>
        public static void monsterMoving(Vector2Int destination, Vector2Int start) {

            var selectedPawn = duelGamePlay.CardInP2[start.x, start.y];

            duelGamePlay.CardInP2[destination.y, destination.x].isOccupied = true;
            duelGamePlay.CardInP2[destination.y, destination.x].panel3Index = selectedPawn.panel3Index;

            duelGamePlay.CardInP2[start.x, start.y].isOccupied = false;

            duelGamePlay.cardInP3[selectedPawn.panel3Index].monsterPrefab.GetComponent<MonsterHandler>().setMoving(new Vector3( destination.x * repo.MyConstant.gridSize,
                                                                                                                                0,
                                                                                                                                (5 - destination.y) * repo.MyConstant.gridSize));
            Panel2.updatePawn = true;
        }

        /// <summary>
        /// fungsi untuk memerintahkan monster menyerang monster lain
        /// </summary>
        /// <param name="destination">sama kayak fungsi monsterMoving</param>
        /// <param name="start">sama kayak fungsi monsterMoving</param>
        /// <param name="attackIndex">attack yang akan dilakukan</param>
        public static void monsterAttacking(Vector2Int destination, Vector2Int start, int attackIndex)
        {
            //COPAS DARI monsterMoving================================
            var selectedPawn = duelGamePlay.CardInP2[start.x, start.y];

            duelGamePlay.CardInP2[destination.y, destination.x].isOccupied = true;
            duelGamePlay.CardInP2[destination.y, destination.x].panel3Index = selectedPawn.panel3Index;

            duelGamePlay.CardInP2[start.x, start.y].isOccupied = false;
            Panel2.updatePawn = true;
            //END.. COPAS DARI monsterMoving================================

            duelGamePlay.cardInP3[selectedPawn.panel3Index].monsterPrefab.GetComponent<MonsterHandler>().setAttacking(new Vector3(  destination.x * repo.MyConstant.gridSize,
                                                                                                                                    0,
                                                                                                                                    (5 - destination.y) * repo.MyConstant.gridSize), attackIndex);
            ///ini original
            //var attackTarget = repo.Validator.canAttack(destination, start, cardInP3[selectedPawn.panel3Index].role);

            ///entah kapan x sama y di vector tertukar
            var attackTarget = repo.Validator.canAttack(    new Vector2Int(destination.y, destination.x),
                                                            new Vector2Int(start.y, start.x),
                                                            cardInP3[selectedPawn.panel3Index].role);

            //Debug.LogError($"di fungsi duelGamePlay.monsterAttacking() bisa attack: {attackTarget.canAttack}, Jumlah musuh {attackTarget.targetsPosition.Count}");
            Debug.LogError($"di fungsi duelGamePlay.monsterAttacking(), destination: {destination.x}, {destination.y}; Start: {start.x}, {start.y}; role: {cardInP3[selectedPawn.panel3Index].role.ToString()}");

            foreach (Vector2Int data in attackTarget.targetsPosition)
            {
                cardInP3[CardInP2[data.x, data.y].panel3Index].damageCounterCount += fetchAllCardData.findCardById(cardInP3[selectedPawn.panel3Index].id).attack[attackIndex].damage;

                Debug.LogError($"di fungsi duelGamePlay.monsterAttacking() mengurangi hp: {cardInP3[CardInP2[data.x, data.y].panel3Index].id}");

                if (cardInP3[CardInP2[data.x, data.y].panel3Index].damageCounterCount >= fetchAllCardData.findCardById(cardInP3[selectedPawn.panel3Index].id).hp)
                {
                    cardInP3[CardInP2[data.x, data.y].panel3Index].monsterPrefab.GetComponent<MonsterHandler>().atacked(true);
                }
                else
                {
                    cardInP3[CardInP2[data.x, data.y].panel3Index].monsterPrefab.GetComponent<MonsterHandler>().atacked(false);
                }
            }


            Panel3.updateCards = true;
        }
    }

}
