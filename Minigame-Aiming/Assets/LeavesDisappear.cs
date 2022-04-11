using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesDisappear : MonoBehaviour {

    public bool disappear;

    private int startSize = 1;
    private float minSize = 0.01f;
    private float maxSize = 1f;    
    private float speed = 4.0f;   
    private Vector3 targetScale, baseScale;
    private float currScale;
         
    void Start() {
        baseScale = transform.localScale;
        transform.localScale = baseScale * startSize;
        currScale = startSize;
        targetScale = baseScale * startSize;
        disappear = false;
    }
         
    void Update() {
        transform.localScale = Vector3.Lerp (transform.localScale, targetScale, speed * Time.deltaTime);
        if (gameObject.activeInHierarchy == false && disappear == false) {
        	Spawn();
        }
    }

	// first wait for seconds is waiting time until "animation" begin
    // then leafes get bigger and smaller to announce disappearing
    IEnumerator waitfordestroy() {
		yield return new WaitForSeconds(3);
		ChangeSize(2);
		yield return new WaitForSeconds(0.5f);
		ChangeSize(1);
		yield return new WaitForSeconds(0.5f);
		ChangeSize(2);
		yield return new WaitForSeconds(0.5f);
		ChangeSize(0.01f);
		yield return new WaitForSeconds(0.5f);
		gameObject.SetActive(false);
	}
         
    public void StartDisappear() {
    	disappear = true;
       	StartCoroutine("waitfordestroy");
    }

    // change size of leaf to show user it will disappear soon
    public void ChangeSize(float scale) {
        currScale = scale;    
        currScale = Mathf.Clamp (currScale, minSize, maxSize+1);
        targetScale = baseScale * currScale;
    }

    public void Spawn() {
    	StopCoroutine("waitfordestroy");
        gameObject.SetActive(true);
        transform.localScale = baseScale * startSize;
        currScale = startSize;
        targetScale = baseScale * startSize;
        disappear = false;
    }    
} 
      