﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using PodTheDog.Common;

using UnityEngine.SceneManagement;

namespace PodTheDog.UEX
{
    public class Diver : MonoBehaviour
    {
        private CharacterController characterController;

        private bool groundedPlayer;
        private Vector3 playerVelocity;
        private float currentSpeed = 0f;
        public float kickStrength = 1.0f;
        private float waterDrag = -0.01f;

        public float flatTime = 2f;

        public Camera photographyCamera;
        public int photoWidth = 600;
        public int photoHeight = 400;

        public RenderTexture photographRenderTexture;

        private Animator animator;

        public PhotoPanel photoPanel;

        public SoundManager soundManager;

        /// <summary>
        /// How long in seconds between kicks when the kick button is held down
        /// </summary>
        public float kickInterval = 0.2f;
        private float lastKickTime;

        public float rotationSpeed = 10f;

        private bool flattening = false;

        List<Fish> allFish;

        private float lastInputTime;
        private float lastInputDelay = 2f;

        public Bubbles bubblesPrefab;

        private float lastBubbleTime;
        public float bubbleInterval = 5f;

        public Transform bubblePoint;
        
        // Start is called before the first frame update
        void Start()
        {
            allFish = new List<Fish>();
            foreach (Fish fish in GameObject.FindObjectsOfType<Fish>())
            {
                allFish.Add(fish);
            }
            Debug.Log("Found fish: " + allFish.Count);

            characterController = gameObject.GetComponent<CharacterController>();
            lastKickTime = Time.time;

            animator = GetComponentInChildren<Animator>();
            Debug.Assert(animator != null);

            photographyCamera.enabled = false;

            photoPanel.HidePanel();
            lastBubbleTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            Swim();

            if (Input.GetKeyDown("e"))
            {
                TakePhotograph();
            }

            if (Input.GetKeyDown("p"))
            {
                photoPanel.TogglePanel();
            }

            if (Time.time > lastBubbleTime + bubbleInterval)
            {
                lastBubbleTime = Time.time;
                EmitBubbles();
            }

            if (Input.GetKeyDown("m"))
            {
                SceneManager.LoadScene("MenuScene");
            }
        }

        private void Swim()
        {
            groundedPlayer = characterController.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                /// TODO: ???
                playerVelocity.y = 0f;
            }

            Vector3 currentDirection = gameObject.transform.forward;
            //            float rotationLeftRight = (Input.GetKey("a") ? 1: 0 * rotationSpeed) - (Input.GetKey("d") ? 1 : 0 * rotationSpeed);
            //            float rotationUpDown = (Input.GetKey("w") ? 1 : 0 * rotationSpeed) - (Input.GetKey("s") ? 1 : 0 * rotationSpeed);

            float rotationLeftRight = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            float rotationUpDown = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;


            if (rotationLeftRight != 0 || rotationUpDown != 0)
            {
                lastInputTime = Time.time;
                //                Vector3 rotation = new Vector3(rotationLeftRight * Time.deltaTime, rotationUpDown * Time.deltaTime, 0);
                //                transform.Rotate(rotation);
//                transform.Rotate(new Vector3(currentDirection.x + rotationLeftRight, currentDirection.y + rotationUpDown, 0), Space.World);

                // Force flat - HACK
                Vector3 currentRotation = transform.rotation.eulerAngles;
                Quaternion floatQ = new Quaternion();
                floatQ.eulerAngles = new Vector3(currentRotation.x + rotationUpDown, currentRotation.y + rotationLeftRight, 0);
                transform.rotation = floatQ;

            }

            // Changes the height position of the player..
            float currentTime = Time.time;
            if (Input.GetButton("Jump") && !groundedPlayer)
            {
                if (currentTime >= lastKickTime + kickInterval)
                {
                    currentSpeed += kickStrength;
                    lastKickTime = currentTime;
                    lastInputTime = currentTime;
                }
            }

            currentSpeed += (currentSpeed * waterDrag);

            if (currentSpeed > 0.1f)
            {
                characterController.Move(gameObject.transform.forward * Time.deltaTime * currentSpeed);
                animator.SetBool("isSwimming", true);
            } 


        }

        private void TakePhotograph()
        {
            soundManager.PlayCamera();
            Texture2D picture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            Camera mainCamera = Camera.main;
            RenderTexture tempRenderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            mainCamera.enabled = false;
            photographyCamera.enabled = true;
            photographyCamera.targetTexture = tempRenderTexture;
            photographyCamera.Render();
            RenderTexture.active = tempRenderTexture;
            Rect photoRect = new Rect(0, 0, Screen.width, Screen.height);
            picture.ReadPixels(photoRect, 0, 0, false);
            picture.Apply();

            mainCamera.enabled = true;
            photographyCamera.enabled = false; 

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(tempRenderTexture);

            ScoringPicture scoringPicture = new ScoringPicture();
            scoringPicture.ScorePhoto(picture, photographyCamera, allFish);


            photoPanel.AddPhoto(scoringPicture);

            photoPanel.ShowPanel();

        }

        private void EmitBubbles()
        {

            Bubbles bubbles = Instantiate(bubblesPrefab, bubblePoint);
            // soundManager.PlayBubbles();
        }
    }
}
