using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturddlefish : Fish
{

    private Rigidbody ourBody;
    private Animator animator;

    public float moveForce = 3f;

    public float rotationLikelyHood = 0.01f;

    public float rotationAmount = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator != null);


        ourBody = gameObject.GetComponent<Rigidbody>();

        animator.SetBool("isSwimming", true);
    }

    // Update is called once per frame
    void Update()
    {
        // move the fish
        ourBody.AddRelativeForce(-gameObject.transform.forward * Time.deltaTime * moveForce);


        // only rotate if not moving
        float rotateR = Random.value;
        if (rotateR <= rotationLikelyHood)
        {
            // Rotate somehow
        }
    }



}
