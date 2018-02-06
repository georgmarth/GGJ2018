using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Animator animator;

    public float speed = 2f;

    public float rotationSpeed = 30f;

    public float maxRotation = 30f;
    public Transform dish;

    private Rigidbody rb;

    private float inputHorizontal = 0f;

    private float inputVertical = 0f;

    private float rotation = 0f;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
	}

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(inputHorizontal * speed * 100 * Time.deltaTime, 0f, 0f);
        animator.SetFloat("walkspeed", velocity.x);
        rb.AddForce(velocity);

        // rotate

        rotation += inputVertical * rotationSpeed * Time.deltaTime;
        rotation = Mathf.Clamp(rotation, -maxRotation, maxRotation);

        dish.rotation = Quaternion.Euler(new Vector3(dish.rotation.x, dish.rotation.y, -rotation));

        // animator variable
        float lean = Mathf.InverseLerp(maxRotation, (-maxRotation - .01f), rotation);
        animator.SetFloat("lean", lean);
    }

    // Update is called once per frame
    void Update () {
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");
	}
}
