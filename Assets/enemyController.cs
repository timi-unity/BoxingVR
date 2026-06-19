using UnityEngine;

public class enemyController : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float followDistance = 5f;

    private Rigidbody rb;
    private Animator an;
    private bool lastDerechazo=false;
    public handCollision hc1,hc2;
    private float lastPunchTime=0;
    public float punchCooldown = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance > 2f && Time.time > lastPunchTime + punchCooldown)
        {
            punch();
        }

        if (distance > followDistance)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                5f * Time.fixedDeltaTime
            );

            rb.MovePosition(
                rb.position + direction * speed * Time.fixedDeltaTime
            );

            an.SetFloat("speed", speed);
        }
        else
        {
            an.SetFloat("speed", 0);

            // Opcional: mirar al jugador sin moverse
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;

            if (lookDir.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDir),
                    5f * Time.fixedDeltaTime
                );
            }
        }
    }

    private void punch()
    {
        lastPunchTime = Time.time;
        an.SetBool("lastDerechazo", lastDerechazo);        
        an.SetTrigger("punch");
        if (lastDerechazo)
        {
            hc1.gameObject.SetActive(true);
        }
        else
        {
            hc2.gameObject.SetActive(true);
        }
        lastDerechazo = !lastDerechazo;
    }

    private void stopPunch()
    {
        hc1.gameObject.SetActive(false);
        hc2.gameObject.SetActive(false);
    }
}