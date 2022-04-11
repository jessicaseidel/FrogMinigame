using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ATTENTION TOUCHHANDLER is Tag Water
public class TouchLeaves : MonoBehaviour {

	public FrogJump frogjump;

	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (frogjump) {
			//______________________________________________________________________________
			// !!clicking change into Handtouching with Dexmo!!
            //
			if (Input.GetMouseButtonDown(0)) {

				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out hit, 100.0f)) {
					if(hit.transform != null) {
						var ob = hit.transform.gameObject;
						// Debug.Log(hit.transform.gameObject.name);

            //
			// !!clicking change into Handtouching with Dexmo!!
			//______________________________________________________________________________

						// ATTENTION: Leafes have to be named with number to jump on!
						// EndHitbox has to be named with number after last leaf!
						string[] array = ob.name.Split(' ');
						// get position of touched object
						Vector3 vec = ob.transform.position; 

						// dont handle jump if animation is still running
						if (frogjump.playanim == false) {
							// if water is touched frog spawning at start	
							if (ob.tag == "Water") {
								frogjump.wrongJump();
							}
							else if (ob.tag == "End" && int.Parse(array[1]) == frogjump.leafnumber + 1) {
								frogjump.endJump(vec);
							}
							else {
								// jump to next leaf if right one is touched
								if (int.Parse(array[1]) == frogjump.leafnumber + 1 ) {
									frogjump.jump(vec, ob);	
									// start counter for rotten leafes to disappear
									if (ob.tag == "Rotten") {
										ob.GetComponent<LeavesDisappear>().StartDisappear();
	 								}
	 							}
	 							// if wrong leaf is touched, jump in water and spawn at start
	 							else {
									frogjump.wrongJump();
	 							}
							}
						}
					}
				} 
			}
		}	
	}
}
 