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
        private float playerSpeed = 2f;
        public float jumpHeight = 1.0f;
        private float gravityValue = -9.8f;

        // Start is called before the first frame update
        void Start()
        {
            characterController = gameObject.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
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
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            characterController.Move(playerVelocity * Time.deltaTime);
        }
    }
}
