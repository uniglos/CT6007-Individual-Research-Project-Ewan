using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Most of this script is adapted from Brackeys' basic character controller script
//https://youtu.be/_QajrabyTJc Brackeys, Oct 2019
public class PlayerScript : Character
{
    //public CharacterController characterController;

    public HealthBarPlayer HealthBar;
    public DDABars DDABars;

    
    public override void Start()
    {
        base.Start(); 
        originalCol = Color.blue;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        HealthBar.SetHealth(health);
        DDABars.SetDDABars(combatAssist, navigationAssist, GameManager.instance.ddaSkew);

    }

    protected override void CharacterMovement()
    {
        
        if (Input.GetButtonDown("Sprint")&& !isCrouching)
        {
            isSprinting = true;
        }
        if (Input.GetButtonDown("Jump") && canJump)
        {
            canJump = false;
            isGrounded = false;
            coyoteTimer = coyoteLimit;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (Input.GetButtonUp("Jump"))
        {
            canJump = false;
            isGrounded = false;
            coyoteTimer = coyoteLimit;
        }
            if (Input.GetButton("Crouch"))
        {
            isCrouching = true;
            this.characterController.height = 1f;
        }
        if(Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
        }
        else if(!isCrouching)
        {
            this.characterController.height = 2.5f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

       
        Vector3 move = transform.right * x + transform.forward * z;
        if ( z <= 0.5||isCrouching)
        {
            isSprinting = false;
        }
        //Debug.Log(move);

        if (isSprinting)
        {
            characterController.Move(move * moveSpeed * 2.5f * Time.deltaTime);
        }
        else
        {
            characterController.Move(move * moveSpeed * Time.deltaTime);
        }




        base.CharacterMovement();
        //characterController.Move(velocity * Time.deltaTime);


    }
    protected override void WeaponUpdate()
    {
        base.WeaponUpdate();
        if (weapons[currentWeapon].isAutomatic)
        {
            if (Input.GetButton("Fire1"))
            {
                //Debug.Log("Fired Weapon");
                weapons[currentWeapon].FireWeapon(gunPoint);
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //Debug.Log("Fired Weapon");
                weapons[currentWeapon].FireWeapon(gunPoint);
            }
        }
    }

    void BulletMagnetism()
    {

    }
}
