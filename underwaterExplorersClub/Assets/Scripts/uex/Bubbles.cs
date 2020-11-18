using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{

    private Rigidbody ourBody;

    public float lifeTime = 2f;

    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        ourBody = gameObject.GetComponent<Rigidbody>();
        Debug.Assert(ourBody != null);

        ourBody.velocity = velocity;

        StartCoroutine(KillBubbles(lifeTime));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator KillBubbles(float lifeSpan)
    {
        float birthTime = Time.time;

        yield return new WaitForSeconds(lifeSpan);

        DestroyImmediate(gameObject);
    }
}
