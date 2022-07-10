using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Panel3 : MonoBehaviour
{

    //public GameObject debugText;


    static bool _isActive = false;
    public static bool isActive
    {
        get { return _isActive; }
        set
        {
            if (value)
            {
                _isActive = value;
                Panel2.isActive = false;
                Panel1.isActive = false;
            }
            else _isActive = value;
        }
    }

    //=======================================
    /// <summary>
    /// apakah user ingin summon monster atau pasang authority
    /// </summary>
    public static bool isHandlingSummon = false;
    public static bool isHandlingAuthority = false;
    /// <summary>
    /// dipakai untuk summon monster jika isHandlingSummon == true
    /// </summary>
    public static string cardId = "0001";
    //=======================================

    public GameObject gameplayCode;
    public GameObject card;
    public GameObject gridView;

    public GameObject cursor;
    public GameObject previewPanel;

    /// <summary>
    /// fungsi untuk mengaktifkan fungsi update di panel ini dari kelas lain
    /// </summary>
    public static bool updateCards = true;

    // Start is called before the first frame update
    void Start()
    {

        updateCard();
    }

    // Update is called once per frame
    void Update()
    {
        //Control====================Start
        if (GamepadMap.activatePanel3) isActive = true;


        if (isActive)
        {
            //geraking pointer
            cursor.transform.localPosition = new Vector3(   cursor.transform.localPosition.x + (GamepadMap.leftStick.x * 0.005f),
                                                            cursor.transform.localPosition.y + (GamepadMap.leftStick.y * 0.005f),
                                                            cursor.transform.localPosition.z);
            var colide = repo.raycastToPanel(cursor);
            if (colide != null)
            {
                if (colide.tag == "Card")
                {
                    var getData = duelGamePlay.cardInP3[colide.GetComponent<CardPrefabVariable>().index];
                    if (getData.isOccupied) previewPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>(duelGamePlay.fetchAllCardData.findCardById(getData.id).imageAssets);
                }
            }
            //=====Geraking pointer end

            if (GamepadMap.trigger) {
                if (isHandlingSummon)
                {
                    var collide = repo.raycastToPanel(cursor);

                    if (collide != null)
                    {
                        if (collide.tag == "Card")
                        {
                            var getDataIndex = collide.GetComponent<CardPrefabVariable>().index;
                            var getData = duelGamePlay.cardInP3[getDataIndex];
                            if (getData.isOccupied == false)
                            {
                                if (duelGamePlay.CardInP2[getData.startCoordinate.x, getData.startCoordinate.y].isOccupied == false)
                                {
                                    var monster = duelGamePlay.fetchAllCardData.findCardById(cardId);
                                    if (monster.role == getData.role)
                                    {
                                        if (DuelFirestore.meIsBlue == getData.isBlue)
                                        {
                                            isHandlingSummon = false;

                                            duelGamePlay.UserAction.removeCardInHand(monster.id);
                                            gameplayCode.GetComponent<DuelFirestore>().publishSummonMonster(monster.id, getDataIndex);


                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                else if (isHandlingAuthority)
                {
                    var collide = repo.raycastToPanel(cursor);
                    if (collide != null)
                    {
                        if (collide.tag == "Card")
                        {
                            var getDataIndex = collide.GetComponent<CardPrefabVariable>().index;
                            var getData = duelGamePlay.cardInP3[getDataIndex];
                            if (getData.isOccupied == true)
                            {
                                isHandlingAuthority = false;
                                getData.authority += duelGamePlay.fetchAllCardData.findCardById(cardId).count;
                                updateCard();

                                Debug.Log($"{getData.id} authority+");
                            }
                        }
                    }
                }
            }
        }
        //Control====================END

        if (updateCards) {
            updateCard();
            updateCards = false;
        }
    }

    void updateCard() {
        repo.destroyAllChild(gridView);

        for (int x = 0; x < duelGamePlay.cardInP3.Count; x++) {

            var data = duelGamePlay.cardInP3[x];

            var aCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);

            aCard.GetComponent<CardPrefabVariable>().instanceOfPanel = CardPrefabVariable.Panel.Panel3;

            if (data.isOccupied) {
                aCard.GetComponent<CardPrefabVariable>().authorityCount = data.authority;
                aCard.GetComponent<CardPrefabVariable>().damageReceive = data.damageCounterCount;
                aCard.GetComponent<CardPrefabVariable>().imageAssets = duelGamePlay.fetchAllCardData.findCardById(data.id).imageAssets;
            }
            else {
                aCard.GetComponent<CardPrefabVariable>().imageAssets = data.placeholderImage;
            }

            aCard.GetComponent<CardPrefabVariable>().index = x;
            aCard.transform.SetParent(gridView.transform, false);
        }
    }
}
