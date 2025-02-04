using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JambiController : MonoBehaviour
{
    private Animator animator;
    private bool isBlinking = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBlinking)
        {
            StartCoroutine(Blink());
        }
    }

    public IEnumerator Blink()
    {
        animator.SetTrigger("Blink");
        isBlinking = true;
        yield return new WaitForSeconds(Random.Range(2, 4));
        isBlinking = false;
    }
}
