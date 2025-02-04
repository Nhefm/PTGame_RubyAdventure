using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] private bool vertical;
    private Rigidbody2D rb;
    private Animator animator;

    private int flip = 1;
    [SerializeField] private float timeMoving;
    private float timer = 0;

    [SerializeField] private int damage;

    private bool isBroken;

    private AudioSource walkSFX;

    [SerializeField] private ParticleSystem smokeParticle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        walkSFX = GetComponent<AudioSource>();
        isBroken = true;
    }

    void Update()
    {
        if(!isBroken)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= timeMoving)
        {
            timer = 0;
            flip = -flip;
        }

        if (vertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", flip);
        }
        else
        {
            animator.SetFloat("Move Y", 0);
            animator.SetFloat("Move X", flip);
        }
    }

    void FixedUpdate()
    {
        if(!isBroken)
        {
            return;
        }

        Vector2 direction = Vector2.right;

        if (vertical)
        {
            direction = Vector2.up;
        }

        rb.MovePosition(rb.position + Time.deltaTime * speed * flip * direction);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-damage);
        }
    }

    public void Fix()
    {
        isBroken = false;
        rb.simulated = false;
        animator.SetTrigger("Fixed");
        walkSFX.Stop();
        smokeParticle.Stop();
    }
}
