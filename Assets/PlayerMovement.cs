using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // Horizontal movement speed
    public float jumpForce = 300f;     // Jump force
    public bool isGrounded = false;    // Basic grounded check for jumping

    public UIManager UIManager;

    public int coins = 0;

    private Rigidbody2D rb;
    private Animator anim;

    void Awake()
    {
        // Get references
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1) Horizontal Movement
        float move = Input.GetAxis("Horizontal"); // -1 to 1
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // 2) Face direction of movement (optional)
        if (move != 0)
            {
                anim.SetBool("isWalking", true);
            }
        else
            {
                anim.SetBool("isWalking", false);
            }

        // 3) Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }

        // 4) Update Animator (optional for “Make Animation” step)
        if (anim != null)
        {
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    // Basic ground check using collision
    void OnCollisionEnter2D(Collision2D col)
    {
        // If bottom of Player hits something tagged "Ground", set isGrounded = true
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (col.gameObject.CompareTag("Coin"))
        {
            coins++;
            UIManager.UpdateCoinText(coins);
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("Win");
        }
    }
}
