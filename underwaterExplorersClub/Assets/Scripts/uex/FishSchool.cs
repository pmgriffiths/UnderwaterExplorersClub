
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PodTheDog.UEX
{
    public class FishSchool : MonoBehaviour
    {
        private List<Renderer> ourRenderers;

        private List<MaterialPropertyBlock> propertyBlocks;

        private Rigidbody ourBody;

        public float deflectionDegrees = 120f;

        public float deflectionTime = 5f;

        public Transform rotationPoint;

        public float initialSpeed = 2f;


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

        public void Awake()
        {
            ourBody = gameObject.GetComponent<Rigidbody>();
            Debug.Assert(ourBody != null);

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

            ourBody.velocity = transform.forward;

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
            if (other.isTrigger)
            {
                if (other.gameObject.layer == LayerConstants.SEA_WALLS)
                {
                    // Deflect ourselves based on the angle between us and the wall

                    StartCoroutine(RotateAroundRotationPoint(rotationPoint.position, deflectionDegrees, deflectionTime));

                }
            }
        }


        /// <summary>
        /// Lerp the projectile through the centre position towards the end velocity.
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endVelocity"></param>
        /// <param name="angle">Radians to rotate/param>
        /// <returns></returns>
        private IEnumerator RotateAroundRotationPoint(Vector3 point, float degrees, float rotationDuration)
        {

            float lerpPos = 0;
            // Start moving to the centre, then increase velocity to the target velocity.
            while (lerpPos < 1)
            {
                lerpPos += Time.deltaTime / rotationDuration;
                transform.RotateAround(point, Vector3.up, lerpPos * degrees);
                yield return null;
            }

            // re-enable particles;
            // ps.Play(false);
        }
    }

}
