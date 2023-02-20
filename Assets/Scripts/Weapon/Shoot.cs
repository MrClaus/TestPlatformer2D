using UnityEngine;

// Shoot-object (bullet) for Weapon
public class Shoot : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2.5f;
    [SerializeField] private float speed = 10f;

    void Start()
    {
        Invoke("Destroy", lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
