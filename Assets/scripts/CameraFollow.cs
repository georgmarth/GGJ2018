using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject player;

    public float smoothTime = 2f;
    Vector3 currentVelocity;

	// Use this for initialization
	void Start () {
        currentVelocity = new Vector3();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x,
            transform.position.y,
            transform.position.z), ref currentVelocity, smoothTime);
	}
}
