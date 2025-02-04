using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class RubyController : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private int maxHealth;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private float vertical;
    private Vector2 lookDirection;

    [SerializeField] private float timeInvincible;
    private bool isInvincible = false;

    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource walkSFX;

    [SerializeField] private Vector2 boundary;

    [SerializeField] private ParticleSystem hitParticle;

    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if(isInvincible)
            {
                return;
            }

            StartCoroutine(InvincibleTimer());
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue((float)currentHealth / maxHealth);
    }

    public IEnumerator InvincibleTimer()
    {
        animator.SetTrigger("Hit");
        hitSFX.Play();
        hitParticle.Play();
        isInvincible = true;
        yield return new WaitForSeconds(timeInvincible);
        hitParticle.Stop();
        hitParticle.Clear();
        isInvincible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = ObjectPooling.sharedInstance.getActiveObject();

            if(!bullet)
            {
                return;
            }

            bullet.SetActive(true);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = new Quaternion(0,0,0,0);
            bullet.transform.Rotate(Mathf.Atan2(lookDirection.y , lookDirection.x) * Mathf.Rad2Deg * Vector3.forward);
            animator.SetTrigger("Launch");
        }

        if(Mathf.Approximately(horizontal, 0) && Mathf.Approximately(vertical, 0))
        {
            walkSFX.Stop();
            return;
        }

        if(!walkSFX.isPlaying)
        {
            walkSFX.Play();
        }
        
        lookDirection.Set(horizontal, vertical);
        lookDirection.Normalize();

        animator.SetFloat("Pos X", lookDirection.x);
        animator.SetFloat("Pos Y", lookDirection.y);
        animator.SetFloat("Speed", new Vector2(horizontal, vertical).magnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + speed * Time.deltaTime * new Vector3( horizontal, vertical, 0));

        float x = Mathf.Clamp(transform.position.x, -boundary.x, boundary.x);
        float y = Mathf.Clamp(transform.position.y, -boundary.y, boundary.y);
        transform.position = new Vector3(x,y);
    }
}
