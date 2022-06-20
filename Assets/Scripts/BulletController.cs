using UnityEngine;
using Random = UnityEngine.Random;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private GameObject[] bloodAnimations;

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
            GameObject toInstantiate = bloodAnimations[Random.Range(0, bloodAnimations.Length)];
            GameObject instance = Instantiate(toInstantiate, contact.point + contact.normal * .0001f, Quaternion.LookRotation(contact.normal));
            var reactivator = instance.AddComponent<DemoReactivator>();
            reactivator.Reactivate();
            other.gameObject.GetComponent<Enemy>().health -= 1;
            Destroy(instance, 2f);
        }
        else
        {
            GameObject decal = GameObject.Instantiate(bulletDecal, contact.point + contact.normal * .01f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject, timeToDestroy);
            Destroy(decal, timeToDestroy);
        }
    }
}
