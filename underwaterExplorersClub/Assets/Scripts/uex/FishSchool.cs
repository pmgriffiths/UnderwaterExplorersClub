
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PodTheDog.UEX
{
    public class FishSchool : Fish
    {
        private List<Renderer> ourRenderers;

        private List<MaterialPropertyBlock> propertyBlocks;

        private Rigidbody ourBody;

        public float accelerationTime = 2f;

        public float rotationTime = 1f;

        public Transform rotationPoint;

        public Vector3 initialVelocity;

        public bool rotateSchool = false;

        public bool faceBackwards = false;

        /// <summary>
        /// Whether the projectile has a particle system.
        /// </summary>
        public bool hasParticles = false;
        private ParticleSystem ps;

        /// <summary>
        /// Whether the particle passes through the centre of hte light
        /// </summary>
        public bool passThroughCentre = false;

        private bool colorChanged = false;

        // how far ahead  to rotate
        public float forwardFactor = 1f;

        // list of triggers we're already reacting to
        private List<Collider> currentCollisions;

        public void Awake()
        {
            ourBody = gameObject.GetComponent<Rigidbody>();
            Debug.Assert(ourBody != null);

            currentCollisions = new List<Collider>();

            // Set the emission color based on our materials color
            ourRenderers = new List<Renderer>();
            propertyBlocks = new List<MaterialPropertyBlock>();
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                ourRenderers.Add(renderer);
                propertyBlocks.Add(new MaterialPropertyBlock());
            }

            if (hasParticles)
            {
                ps = gameObject.GetComponentInChildren<ParticleSystem>();
                Debug.Assert(ps != null);
                var main = ps.main;
                main.startColor = Color.red;
            }

            ourBody.velocity = initialVelocity;
            // ourBody.transform.rotation = Quaternion.Euler(initialVelocity.normalized);

            if (!faceBackwards)
                ourBody.transform.forward = initialVelocity;
            else
            {
                ourBody.transform.forward = -initialVelocity;
            }

        }

        public void Update()
        {
            if (colorChanged)
            {
                UpdateColor(Color.blue);
            }
        }

        /// <summary>
        /// Sets the color of all child renderers
        /// </summary>
        /// <param name="color"></param>
        private void UpdateColor(Color color)
        {
            /// TODO : find out why this is 0 only, not all blocks
            int r = 0;
            foreach (Renderer renderer in ourRenderers)
            {
                MaterialPropertyBlock propertyBlock = propertyBlocks[r];
                renderer.GetPropertyBlock(propertyBlock);

                propertyBlock.SetColor("_Color", color);
                renderer.SetPropertyBlock(propertyBlock);
                propertyBlocks[r] = propertyBlock;
            }
        }


        // 
        void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger && !currentCollisions.Contains(other))
            {
                if (other.gameObject.layer == LayerConstants.SEA_WALLS)
                {
                    StopAllCoroutines(); /// ????

                    currentCollisions.Add(other);

                    // Deflect ourselves based on the angle between us and the wall
                    Vector3 outDirection = Vector3.Reflect(ourBody.velocity, other.transform.forward);
                    Vector3 outVelocity = outDirection.normalized * ourBody.velocity.magnitude;

                    // Deflection angle
                    float angleDegrees = Vector3.Angle(ourBody.velocity, outVelocity);

                    // StartCoroutine(RotateAroundRotationPoint(rotationPoint.position, deflectionDegrees, deflectionTime, other));
                    // StartCoroutine(LerpDirection(ourBody.position, outVelocity, Mathf.Deg2Rad * angleDegrees, other));
                    StartCoroutine(StopRotateAndMove(outDirection, outVelocity, Mathf.Deg2Rad * angleDegrees, other));

                }
            } else
            {
                Debug.Log("Hit collider again: " + other);
            }
        }


        /// <summary>
        /// Lerp the projectile through the centre position towards the end velocity.
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endVelocity"></param>
        /// <param name="angle">Radians to rotate/param>
        /// <returns></returns>
        private IEnumerator StopRotateAndMove(Vector3 endDirection, Vector3 endVelocity, float angle, Collider other)
        {
            // stop particles
            if (hasParticles)
            {
                ps.Stop(false);
            }

            Debug.Log("RotateAndMove endVel: " + endVelocity + " angle: " + angle + " other: " + other);

            float lerpPos = 0;
            Vector3 startDirection = transform.forward;
            ourBody.velocity = Vector3.zero;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / rotationTime;

                Vector3 nextRotation = Vector3.Lerp(startDirection, endDirection, lerpPos);
                transform.forward = nextRotation;
                yield return null;
            }

            lerpPos = 0;
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / accelerationTime;

                ourBody.velocity = Vector3.Lerp(Vector3.zero, endVelocity, lerpPos);
                yield return null;
            }
            // ourBody.transform.rotation = Quaternion.Euler(endVelocity.normalized);
            ourBody.velocity = endVelocity;

            if (currentCollisions.Contains(other))
            {
                currentCollisions.Remove(other);
            }

            // re-enable particles;
            // ps.Play(false);
        }

    }

}
