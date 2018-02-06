using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

    public PlayManager.Symbol BeamSymbol;

    public float minSpeed = 1f;
    public float maxSpeed = 5f;
    public float maxAngle = 45f;
    public float rotateSpeed = 3f;

    private float moveSpeed;
    private Rigidbody rb;

    public bool Intersecting
    {
        get
        {
            return intersectingDish && intersectingFocal;
        }

    }

    private bool intersectingDish;
    private bool intersectingFocal;

    private void Start()
    {
        intersectingDish = false;
        intersectingFocal = false;
        rb = GetComponent<Rigidbody>();
        moveSpeed = Random.Range(minSpeed, maxSpeed) * ((Random.Range(0, 2) * 2) - 1);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dish")
        {
            intersectingDish = true;
        }

        if (other.gameObject.tag == "Focal")
        {
            intersectingFocal = true;
        }

        if (other.gameObject.tag == "Wall")
        {
            moveSpeed = -moveSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Dish")
        {
            intersectingDish = false;
        }

        if (other.gameObject.tag == "Focal")
        {
            intersectingFocal = false;
        }
    }


    private void FixedUpdate()
    {
        MoveHorizontally();
        Rotate();
    }

    private void MoveHorizontally()
    {
        rb.velocity = new Vector3(moveSpeed, 0, 0);
    }

    private void Rotate()
    {
        Vector3 currentRotation = rb.rotation.eulerAngles;
        float zRotation = currentRotation.z + rotateSpeed * Time.deltaTime;
        if (zRotation > maxAngle && zRotation < 360f - maxAngle)
        {
            rotateSpeed = -rotateSpeed;
        }
        rb.MoveRotation(Quaternion.Euler(currentRotation.x, currentRotation.y, zRotation));
    }
}
