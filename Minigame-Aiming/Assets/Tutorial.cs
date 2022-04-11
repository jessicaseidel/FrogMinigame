using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public GameObject PrefabLevel;
	public GameObject Touch;
	public FrogJump frogjump;

	private Vector3 destination, start; // rotdest;
	private GameObject[] Objects = new GameObject[4];
	private GameObject level, parentObject, dexmoHand, finger, touch;
	//private float rotdestx, startrotdestx; // if hand should rotate completely

	private bool rotateup = false;		// rotation of index finger up or downwards
	private bool rot = false;			// allow rotation of index finger
	private bool touched = false;       // is moving leaf touched

    /// 
    /// Tutorial not working, cause dexmo API is deleted
    /// 

    void Start () {
		GameObject filenamefld = null;
        Transform[] trans = null; // DEXMO HAND DELETED // GameObject.Find("DexmoControllerAndHands").GetComponentsInChildren<Transform>(true);
        dexmoHand = null;
        /*foreach (Transform t in trans) {
    		if (t.gameObject.name == "TranslucentRight") {
        		dexmoHand = t.gameObject;
     		}
 		}
 		// search index finger of right hand
 		trans = dexmoHand.GetComponentsInChildren<Transform>(true);
 		foreach (Transform t in trans) {
    		if (t.gameObject.name == "index1") {
        		finger = t.gameObject;
     		}
 		} 
 		start = dexmoHand.transform.position; 
 		destination = dexmoHand.transform.position; */
    }

    void Update () {
		// move hand to destination
		if (dexmoHand != null && dexmoHand.transform.position != destination) {
				float delta = 0.15f * Time.deltaTime;
         		Vector3 currentPosition = dexmoHand.transform.position;
         		Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);
         		dexmoHand.transform.position = nextPosition;
		} 
		// rotate hand
		/*if (dexmoHand.transform.rotation.x != rotdestx) {
			Vector3 vec = new Vector3(1, 0, 0);
			if (dexmoHand.transform.rotation.x < rotdestx) {
				dexmoHand.transform.Rotate(0.5f,0,0);
			}
			else if (dexmoHand.transform.rotation.x > rotdestx) {
				dexmoHand.transform.Rotate(-0.5f,0,0);
			} 
		} */
		// rotate finger
		if (rot == true) {
			Vector3 vec = new Vector3(1, 0, 0);
			if (rotateup == false) {
				finger.transform.Rotate(0,0,1f);
			}
			else if (rotateup == true) {
				finger.transform.Rotate(0,0,-1f);
			}  		
		}
		// if moving leaf is touched, move touch animation with it
		if (touched == true) {
					touch.transform.position = Objects[2].transform.position;
		}
	}

	IEnumerator ShowTouch(Vector3 vec) {
		touch = (GameObject)Instantiate(Touch, vec, Quaternion.identity);
		touch.transform.Rotate (270f, 270f, 0f);
		yield return new WaitForSeconds(1);
		touched = false;
		Destroy(touch);
	}

	// Hand to destination, rotate finger down to touch
	IEnumerator HandtoDest() {		
 		yield return new WaitUntil(() => (dexmoHand.transform.position == destination));	
 		rotateup = false;
 		rot = true;
 		yield return new WaitUntil(() => (finger.transform.rotation.z < -0.2f));
 		rotateup = true;
	}

	// Rotate finger up after touching, move hand back to start
	IEnumerator HandtoStartPos() {	
 		yield return new WaitUntil(() => (finger.transform.rotation.z > -0.06f));
 		rot = false;
 		rotateup = false;

		destination = start;
	}

	IEnumerator RunTutorial() {
		if (dexmoHand != null) {
			dexmoHand.SetActive(true);
		}
		frogjump.backToStart();
		Objects[1].transform.localScale = new Vector3(100, 100, 100);
		Vector3 vec = new Vector3(0,0,0);
		// Hand destinations
		Vector3[] dest  = new [] { new Vector3(3.214f,4.94f,-11.655f), new Vector3(3.315f,4.94f,-11.67f), new Vector3(3.41f,4.94f,-11.65f), new Vector3(3.5f,4.94f,-11.64f) };

		yield return new WaitForSeconds(0.5f);

		//________________________ HAND TO FIRST LEAF
 		destination = dest[0];
 		//rotdestx = 10;  if hand should rotate completely

 		// Hand and finger to destination
 		yield return StartCoroutine(HandtoDest());
 		StartCoroutine(ShowTouch(Objects[0].transform.position));

 		// JUMP TO FIRST LEAF
		GameObject ob = Objects[0];
		vec = ob.transform.position;
		frogjump.jump(vec, ob);	

		// Rotate finger up, move Hand to start 		
		yield return StartCoroutine(HandtoStartPos());
		// rotdestx = startrotdestx; if hand should rotate completely

		//___________________________ HAND TO WATER
		yield return new WaitForSeconds(2);
		destination =  new Vector3(3.27f,4.94f,-11.645f);
 		// Hand and finger to destination
 		yield return StartCoroutine(HandtoDest());
 		StartCoroutine(ShowTouch(new Vector3(3.225f, 4.885f, -11.515f)));
 		
 		// JUMP TO WATER
		frogjump.wrongJump();

		// Rotate finger up, move Hand to start 		
		yield return StartCoroutine(HandtoStartPos());

		//______________________________ HAND TO FIRST LEAF
		yield return new WaitForSeconds(2);
		destination = dest[0];
 		// Hand and finger to destination
 		yield return StartCoroutine(HandtoDest());
 		StartCoroutine(ShowTouch(Objects[0].transform.position));
 		
 		// JUMP TO FIRST LEAF
		ob = Objects[0];
		vec = ob.transform.position;
		frogjump.jump(vec, ob);

		// Rotate finger up, move Hand to start 		
		yield return StartCoroutine(HandtoStartPos());

		//________________________________ HAND TO SECOND LEAF
		yield return new WaitForSeconds(2);
		destination = dest[1];
 		// Hand and finger to destination
 		yield return StartCoroutine(HandtoDest());
 		StartCoroutine(ShowTouch(Objects[1].transform.position));
 		
 		// JUMP TO SECOND LEAF
		ob = Objects[1];
		vec = ob.transform.position;
		frogjump.jump(vec, ob);
		ob.GetComponent<LeavesDisappear>().StartDisappear();

		// Rotate finger up, move Hand to start 		
		yield return StartCoroutine(HandtoStartPos());

		// wait til sink
		yield return new WaitForSeconds(6);
		//frogjump.respawnLeaves();
		Objects[1].transform.localScale = new Vector3(100, 100, 100);

		//_________________________________ JUMP from start to end		
		for (int i = 0; i < 4; i++) {
			if (i != 0){
				yield return new WaitForSeconds(2);	
			}
			destination = dest[i];
			yield return StartCoroutine(HandtoDest());
			StartCoroutine(ShowTouch(Objects[i].transform.position));
 		
 			// JUMP TO LEAF
			ob = Objects[i];
			vec = ob.transform.position;

 			if (i == 3) {
				frogjump.endJump(vec);
 			}
 			else if (i == 2) {
 				touched = true;
				frogjump.jump(vec, ob);
 			}
 			else {
				frogjump.jump(vec, ob);
 			}
			// Rotate finger up, move Hand to start 		
			yield return StartCoroutine(HandtoStartPos());
		}

		yield return new WaitForSeconds(2);
	 	DeleteTutorial();
	}

	// load tutorial level and start tutorial 
	public void LoadLevel() {
		level = (GameObject)Instantiate(PrefabLevel, new Vector3(3.236f, 4.9878f, -11.57f), Quaternion.identity);
	
		foreach (Transform child in level.transform) {
			if(child.name.Contains("1")) {
				Objects[0] = child.gameObject;
			}
			else if(child.name.Contains("2")) {
				Objects[1] = child.gameObject;
				Objects[1].transform.localScale = new Vector3(100, 100, 100);
				frogjump.respawns.Add(Objects[1]);
			}
			else if(child.name.Contains("3")) {
				Objects[2] = child.gameObject;
			}
			else if(child.name.Contains("4")) {
				Objects[3] = child.gameObject;
			}
		}
		StartCoroutine(RunTutorial());
	 	RunTutorial();		
	}

	// Delete old level prefab in scene
	public void DeleteTutorial() {
		if (level != null) {
			Destroy(level);
			dexmoHand.SetActive(false);
		}
	}
}
