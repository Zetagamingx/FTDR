using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 velocity;
    private float lifetime;
    private float timer;

    [SerializeField] private float gravity;
    [SerializeField] private float bulletSpeedUp;

    public void Init(Vector3 direction, float speed, float lifetime)
    {
        velocity = direction * speed;

        velocity.y += bulletSpeedUp;

        this.lifetime = lifetime;
        timer = 0f;
    }

    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
        
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            gameObject.SetActive(false);
        }


    }
}
