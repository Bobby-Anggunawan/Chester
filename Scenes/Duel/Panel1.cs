using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Panel1 : MonoBehaviour
{

    static public bool handleUpdate = true;

    public GameObject cursor;
    public GameObject previewPanel;
    public GameObject card;

    public GameObject gameplayCode;
    static bool _isActive = true; //saat game pertama dimulai, panel ini sudah aktif
    public static bool isActive {
        get { return _isActive; }
        set {
            if (value)
            {
                _isActive = value;
                Panel2.isActive = false;
                Panel3.isActive = false;
            }
            else _isActive = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (handleUpdate) {
            updateCardInPanel();
            handleUpdate = false;
        }

        //Control====================Start
        if (GamepadMap.activatePanel1) {
            isActive = true;
        }
        if (GamepadMap.draw) {
            duelGamePlay.UserAction.drawCard();
        }

        if (isActive)
        {
            //Jalankan Kursor
            cursor.transform.localPosition = new Vector3(   cursor.transform.localPosition.x + (GamepadMap.leftStick.x * 0.005f),
                                                            cursor.transform.localPosition.y + (GamepadMap.leftStick.y * 0.005f),
                                                            cursor.transform.localPosition.z);
            updateCardInPreview();

            //SUMMON
            if (GamepadMap.trigger)
            {
                var colide = repo.raycastToPanel(cursor);
                if (colide != null)
                {
                    var getType = colide.GetComponent<CardPrefabVariable>().type;
                    if (getType == CardData.Card.Type.creature) duelGamePlay.UserAction.summonMonster(colide.GetComponent<CardPrefabVariable>().id);
                    else if (getType == CardData.Card.Type.authority) duelGamePlay.UserAction.setAuthority(colide.GetComponent<CardPrefabVariable>().id);
                }
            }
        }
        //Control====================End
    }

    void updateCardInPreview() {
        var ray = repo.raycastToPanel(cursor);
        if (ray != null) {
            if (ray.tag == "Card") previewPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>(ray.GetComponent<CardPrefabVariable>().imageAssets);
        }
    }
    
    private void updateCardInPanel()
    {

        repo.destroyAllChild(transform.gameObject);

        for (int x = 0; x < duelGamePlay.cardInHand.Count; x++) {
            var aCard = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            CardPrefabVariable cardVar = aCard.GetComponent<CardPrefabVariable>();

            CardData.Card getInfo = duelGamePlay.fetchAllCardData.findCardById(duelGamePlay.cardInHand[x].id);
            cardVar.id = getInfo.id;
            cardVar.type = getInfo.type;
            cardVar.cardName = getInfo.name;
            cardVar.name = getInfo.name;
            cardVar.imageAssets = getInfo.imageAssets;
            cardVar.stackCount = duelGamePlay.cardInHand[x].duplicate;

            cardVar.instanceOfPanel = CardPrefabVariable.Panel.Panel1;

            aCard.transform.SetParent(transform, false);
        }
    }
}
