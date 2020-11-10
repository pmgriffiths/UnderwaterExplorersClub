using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public Camera photographyCamera;
        public int photoWidth = 600;
        public int photoHeight = 400;

        public RenderTexture photographRenderTexture;

        public PhotoPanel photoPanel;

        /// <summary>
        /// How long in seconds between kicks when the kick button is held down
        /// </summary>
        public float kickInterval = 0.2f;
        private float lastKickTime;

        public float rotationSpeed = 10f;
        
        // Start is called before the first frame update
        void Start()
        {
            characterController = gameObject.GetComponent<CharacterController>();
            lastKickTime = Time.time;
            photoPanel.HidePanel();
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
        }

        private void Swim()
        {
            groundedPlayer = characterController.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                /// TODO: ???
                playerVelocity.y = 0f;
            }

            /*
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            // Vector3 movePosition = gameObject.transform.forward + moveDirection;
            // characterController.Move(moveDirection * Time.deltaTime * currentSpeed);

            if (moveDirection != Vector3.zero)
            {
                gameObject.transform.forward -= moveDirection * Time.deltaTime;
            } */

            Vector3 currentDirection = gameObject.transform.forward;
            float rotationLeftRight = Input.GetAxis("Vertical") * rotationSpeed ;
            float rotationUpDown = Input.GetAxis("Horizontal") * rotationSpeed;

            Vector3 rotation = new Vector3(rotationLeftRight * Time.deltaTime, rotationUpDown * Time.deltaTime, 0);
            transform.Rotate(rotation);

            // Changes the height position of the player..
            float currentTime = Time.time;
            if (Input.GetButton("Jump") && !groundedPlayer)
            {
                if (currentTime >= lastKickTime + kickInterval)
                {
                    currentSpeed += kickStrength;
                    lastKickTime = currentTime;
                }
            }
            currentSpeed += (currentSpeed * waterDrag);
            if (currentSpeed <= 0)
            {
                currentSpeed = 0f;
            }
            
            characterController.Move(gameObject.transform.forward * Time.deltaTime * currentSpeed);
        }

        private void TakePhotograph()
        {
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

            photoPanel.AddPhoto(picture);

        }
    }
}
