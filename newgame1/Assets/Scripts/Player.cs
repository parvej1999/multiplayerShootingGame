using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;
public class Player : NetworkBehaviour
{
    // Update is called once per frame
    public float Movespeed;
    public float Turnspeed = 120f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    
    // for sound
    private bool isWalking=false;
    private bool isRunning=false;
    public bool isStepping = false;
    public int stepNum;
    public AudioSource footStep0;
    public AudioSource footStep1;
    public AudioSource footStep2;
    public AudioSource footStep3;
    public AudioSource footStep4;
    public int playerRotationSensitivity = 10;
    public float force = 1.0f;
    public Rigidbody rb;
    public bool isGrounded;    //to detect whther player is on ground or not

    // 12nov
    public GameObject playerCamera;

    void OnCollisionStay() {
        isGrounded = true;
    } 

    void OnCollisionExit() {
        isGrounded = false;
    }

    void makeJump(){
        this.GetComponent<Animation>().Play("Jump");
        rb = GetComponent<Rigidbody>();
        rb.AddForce(0, force, 0, ForceMode.Impulse);
        // isGrounded=true;
    }

    void start(){
        Debug.Log("Hey Started");

        if(isLocalPlayer){
            Debug.Log("I am local player");
            playerCamera.SetActive(true);
        }
        else{
            playerCamera.SetActive(false);
        }
    }

    void Update()
    {
    	// if (!isLocalPlayer)
        // {
        //     return;
        // }
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");

        // to rotate the player with mouse sensitivity
        if(!Input.GetButton("E")){
            if(Input.GetAxis("Mouse X")<0){
                this.transform.Rotate(0, -playerRotationSensitivity, 0);
            }
            if(Input.GetAxis("Mouse X")>0){
                this.transform.Rotate(0, playerRotationSensitivity, 0);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            makeJump();
        }

        if(Input.GetButton("S")){
            this.GetComponent<Animation>().Play("WalkBack");
            Movespeed = 5.0f;
            this.transform.Translate(Vector3.back * Movespeed * Time.deltaTime);
        }

        if(Input.GetButton("W"))
        {
            if(Input.GetButton("LeftShift")){
                this.GetComponent<Animation>().Play("Run");
                Movespeed=30.0f;            // setting the speed of the player while it is running
                isRunning = true;           // Setting true while player is in running state
                isStepping = true;         
            }


            else{
                this.GetComponent<Animation>().Play("Walk");
                Movespeed=20.0f;    // setting the speed of the player while it is running
                isWalking = true;  // Setting true while player is in walking state 
                isStepping = true; 
            }
        	
        	
        	this.transform.Translate(Vector3.forward * Movespeed * Time.deltaTime);      // makes forward movement
        	this.transform.localRotation *= Quaternion.AngleAxis(horz * Turnspeed * Time.deltaTime, Vector3.up); // makes rataion in x axis
        	
        	// sound
        	   // setting True when player is stepping
        	if(isStepping){
                Debug.Log("isStepping ");
                Debug.Log(stepNum);

        		StartCoroutine(stepSound());
        	}
        }

        else {

            if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
                    makeJump();
                }
            else{
                    this.GetComponent<Animation>().Play("Idle");
                }
            
        }

        if(Input.GetMouseButton(1))
        {
            CmdFire();
        }
    }
    [Command]
    public void CmdFire()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

        // Spawn the bullet on the client
        NetworkServer.Spawn(bullet);
        
        Destroy(bullet, 7);
    }
    /*
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }*/
    
    IEnumerator stepSound(){
    	if(isWalking ){
    		
            Debug.Log("isStepping inside Couroutine");
    		stepNum = Random.Range(0, 5);
    		if(stepNum == 0){
    			footStep0.Play();
    		}
    		else if(stepNum == 1){
    			footStep1.Play();
    		}
    		else if(stepNum == 2){
    			footStep1.Play();
    		}
    		else if(stepNum == 3){
    			footStep3.Play();
    		}
    		else if(stepNum == 4){
    			footStep4.Play();
    		}
    		
    	}
        yield return new WaitForSeconds(5f);
        Debug.Log("At the end of the couroutine");
    	isStepping = false;
    }
}
