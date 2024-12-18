using UnityEngine;

namespace Script
{
    public class ThirdPersonAnimation : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody rb;
        private ThirdPersonController controller; // Ссылка на контроллер
        private float maxSpeed = 5f;

        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            controller = GetComponent<ThirdPersonController>();
        }

        void Update()
        {
          
            animator.SetFloat("speed", rb.linearVelocity.magnitude / maxSpeed);

            
            if (controller != null && controller.IsGrounded())
            {
                animator.SetBool("jump", false);
            }
            else
            {
                animator.SetBool("isGrounded", false);
                animator.SetBool("jump", true);
            }
        }
    }
}