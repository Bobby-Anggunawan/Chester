using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterHandler : MonoBehaviour
{

    public bool isTeamBlue;
    public GameObject mesh;

    //=========================================================
    Vector3 _moveTarget = new Vector3();
    //=========================================================
    bool _isMoving = false;
    public void setMoving(Vector3 target) {
        _isMoving = true;
        _moveTarget = target;
        transform.GetComponent<Animator>().SetBool("walk", true);
    }
    //=========================================================
    bool _isAttacking = false;
    int _attackIndex = 0;
    public void setAttacking(Vector3 target, int attackIndex) {
        _isAttacking = true;
        _moveTarget = target;
        _attackIndex = attackIndex;
        transform.GetComponent<Animator>().SetBool("walk", true);
    }
    //=========================================================
    bool _atacked = false;
    bool _isDead = false;
    public void atacked(bool isDead) {
        _atacked = true;
        _isDead = isDead;
    }
    //=========================================================

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            var step = 36 * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, step);
            if (transform.position == _moveTarget)
            {
                _isMoving = false;
                transform.GetComponent<Animator>().SetBool("walk", false);
            }
        }
        else if (_isAttacking)
        {
            var step = 36 * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, step);
            if (transform.position == _moveTarget)
            {
                _isAttacking = false;
                transform.GetComponent<Animator>().SetBool("walk", false);
                transform.GetComponent<Animator>().SetTrigger($"attack{_attackIndex + 1}");

                
            }
        }
        else if (_atacked)
        {
            if (_isDead)
            {
                transform.GetComponent<Animator>().SetTrigger("die");
            }
            else
            {
                transform.GetComponent<Animator>().SetTrigger("getHit");
            }
            _atacked = false;
        }
    }
}
