using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Panel2 : MonoBehaviour
{

    static bool _isActive = false;
    public static bool isActive
    {
        get { return _isActive; }
        set
        {
            if (value)
            {
                _isActive = value;
                Panel1.isActive = false;
                Panel3.isActive = false;
            }
            else _isActive = value;
        }
    }


    public GameObject gameplayCode;

    public GameObject blueSelector;
    private GameObject selector;        //cuma dipakai nampung hasil clone blueSelector
    public GameObject pawn;
    public GameObject board;            //untuk nampung pawn, karena kalau angsung ke parrent nanti gak kelihatan karena parrentnya transparan
    public GameObject moveList;
    public GameObject aMove;

    const float gridWidth = 0.1666667f;

    private int verticalIndex = 0;      //atas ke bawah
    private int horizontalIndex = 0;    //kiri ke kanan

    /// <summary>
    /// kalau true, dungsi Update() akan memanggil fungsi updatePawn()
    /// </summary>
    public static bool updatePawn = true;
    public static bool updateAttackSelector = false;
    public static bool handleAttack = false;
    public static bool chosingAttack = false;

    public static Vector2Int attackingPawnIndex = new Vector2Int(0, 0);
    public static int selectedMoveIndex = 0; //index serangan yang akan dilakukan monster ke monster lain
    public static string attackingMonsterID = "0001";


    // Start is called before the first frame update
    void Start()
    {
        selector = Instantiate(blueSelector, new Vector3(0, 0, 0), Quaternion.identity);
        selector.transform.parent = gameObject.transform;
        selector.transform.localPosition = new Vector3((gridWidth * (horizontalIndex - 2)) - (gridWidth / 2), (gridWidth * (3 - verticalIndex)) - (gridWidth / 2), -0.01f);
        selector.transform.localRotation = Quaternion.Euler(0, 0, 0);
        selector.transform.localScale = new Vector3(0.1666667f, 0.1666667f, 0.06850646f);
    }

    // Update is called once per frame
    void Update()
    {
        if (updatePawn) {
            _updatePawn();
        }
        if (updateAttackSelector)
        {
            _updateAttackSelector();
        }

        //Control====================Start
        if (GamepadMap.activatePanel2) isActive = true;

        if (GamepadMap.dirRight) {
            if (!chosingAttack)
            {
                if (horizontalIndex < 5 && isActive)
                {
                    horizontalIndex += 1;
                    selector.transform.localPosition = new Vector3((gridWidth * (horizontalIndex - 2)) - (gridWidth / 2), selector.transform.localPosition.y, -0.01f);
                }
            }
        }
        if (GamepadMap.dirLeft) {
            if (!chosingAttack)
            {
                if (horizontalIndex > 0 && isActive)
                {
                    horizontalIndex -= 1;
                    selector.transform.localPosition = new Vector3((gridWidth * (horizontalIndex - 2)) - (gridWidth / 2), selector.transform.localPosition.y, -0.01f);
                }
            }
        }
        if (GamepadMap.dirUp) {
            if (!chosingAttack)
            {
                if (verticalIndex > 0 && isActive)
                {
                    verticalIndex -= 1;
                    selector.transform.localPosition = new Vector3(selector.transform.localPosition.x, (gridWidth * (3 - verticalIndex)) - (gridWidth / 2), -0.01f);
                }
            }
            else
            {
                if (selectedMoveIndex > 0)
                {
                    selectedMoveIndex--;
                    updateAttackSelector = true;
                }
            }
        }
        if (GamepadMap.dirDown) {
            if (!chosingAttack)
            {
                if (verticalIndex < 5 && isActive)
                {
                    verticalIndex += 1;
                    selector.transform.localPosition = new Vector3(selector.transform.localPosition.x, (gridWidth * (3 - verticalIndex)) - (gridWidth / 2), -0.01f);
                }
            }
            else
            {
                if (selectedMoveIndex < duelGamePlay.fetchAllCardData.findCardById(attackingMonsterID).attack.Length - 1)
                {
                    selectedMoveIndex++;
                    updateAttackSelector = true;
                }
            }
        }
        if (GamepadMap.trigger) {
            if (isActive)
            {
                //pilih pion yang mau digerakkan
                if (handleAttack == false && chosingAttack == false)
                {
                    if (duelGamePlay.CardInP2[verticalIndex, horizontalIndex].isOccupied == true)
                    {
                        attackingPawnIndex.x = verticalIndex;
                        attackingPawnIndex.y = horizontalIndex;

                        var selectedPawn = duelGamePlay.CardInP2[attackingPawnIndex.x, attackingPawnIndex.y];
                        if (DuelFirestore.meIsBlue == duelGamePlay.cardInP3[selectedPawn.panel3Index].isBlue)
                            handleAttack = true;
                    }
                }
                else if (chosingAttack)
                {
                    chosingAttack = false;
                    handleAttack = false;
                    updateAttackSelector = true;

                    gameplayCode.GetComponent<DuelFirestore>().publishAttackMonster(new Vector2Int(horizontalIndex, verticalIndex), attackingPawnIndex, selectedMoveIndex);

                    selectedMoveIndex = 0;
                }
                //pindahkan pion ke posisi ini
                else
                {
                    var selectedPawn = duelGamePlay.CardInP2[attackingPawnIndex.x, attackingPawnIndex.y];
                    var allowToMove = repo.Validator.isMoveValid(duelGamePlay.cardInP3[selectedPawn.panel3Index].role, new Vector2Int(attackingPawnIndex.x, attackingPawnIndex.y), new Vector2Int(verticalIndex, horizontalIndex));

                    if (allowToMove)
                    {
                        var canAttack = repo.Validator.canAttack(new Vector2Int(verticalIndex, horizontalIndex), new Vector2Int(attackingPawnIndex.y, attackingPawnIndex.x), duelGamePlay.cardInP3[selectedPawn.panel3Index].role);

                        //Debug.LogError($"di Panel2, bisa attack: {canAttack.canAttack}, Jumlah musuh {canAttack.targetsPosition.Count}");
                        Debug.LogError($"di Panel2, destination: {verticalIndex}, {horizontalIndex}; Start: {attackingPawnIndex.y}, {attackingPawnIndex.x}; role: {duelGamePlay.cardInP3[selectedPawn.panel3Index].role.ToString()}");

                        if (canAttack.canAttack == false)
                        {
                            handleAttack = false;

                            gameplayCode.GetComponent<DuelFirestore>().publishMovingMonster(new Vector2Int(horizontalIndex, verticalIndex), attackingPawnIndex);
                        }
                        else
                        {
                            chosingAttack = true;
                            updateAttackSelector = true;
                            attackingMonsterID = duelGamePlay.cardInP3[selectedPawn.panel3Index].id;
                        }
                    }
                }
            }
        }
        //Control====================End
    }


    Vector3 getSpawnPosition(int indexVer, int indexHor) {
        if (indexVer > -1 && indexVer < 6 && indexHor > -1 && indexHor < 6) {
            return new Vector3((gridWidth * (indexHor - 2)) - (gridWidth / 2), (gridWidth * (3 - indexVer)) - (gridWidth / 2), -0.01f);
        }
        return Vector3.zero;
    }
    void _updatePawn() {

        repo.destroyAllChild(board);

        for (int x = 0; x < 6; x++) {
            for (int y = 0; y < 6; y++) {
                var tile = duelGamePlay.CardInP2[x, y];
                if (tile.isOccupied == true) {
                    var aPawn = Instantiate(pawn, new Vector3(0, 0, 0), Quaternion.identity);
                    aPawn.transform.SetParent(board.transform, false);
                    aPawn.transform.localPosition = getSpawnPosition(x, y);
                    aPawn.GetComponent<Image>().sprite = Resources.Load<Sprite>(tile.getRoleIcon());
                }
            }
        }

        //berhenti memanggil fungsi ini di fungsi Update()
        updatePawn = false;
    }

    void _updateAttackSelector() {
        repo.destroyAllChild(moveList);

        if (chosingAttack) {
            var data = duelGamePlay.fetchAllCardData.findCardById(attackingMonsterID).attack;
            for (int x = 0; x < data.Length; x++)
            {
                var move = Instantiate(aMove, new Vector3(0, 0, 0), Quaternion.identity);
                move.transform.SetParent(moveList.transform, false);
                move.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(data[x].name);
                move.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(data[x].damage.ToString());
                if (selectedMoveIndex == x)
                {
                    var temp = move.GetComponent<Image>().color;
                    temp.r = 0.39f;
                    temp.g = 0.33f;
                    temp.b = 0.2f;
                    move.GetComponent<Image>().color = temp;
                }
            }
        }

        updateAttackSelector = false;
    }

}
