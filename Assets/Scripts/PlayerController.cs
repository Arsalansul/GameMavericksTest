using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController
    {
        protected Joystick joystickLeft;
        protected Joystick joystickRight;

        private Animator animator;

        private CharacterController controller;

        private float speed = 6;  //multipicator for input values
        private float rotationSpeed = 1;  //multipicator for input values

        private Quaternion startRotationOnInput;

        private GameObject playerGameObject;

        public Vector3 position => playerGameObject.transform.position;
        public Vector2 positionV2 => new Vector2(playerGameObject.transform.position.x, playerGameObject.transform.position.z);

        private const int winAnimCount = 4;

        public void Rotate()
        {
            if (!joystickRight.pressed)
            {
                startRotationOnInput = playerGameObject.transform.rotation;
                return;
            }

            var joystickVector = new Vector3(joystickRight.Horizontal, 0, joystickRight.Vertical);
            playerGameObject.transform.rotation = Quaternion.Slerp(playerGameObject.transform.rotation, Quaternion.LookRotation(startRotationOnInput * joystickVector), Time.deltaTime * rotationSpeed);
        }

        public void Move()
        {
            var moveVector = playerGameObject.transform.TransformDirection(Vector3.forward) * (joystickLeft.Vertical + Input.GetAxis("Vertical")) +
                             playerGameObject.transform.TransformDirection(Vector3.right) * (joystickLeft.Horizontal + Input.GetAxis("Horizontal"));

            controller.SimpleMove(moveVector * speed);
        }

        public void SpawnPlayer()
        {
            var spawnPoint = GameObject.Find("spawnPoint").transform;
            playerGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), spawnPoint.position, spawnPoint.rotation);

            joystickLeft = GameObject.Find("JoystickLeft").GetComponent<Joystick>();
            joystickRight = GameObject.Find("JoystickRight").GetComponent<Joystick>();

            animator = playerGameObject.GetComponent<Animator>();

            controller = playerGameObject.GetComponent<CharacterController>();
        }

        public void SetAnimatorParams()
        {
            animator.SetFloat("speed", controller.velocity.magnitude);
        }

        public void Death()
        {
            animator.SetBool("dead", true);
        }

        public void Win()
        {
            animator.SetInteger("win", Random.Range(0,winAnimCount) + 1);
        }
    }
}
