using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed;
    public float jumpforce;
    float h;
    float v;


    Rigidbody rb;
    CapsuleCollider theCapsule;

    public bool isGrounded;

    //테스트



    // Start is called before the first frame update
    void Start()
    {
        theCapsule = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        CheckGround();
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");



    }
    private void FixedUpdate()
    {

        Move();

    }

    void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");



        Vector3 horizontal = transform.right * h;
        Vector3 vertical = transform.forward * v;

        Vector3 velocity = (horizontal + vertical).normalized * moveSpeed;



        rb.MovePosition(transform.position + velocity * Time.deltaTime);

    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpforce, 0), ForceMode.Impulse);

        }
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, theCapsule.bounds.extents.y + 0.1f);


    }
}
