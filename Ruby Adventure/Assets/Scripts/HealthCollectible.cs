using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private int healthPoint;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController controller = collision.GetComponent<RubyController>();
        
        if (controller != null)
        {
            audioSource.Play();
            controller.ChangeHealth(healthPoint);
            Destroy(gameObject);
        }
    }
}
