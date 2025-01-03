using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;


    [Header("Controller")]
    [SerializeField] StarterAssetsInputs Inputs;
    public enum Kondisi
    {
        Idle,
        Lari,
        Menyerang,
        Jalan,
        Bertahan,
        Loncat
    }
    public Kondisi kondisi;
   // public bool Lari, Loncat, Menyerang,Jalan;
    private ThirdPersonController thirdPersonController;
    void Start()
    {
        thirdPersonController = Inputs.transform.GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Inputs.jump);
        Debug.Log(kondisi);
        SetAnimasi();   
    }

    float timeattack;
    int attackcurrent;
    void SetAnimasi()
    {
        if(anim != null)
        {
            timeattack += Time.deltaTime;
     //S       if (thirdPersonController.Grounded) anim.SetBool("IsGround", );
            if(Inputs.Shield && thirdPersonController.Grounded) {
                anim.SetBool("Shield", true);
                kondisi = Kondisi.Bertahan;
            }
            else
            {
                kondisi = Kondisi.Idle;
                anim.SetBool("Shield", false);
            }
            if (thirdPersonController.Grounded && Inputs.Attack & timeattack > 0.5f)
            {
                //kondisi = Kondisi.Bertahan;

                SetKondisi(Kondisi.Menyerang);
                Attack();
            } 
            anim.SetFloat("Speed", thirdPersonController._speed);

            if (thirdPersonController.Grounded && Inputs.jump)
            {
                SetKondisi(Kondisi.Loncat);
                
                anim.SetBool("IsGround", true);
              // anim.SetTrigger("Jump");
            }
        }
       // SetKondisi();
    }


    

    private void SetKondisi(Kondisi input = Kondisi.Idle) { 
    
        kondisi = input;
    }
    private void Attack()
    {
        attackcurrent++;
        if(attackcurrent > 4) attackcurrent = 1;
        if (timeattack > 1.0f) attackcurrent = 1;
        anim.SetTrigger("Attack"+attackcurrent);
        timeattack = 0.0f;  
    }
    public void SetFalseAnim()
    {
        SetKondisi(Kondisi.Idle);

        anim.SetBool("IsGround",false);
    }

    public void SetFalseAttack()
    {
        kondisi = Kondisi.Idle;
    }
}
