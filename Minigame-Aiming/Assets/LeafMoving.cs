using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMoving : MonoBehaviour {

	public float speed;
	private Vector3 destination, destination1, destination2;

	// start moving leafes up and down in the river
	void Start () {
		var parentName = transform.parent.name;

		speed = 0.03f;
		Vector3 vec = new Vector3(0,0,0.05f);
		destination1 = transform.position + vec;
		destination2 = transform.position - vec;
		int i = Random.Range(0,2);
		if (parentName == "TutorialLevel(Clone)") {
			i = 0;
		}
		if (i == 0) {
			destination = destination1;
		} else {
			destination = destination2;
		}
	}
	
	// Move leafes up and down 
	void Update () {
		if (transform.position != destination) {
				float delta = speed * Time.deltaTime;
         		Vector3 currentPosition = transform.position;
         		Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);
         		// Move the object to the next position
         		transform.position = nextPosition;
			}
		else {
			if (destination == destination1) {
				destination = destination2;
			}
			else if (destination == destination2) {
				destination = destination1;
			}
		}	
	}
}
