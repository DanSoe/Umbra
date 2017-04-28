﻿using UnityEngine;
using System.Collections;

public class PlayerController: MonoBehaviour 
{

    Animator playerAnim;

	//basic movement
	public float JumpHeight;
	public float MoveSpeed;
    public float RunSpeed;
    public float WalkSpeed;
	public float maxVelocity = 120f;
	public Rigidbody player;
    public GameObject raySpawn;

	//is the player dashing?
	public bool dash;
	//how long can dash last=
	public float timer;
	//does dash boost the player's speed?
	public float DashSpeed;

    //Stats
    public int curHealth;
    public int maxHealth = 5;


	public LayerMask whatIsGround;
	public bool grounded;
	public float distanceRay;



	// Use this for initialization
	void Start()
	    {
            playerAnim = GetComponent<Animator>();
			dash = false;
			player = GetComponent<Rigidbody>();
			timer = 0;

            curHealth = maxHealth;

			//script = GameObject.Find ("DoubleJumpCheckPoint").GetComponent<GetDoublejump>();

		    //nextFire = Time.time;
		}
		
	void FixedUpdate()
		{
            //Debug.Log(player.velocity);

			RaycastHit rayOut;
			//grounded = Physics.SphereCast(player.transform.position, -transform.up, out rayOut, distanceRay, whatIsGround );
            grounded = Physics.SphereCast(raySpawn.transform.position, 0.5f , -transform.up, out rayOut, distanceRay, whatIsGround);

        //Kontrollerer max farten spilleren kan ha for å hindre tullete sterke dash boosts
            if (player.velocity.magnitude > maxVelocity)
            {
                player.velocity = Vector3.ClampMagnitude(player.velocity, maxVelocity);
                
            }
	    }

	// Update is called once per frame
	void Update()
	    {
            float realDashSpeed = DashSpeed;

            if (Input.GetAxisRaw("Sprint") > 0.5f) //(KeyCode.LeftShift))
            {
                MoveSpeed = RunSpeed;
                realDashSpeed = 0.75f;

            }
            else if (Input.GetAxisRaw("Sprint") < 0.5f) //(Input.GetKeyUp(KeyCode.LeftShift))
            {
                MoveSpeed = WalkSpeed;
                realDashSpeed = 1.5f;
            }
            
		//player.AddForce (Vector3.down * 100f); //(Implimenter etter vi har en grounded check)
		if (Input.GetAxisRaw("Horizontal") < -0.001f && dash == false)
		{
			//transform.Translate((-transform.forward) * MoveSpeed * Time.deltaTime, Space.World);
            playerAnim.SetFloat("Speed", MoveSpeed);
            if (transform.rotation != Quaternion.Euler(0, -90, 0))
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            player.AddForce(transform.forward * MoveSpeed);
			//SpeedLimiter ();

		}
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.001f)//(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            playerAnim.SetFloat("Speed", 0f);
        }


        if (Input.GetAxisRaw("Horizontal") > 0.001f  && dash == false) 
		{
			//transform.Translate ((transform.forward) * MoveSpeed * Time.deltaTime, Space.World);
            playerAnim.SetFloat("Speed", MoveSpeed);
            if (transform.rotation != Quaternion.Euler(0, 90, 0))
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            player.AddForce(transform.forward * MoveSpeed);
 

		}

		if (Input.GetButtonDown("Dash Left") /*(Input.GetKeyDown (KeyCode.Q)*/ && dash == false) 
		{
			timer = 40;
			//MoveSpeed = MoveSpeed + DashSpeed;
            if (transform.rotation == Quaternion.Euler(0, 90, 0))
            {
                player.AddForce(-transform.forward * (MoveSpeed * realDashSpeed), ForceMode.VelocityChange);
                playerAnim.SetBool("DashBackward", true);
                //print(realDashSpeed);
                
            }
            else
            {
                player.AddForce(transform.forward * (MoveSpeed * realDashSpeed), ForceMode.VelocityChange);
                //print(realDashSpeed);
                playerAnim.SetBool("DashForward", true);
            }

			//insert animation code

		}
        if (Input.GetButtonDown("Dash Right") /*(Input.GetKeyDown (KeyCode.E)*/ && dash == false) 
		{
			timer = 40  ;
			//MoveSpeed = MoveSpeed + DashSpeed;
            if (transform.rotation == Quaternion.Euler(0, -90, 0))
            {
                player.AddForce(-transform.forward * (MoveSpeed + realDashSpeed), ForceMode.VelocityChange);
                playerAnim.SetBool("DashBackward", true);
            }
            else
            {
                player.AddForce(transform.forward * (MoveSpeed + realDashSpeed), ForceMode.VelocityChange);
                playerAnim.SetBool("DashForward", true);
            }

			//insert animation code
            

		}
		//counts down the duration of the dash
		if (timer > 0)
		{
			dash = true;
			--timer;

		}
		//ends the dash once the timer reaches 0
		if (timer <= 0 && dash)
		{
			dash = false;
			//MoveSpeed = MoveSpeed - DashSpeed;
			//insert end animation code
		
		}


		if (Input.GetButtonDown("Jump")/*(Input.GetKeyDown(KeyCode.Space)*/ && grounded)
		{
            playerAnim.SetBool("Jump",true);


			player.velocity = new Vector3 (GetComponent<Rigidbody> ().velocity.x, JumpHeight);


		}

		if (player != grounded)
		{
			player.AddForce (Vector3.down * 80f); //(Implimenter etter vi har en grounded check)
		}
		/*
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			player.velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, JumpHeight);

        }
*/
/*
        if(player.velocity.y == 0)
        {
            playerAnim.SetBool("Landing", true);
        }
        */
        if(grounded == true)
        {
            playerAnim.SetBool("Landing", true);
        }   
       


		if (Input.GetKey("escape"))
		 {
			Application.Quit();
		 }

		//	Debug.Log ("velocity " + player.velocity.sqrMagnitude);
        
        if(curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
        if(curHealth <= 0)
        {
            Die();
        }
	}
    void Die()
    {
        //restart
        Application.LoadLevel(0);
    }
    void LateUpdate()
    {
        playerAnim.SetBool("Jump", false);
        playerAnim.SetBool("DashForward", false);
        playerAnim.SetBool("DashBackward", false);
        playerAnim.SetBool("Landing", false);

    }


    void OnTriggerEnter(Collider PlayerColli)
    {
        if (PlayerColli.gameObject.tag == "TeleportCube")
        {
            transform.position = new Vector3(438f, 91.37f, 10f);
        }
        if (PlayerColli.gameObject.tag == "TeleportCage")
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 16f);
        }
        if (PlayerColli.gameObject.tag == "TeleportElevator")
        {
            transform.position = new Vector3(314f, 258f, 5f);
        }
        if (PlayerColli.gameObject.tag == "TeleportElevator1")
        {
            transform.position = new Vector3(790f, 351f, 5f);
        }
        if (PlayerColli.gameObject.tag == "GameDone")
        {
            Application.LoadLevel(0);
        }
    }
}