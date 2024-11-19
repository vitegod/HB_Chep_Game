using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 300f;
    [SerializeField] private float jumpForce = 350;
    // [SerializeField] private float cooldownHitTime = 0.5f;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDead = false;

    private float horizontal;
    

    private int coin = 0;

    private Vector3 savePoint;


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Player tag: " + gameObject.tag);
        SavePoint();
        
        OnInit();
    }

    private void OnInit() {
        isGrounded = true;
        isJumping = false;
        isAttacking = false;
        isDead = false;

        transform.position = savePoint;
        ChangeAnim("idle");
    }

    void Update()
    {
        if (isGrounded && !isJumping && !isAttacking)
        {
            HandleInput();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead) {
            return;
        }

        bool wasGrounded = isGrounded;

        isGrounded = CheckGrounded();

        if (isGrounded && !wasGrounded)
        {
            isJumping = false;
            ChangeAnim("idle");
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        horizontal = Input.GetAxisRaw("Horizontal");

        UpdateMovement();

        // CheckEdges();
    }

    private void CheckEdges()
    {
        RaycastHit2D sideHit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(horizontal), 0.5f, groundLayer);

        if (sideHit.collider != null && !isGrounded)
        {
            rb.AddForce(new Vector2(Mathf.Sign(horizontal) * 10f, -10f));
        }
    }

    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    private void HandleInput()
    {
        if (isJumping || isAttacking) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Throw();
        }

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            ChangeAnim(isGrounded ? "run" : "jump");
        }
    }


    private void UpdateMovement()
    {
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            ChangeAnim(isGrounded ? "run" : "jump");
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, (horizontal > 0) ? 0 : 180, 0));
        }
        else if (isGrounded && Mathf.Abs(horizontal) <= 0.1f)
        {
            ChangeAnim("idle");
            rb.velocity = new Vector2(0, rb.velocity.y);
            // rb.velocity = Vector2.zero;
        }
        else if (!isGrounded && rb.velocity.y < -0.1f)
        {
            ChangeAnim("fall");

        }
        else if (!isGrounded && rb.velocity.y > 0.1f)
        {
            ChangeAnim("jump");
        }
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rb.AddForce(Vector2.up * jumpForce);
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        ChangeAnim("attack");
        //yield return new WaitForSeconds(cooldownHitTime);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        ChangeAnim("idle");
        isAttacking = false;
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator ThrowCoroutine()
    {
        isAttacking = true;
        ChangeAnim("throw");
        //yield return new WaitForSeconds(cooldownHitTime);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        ChangeAnim("idle");
        isAttacking = false;
    }

    private void Throw()
    {
        if (!isAttacking)
        {
            StartCoroutine(ThrowCoroutine());
        }
    }

    internal void SavePoint() {
        savePoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Coin") {
            // Debug.Log("Received coin" + collision.gameObject.name);
            Destroy(collision.gameObject);
            coin++;
        }
        if (collision.tag == "DeathZone") {
            ChangeAnim("die");
            isDead = true;

            Invoke(nameof(OnInit), 0.5f);
        }
    }
}