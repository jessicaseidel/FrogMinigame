using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public List<GameObject> PrefabObjects;
	public Tutorial tut;
	public bool tutrunning;
	public float userlevel; // variable to increase level difficulty 
	
	private GameObject leaf, endHitbox, parentObject;
	private int leafdifficulty = 1; 
	private	int jumpObjectNumber = 4;
	private	float delta = -0.1f;
	private	float next = 0.1f;
	private float leafScale = 0.0f;


	void Start () {
		parentObject = (GameObject)Instantiate(new GameObject(), new Vector3(3.276f, 4.888f, -11.53f), Quaternion.identity);
		parentObject.name = "Level";
		NextLevel();

		userlevel = 1;
	}
	
	void Update () {

		// Start Tutorial with key T, TODO: maybe change input to start it, so every minigame tutorial has the same start!
        /// 
        /// Tutorial not working, cause dexmo API is deleted
        /// 
		if (Input.GetKeyDown(KeyCode.T) && tutrunning == false) {
           // DeleteOldLevel();
           // tut.LoadLevel();
           // tutrunning = true;
        }
        // Continue game with key C, TODO change input to continue!
        if (Input.GetKeyDown(KeyCode.C)) {
            //	tut.DeleteTutorial();
            // 	tutrunning = false;
        	// userlevel--; // decrease user level, otherwise it will inncrease even if user doesnt succesfully completed
            // NextLevel();
        }
	}

	// Load next level
	public void NextLevel() {
		SelectDiffiulty();
		DeleteOldLevel();

		// create leaves with random rotation and random spawn
		for (int i = 1; i < jumpObjectNumber; i++) {
			float randomSpawn = Random.Range(-0.05f, 0.05f);
			leaf = (GameObject)Instantiate(PrefabObjects[Random.Range(0, leafdifficulty)], new Vector3(3.276f + delta, 4.888f, -11.53f + randomSpawn), Quaternion.identity);
			float randomRotation = Random.Range(0.0f, 360.0f);
			leaf.transform.Rotate (270f, randomRotation, 0f);
			float scale = Random.Range(leafScale - 0.02f, leafScale +0.02f);
			leaf.transform.localScale += new Vector3(scale,scale,0);
			leaf.name = leaf.name.Replace("(Clone)", " " + i);
			leaf.transform.parent = parentObject.transform;

			delta += next;
		}

		// create endhitbox
		endHitbox = (GameObject)Instantiate(PrefabObjects[PrefabObjects.Count -1], new Vector3(3.465f, 4.9f, -11.53f), Quaternion.identity);
		endHitbox.name = endHitbox.name.Replace("(Clone)", " "  + jumpObjectNumber);
		endHitbox.transform.parent = parentObject.transform;  

		// increase userlevel when level successfully completed
		// increase float less (0.1, 0.5 etc) to have multiple level with same difficulty 
		userlevel++;
	}


	// set difficulty: number of leaves, moving or disappearing leaves
	// set delta for distance for next leaf
	private void SelectDiffiulty() {
		if(userlevel == 0) {
			leafScale = 0.1f;
			leafdifficulty = PrefabObjects.Count -3;
			jumpObjectNumber = 4;
			delta = -0.1f;
			next = 0.1f;
		}
		else if(userlevel == 1) {
			leafScale = 0.05f;
			leafdifficulty = PrefabObjects.Count -2;
			jumpObjectNumber = 5;
			delta = -0.12f;
			next = 0.08f;
		}
		else if(userlevel == 2) {
			leafScale = 0.0f;
			leafdifficulty = PrefabObjects.Count -1;
			jumpObjectNumber = 6;
			delta = -0.13f;
			next = 0.065f;
		}
		else if(userlevel >= 3) {
			leafScale = -0.02f;
			jumpObjectNumber = 7;
			delta = -0.138f;
			next = 0.054f;
		}
	}

	// Delete old level prefab  in scene
	private void DeleteOldLevel() {
		foreach (Transform child in parentObject.transform) {
			Destroy(child.gameObject);
		}
	}
}
