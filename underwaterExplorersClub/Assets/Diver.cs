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
        private float playerSpeed = 0f;
        public float kickStrength = 1.0f;
        private float waterDrag = -0.01f;
        
        // Start is called before the first frame update
        void Start()
        {
            characterController = gameObject.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            Swim();
        }

        void NotUpdate()
        { 
            groundedPlayer = characterController.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(kickStrength * -3.0f * waterDrag);
            }

            // playerVelocity.y += gravityValue * Time.deltaTime;
            characterController.Move(playerVelocity * Time.deltaTime);
        }

        private void Swim()
        {
            groundedPlayer = characterController.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                /// TODO: ???
                playerVelocity.y = 0f;
            }


            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            // Vector3 movePosition = gameObject.transform.forward + moveDirection;

            characterController.Move(moveDirection * Time.deltaTime * playerSpeed);

            if (moveDirection != Vector3.zero)
            {
                gameObject.transform.forward -= moveDirection * Time.deltaTime;
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && !groundedPlayer)
            {
                playerSpeed += kickStrength;
            }
            playerSpeed += (playerSpeed * waterDrag);
            if (playerSpeed <= 0)
            {
                playerSpeed = 0f;
            }

            characterController.Move(gameObject.transform.forward * Time.deltaTime * playerSpeed);

        }
    }
}
