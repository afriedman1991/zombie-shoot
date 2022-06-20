using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    private float speed = 50f;
    public float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().health -= 1;
        }
        else
        {
            GameObject decal = GameObject.Instantiate(bulletDecal, contact.point + contact.normal * .01f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject, timeToDestroy);
            Destroy(decal, timeToDestroy);
        }
    }
}
