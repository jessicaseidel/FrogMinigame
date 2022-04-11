using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FrogJump : MonoBehaviour {

	public int leafnumber;
	public float speed;
	public bool loadnewlvl = false;
	public bool playanim = false;
	public List<GameObject> respawns;
    public Sounds sound;
	public LevelManager lvlManager;

	private bool spawn = false;
	private Vector3 destination, startpoint, endpoint;
	private Animator FrogAnim, SmallFrogAnim1, SmallFrogAnim2;
	private GameObject objectSitOn;

	void Start () {
		leafnumber = 0;
		speed = 0.15f;
		FrogAnim = GetComponent<Animator>();
		startpoint = new Vector3(3.076f,4.9f,-11.53f);
		destination = startpoint;
		transform.position = startpoint;
		endpoint = new Vector3(0.00f,0.00f,-0.02f);

		// delay for small frogs jumping independent
		SmallFrogAnim1 = GameObject.Find("FrogSmall").GetComponent<Animator>();
		SmallFrogAnim2 = GameObject.Find("FrogSmall2").GetComponent<Animator>();
		SmallFrogAnim1.Play(0,-1, 0.3f);

		respawns = new List<GameObject>();
		foreach(GameObject rottenleaf in GameObject.FindGameObjectsWithTag("Rotten")) {
            respawns.Add(rottenleaf);
        }
	}
	
	void Update () {
		// animate frog moving towards next destination
		if (playanim) {
			if (transform.position != destination) {
        		speed = FrogAnim.GetCurrentAnimatorStateInfo(0).length;
				float delta = speed * Time.deltaTime;
         		Vector3 currentPosition = transform.position;
         		Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);
         		// Move the object to the next position
         		transform.position = nextPosition;
			}
			if (transform.position == destination) {
				if (!FrogAnim.GetCurrentAnimatorStateInfo(0).IsTag("jump") && !FrogAnim.GetCurrentAnimatorStateInfo(0).IsTag("wrongjump")) {
					playanim = false; // destination reached and animation finished: allow new jump
					leafnumber += 1;
					if(sound != null) {
						sound.SoundOnLeaf();
					}
				}
			}
		}
		if (objectSitOn != null) {
			// if rotten leaf is disappeared, jump in water
			if (objectSitOn.activeInHierarchy == false) {
				wrongJump();
			}
			// move frog with moving leaf 
			else if (transform.position == destination) {
				if (objectSitOn.tag == "Moving") {
					Vector3 vec = new Vector3(-0.007f,0,0.0f);
					transform.position = objectSitOn.transform.position + vec;
					destination = transform.position;
				}
			}
		}
		// Small frogs jumping when big frog at end
		if (transform.position == endpoint)  {
			SmallFrogAnim1.SetTrigger("JumpTriggerSmall");
	 		SmallFrogAnim2.SetTrigger("JumpTriggerSmall");
		}
		if (transform.position != endpoint)  {
			SmallFrogAnim1.ResetTrigger("JumpTriggerSmall");
	 		SmallFrogAnim2.ResetTrigger("JumpTriggerSmall");
		}
	}

	IEnumerator waitforspawn() {
		yield return new WaitUntil(() => (spawn == true ));
		yield return new WaitForSeconds(1);
	 	backToStart();
	}

	IEnumerator waitfornextlvl() {
		yield return new WaitUntil(() => loadnewlvl == true );
		loadnewlvl = false;
		yield return new WaitForSeconds(3);
		//Array.Clear(respawns, 0, respawns.Length);
		lvlManager.NextLevel();
		
		respawns.Clear();         
		backToStart();
		respawns.Clear(); 

		// update respawn objects
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("Rotten")) {
             respawns.Add(fooObj);
         }
	}

	// frog jump to next destination
	public void jump(Vector3 vector, GameObject objectSittingOn) {
		objectSitOn = objectSittingOn;
	 	FrogAnim.SetTrigger("jumpTrigger"); // play jump animation
	 	// wait with jumping??
		System.Threading.Thread.Sleep(120);
	 	playanim = true;
	 	// add small distance for frog sitting in mid on leaf
	 	Vector3 vec = new Vector3(-0.007f,0,0.0f);
	 	destination = vector + vec;
	}

	// frog jump to end
	public void endJump(Vector3 vector) {
		objectSitOn = null;
	 	FrogAnim.SetTrigger("jumpTrigger");
	 	// wait with jumping?
		System.Threading.Thread.Sleep(120);
	 	playanim = true;
	 	Vector3 vec = new Vector3(-0.01f,0,0.0f);
	 	endpoint = vector + vec;
	 	destination = endpoint;
	 	loadnewlvl = true;

	 	checkTutorial();
	 	if(sound != null) {
			sound.SoundFrog();
		}

	  	StartCoroutine(waitfornextlvl());
	 	waitfornextlvl();
	}

	// if tutorial is running dont increase level
	private void checkTutorial(){
		if (lvlManager.tutrunning == true) {
			lvlManager.tutrunning = false;
			lvlManager.userlevel--;
		}
	}

	// frog jump in water
	public void wrongJump() {
		objectSitOn = null;
	 	FrogAnim.SetTrigger("wrongJumpTrigger");  
	 	playanim = true;
	 	spawn = true;
	 	if(sound != null) {
			sound.SoundOnWater();
		}
	  	StartCoroutine(waitforspawn());
	}

	// frog spawning at startpoint after wrong click
	public void backToStart() {
		objectSitOn = null;
		transform.position = startpoint;
	 	destination = startpoint;
		leafnumber = 0; 	// set actual leaf number back to 0
	 	spawn = false;
	 	respawnLeaves();
	}

	// respawn all rotten Leafes
	public void respawnLeaves() {
		if (respawns != null) {
			foreach (GameObject respawn in respawns) {
				if (respawn != null) {
            		respawn.GetComponent<LeavesDisappear>().Spawn();
				}
        	}
		}
	}
}
