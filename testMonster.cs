using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMonster : MonoBehaviour
{

    Animator animationController;

    MyInputSystem control;

    private void Awake()
    {
        control = new MyInputSystem();
        animationController = transform.GetComponent<Animator>();

        control.Test.satu.performed += context => animationController.SetTrigger("attack1");        //1
        control.Test.dua.performed += context => animationController.SetTrigger("attack2");         //2
        control.Test.tiga.performed += context => animationController.SetTrigger("attack3");         //5

        control.Test.getHit.performed += context => animationController.SetTrigger("getHit");       //3
        control.Test.die.performed += context => animationController.SetTrigger("die");             //4
        control.Test.walk.performed += context => animationController.SetBool("walk", true);        //up
        control.Test.stop.performed += context => animationController.SetBool("walk", false);       //down
    }

    private void OnEnable()
    {
        control.Test.Enable();
    }

    //used for MyInputSystem
    private void OnDisable()
    {
        control.Test.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
