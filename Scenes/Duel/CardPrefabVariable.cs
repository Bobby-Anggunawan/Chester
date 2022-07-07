using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefabVariable : MonoBehaviour
{

    public enum Panel {Panel1, Panel3}

    public Panel instanceOfPanel;

    public string id;
    public string imageAssets;
    public string cardName;

    //untuk panel 1==============================================
    public CardData.Card.Type type;
    public int stackCount;
    //END untuk panel 1==========================================

    //untuk panel 3==============================================
    public int index;               //index di list duelGameplay
    public int damageReceive = 0;
    public int authorityCount = 0;
    //END untuk panel 3==========================================

    private void Start()
    {
        //update image di semua panel
        transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(imageAssets);

        if (instanceOfPanel == Panel.Panel1)
        {
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(stackCount.ToString());
        }
        else if (instanceOfPanel == Panel.Panel3) {
            if (authorityCount == 0)
            {
                var temp = transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 0;
                transform.GetChild(0).GetComponent<Image>().color = temp;

                temp = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                temp.a = 0;
                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = temp;
            }
            else {
                var temp = transform.GetChild(0).GetComponent<Image>().color;
                temp.a = 1;
                transform.GetChild(0).GetComponent<Image>().color = temp;

                temp = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                temp.a = 1;
                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = temp;

                transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(authorityCount.ToString());
            }
            if (damageReceive == 0)
            {
                var temp = transform.GetChild(1).GetComponent<Image>().color;
                temp.a = 0;
                transform.GetChild(1).GetComponent<Image>().color = temp;

                temp = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                temp.a = 0;
                transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = temp;
            }
            else {
                var temp = transform.GetChild(1).GetComponent<Image>().color;
                temp.a = 1;
                transform.GetChild(1).GetComponent<Image>().color = temp;

                temp = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color;
                temp.a = 1;
                transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = temp;

                transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(damageReceive.ToString());
            }
        }
    }

    private void Update()
    {
    }
}
