using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturddlefish : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator != null);

        animator.SetBool("isSwimming", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
