using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataTipe_CardPrefabCode {
    public enum InstanceOfScrool {
        top, buttom
    }


    static CardData _cardData;
    public static CardData.Card getCardData(string cardId) {
        if (_cardData == null) {
            _cardData = new CardData();
        }
        return _cardData.findCardById(cardId);
    }
}

public class CardsPrefabCode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public DataTipe_CardPrefabCode.InstanceOfScrool instanceOf;
    public string cardID;
    public int stackCount = 0;
    public TextMeshProUGUI textStackCount;

    public CardData.Card cardData;

    public void initiateThisObject(DataTipe_CardPrefabCode.InstanceOfScrool instanceOf, string cardID, int stackCount) {
        this.instanceOf = instanceOf;
        this.cardID = cardID;
        this.stackCount = stackCount;
    }

    //================
    const long lamaHold = 5415950;


    bool isClicked = false;
    long storeTick = 0;
    //================

    // Start is called before the first frame update
    void Start()
    {
        cardData = DataTipe_CardPrefabCode.getCardData(cardID);
        transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(cardData.imageAssets);
        textStackCount.SetText(stackCount.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicked) {
            var beda = Math.Abs(DateTime.Now.Ticks - storeTick);
            if (beda > lamaHold)
            {
                Debug.Log($"{cardID} dipilih setelah: {beda}");
                isClicked = false;
                if (instanceOf == DataTipe_CardPrefabCode.InstanceOfScrool.top) ScrollViewCode.cardToEnterSCBottom = cardID;
                else ScrollViewCode.cardToEnterSCTop = cardID;
            }
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        storeTick = DateTime.Now.Ticks;
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        isClicked = false;
        /*
        var beda = Math.Abs(DateTime.Now.Ticks - storeTick);
        isClicked = false;

        if (beda > lamaHold) {
            Debug.Log($"{cardData.id} dipilih setelah: {beda}");
        }*/
    }
}
