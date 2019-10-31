using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpForce = 10;
    [Space]
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private KeyCode jump = KeyCode.Space;
    [Space]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody rigidbody;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dialog.talking)
        {
            Movement(GetInput());
            Jump();
        }
    }

    Vector2 GetInput()
    {
        Vector2 input = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        input.Normalize();
        return input;
    }

    void Movement(Vector2 input)
    {
        rigidbody.velocity = new Vector3(input.x * Time.deltaTime * speed, rigidbody.velocity.y, input.y * Time.deltaTime * speed);
    }

    void Jump()
    {
        if (Input.GetKeyDown(jump))
        {
            rigidbody.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
            audioSource.PlayOneShot(jumpSound);
        }
    }
}
