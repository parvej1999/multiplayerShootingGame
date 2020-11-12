using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;
public class Player : NetworkBehaviour
{
    // Update is called once per frame
    public float Movespeed = 3.5f;
    public float Turnspeed = 120f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    
    // for sound
    private bool isWalking=false;
    public int stepNum;
    public bool isStepping = false;
    public AudioSource fooStep0;
    public AudioSource fooStep1;
    public AudioSource fooStep2;
    public AudioSource fooStep3;
    public AudioSource fooStep4;
    void Update()
    {
    	if (!isLocalPlayer)
        {
            return;
        }
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
        if(Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
        	this.GetComponent<Animation>().Play("Walk");
        	isWalking = true;
        	this.transform.Translate(Vector3.forward * vert * Movespeed * Time.deltaTime);
        	this.transform.localRotation *= Quaternion.AngleAxis(horz * Turnspeed * Time.deltaTime, Vector3.up);
        	
        	// sound
        	isStepping = true;
        	if(isStepping){
        		StartCoroutine(stepSound());
        	}
        }
        else{
        	this.GetComponent<Animation>().Play("Idle");
        }
        if(Input.GetKeyDown(KeyCode.Space))
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
    	if(isWalking && !isStepping){
    		isStepping = true;
    		stepNum = Random.Range(0, 5);
    		if(stepNum == 0){
    			fooStep0.Play();
    		}
    		else if(stepNum == 1){
    			fooStep1.Play();
    		}
    		else if(stepNum == 2){
    			fooStep1.Play();
    		}
    		else if(stepNum == 3){
    			fooStep3.Play();
    		}
    		else if(stepNum == 4){
    			fooStep4.Play();
    		}
    		
    	}
        yield return new WaitForSeconds(0.3f);
    	isStepping = false;
    }
}
