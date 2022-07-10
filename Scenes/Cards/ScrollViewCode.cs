using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DataTipe_ScrollViewCode {
    public class CardPrefabData {
        public CardPrefabData(string cardID, int stackCount) {
            this.cardID = cardID;
            this.stackCount = stackCount;
        }
        public string cardID;
        public int stackCount;
    }
}

public class ScrollViewCode : MonoBehaviour, IPointerDownHandler
{
    public DataTipe_CardPrefabCode.InstanceOfScrool instanceOf;
    public GameObject cardPrefab;

    public static string cardToEnterSCBottom;   //daftra kartu
    public static string cardToEnterSCTop;      //deck

    public static bool SC1ShouldReload = false;
    public static bool SC2ShouldReload = false;

    public static List<DataTipe_ScrollViewCode.CardPrefabData> listCardTop = new List<DataTipe_ScrollViewCode.CardPrefabData>() {
        new DataTipe_ScrollViewCode.CardPrefabData("0001", 3),
        new DataTipe_ScrollViewCode.CardPrefabData("0002", 5),
        new DataTipe_ScrollViewCode.CardPrefabData("0003", 1)
    };
    public static List<DataTipe_ScrollViewCode.CardPrefabData> listCardBottom = new List<DataTipe_ScrollViewCode.CardPrefabData>() {
        new DataTipe_ScrollViewCode.CardPrefabData("0004", 8),
        new DataTipe_ScrollViewCode.CardPrefabData("0005", 3),
        new DataTipe_ScrollViewCode.CardPrefabData("0006", 2)
    };

    // Start is called before the first frame update
    void Start()
    {
        justUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (instanceOf == DataTipe_CardPrefabCode.InstanceOfScrool.top)
        {
            if (SC1ShouldReload) justUpdate();
            SC1ShouldReload = false;
        }
        else {
            if (SC2ShouldReload) justUpdate();
            SC2ShouldReload = false;
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log("tekan scrollview "+ instanceOf.ToString());
        if (instanceOf == DataTipe_CardPrefabCode.InstanceOfScrool.top) {
            if (cardToEnterSCTop != null) updateAddThisScrollItem();
        }
        if (instanceOf == DataTipe_CardPrefabCode.InstanceOfScrool.buttom) {
            if(cardToEnterSCBottom !=null) updateAddThisScrollItem();
        }
    }

    void justUpdate() {

        repo.destroyAllChild(transform.gameObject);

        if (instanceOf == DataTipe_CardPrefabCode.InstanceOfScrool.top)
        {
            foreach (DataTipe_ScrollViewCode.CardPrefabData data in listCardTop)
            {
                var aCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                aCard.GetComponent<CardsPrefabCode>().initiateThisObject(instanceOf, data.cardID, data.stackCount);
            }
        }
        else
        {
            foreach (DataTipe_ScrollViewCode.CardPrefabData data in listCardBottom)
            {
                var aCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                aCard.GetComponent<CardsPrefabCode>().initiateThisObject(instanceOf, data.cardID, data.stackCount);
            }
        }
    }

    void updateAddThisScrollItem() {

        repo.destroyAllChild(transform.gameObject);

        if (instanceOf == DataTipe_CardPrefabCode.InstanceOfScrool.top) {
            //Tambah item ke list
            bool ketemu = false;
            for (int x=0; x< listCardTop.Count; x++) {
                if (listCardTop[x].cardID == cardToEnterSCTop) {
                    listCardTop[x].stackCount++;
                    ketemu = true;
                }
                var aCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                aCard.GetComponent<CardsPrefabCode>().initiateThisObject(instanceOf, listCardTop[x].cardID, listCardTop[x].stackCount);
            }
            if (!ketemu)
            {
                listCardTop.Add(new DataTipe_ScrollViewCode.CardPrefabData(cardToEnterSCTop, 1));
                var aCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                aCard.GetComponent<CardsPrefabCode>().initiateThisObject(instanceOf, cardToEnterSCTop, 1);
            }
            for (int x = 0; x < listCardBottom.Count; x++) {
                if (listCardBottom[x].cardID == cardToEnterSCTop) {
                    if (listCardBottom[x].stackCount == 1) listCardBottom.RemoveAt(x);
                    else listCardBottom[x].stackCount--;
                    SC2ShouldReload = true;
                    break;
                }
            }

            cardToEnterSCTop = null;


        }
        else if (instanceOf == DataTipe_CardPrefabCode.InstanceOfScrool.buttom)
        {
            //Tambah item ke list
            bool ketemu = false;
            for (int x = 0; x < listCardBottom.Count; x++)
            {
                if (listCardBottom[x].cardID == cardToEnterSCBottom)
                {
                    listCardBottom[x].stackCount++;
                    ketemu = true;
                }
                var aCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                aCard.GetComponent<CardsPrefabCode>().initiateThisObject(instanceOf, listCardBottom[x].cardID, listCardBottom[x].stackCount);
            }
            if (!ketemu)
            {
                listCardBottom.Add(new DataTipe_ScrollViewCode.CardPrefabData(cardToEnterSCBottom, 1));
                var aCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                aCard.GetComponent<CardsPrefabCode>().initiateThisObject(instanceOf, cardToEnterSCBottom, 1);
            }
            for (int x = 0; x < listCardTop.Count; x++)
            {
                if (listCardTop[x].cardID == cardToEnterSCBottom)
                {
                    if (listCardTop[x].stackCount == 1) listCardTop.RemoveAt(x);
                    else listCardTop[x].stackCount--;
                    SC1ShouldReload = true;
                    break;
                }
            }

            cardToEnterSCBottom = null;
        }
    }
}
