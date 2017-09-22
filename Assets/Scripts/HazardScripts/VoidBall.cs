using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBall : MonoBehaviour {

    public bool SelfCollide = false;
    public bool enableGravity = false;
    public float angle = 180;
    [Range(0, 100)]
    public float angleRandomizePercentage;

    public float speed = 6.0f;
    [Range(0, 100)]
    public float speedRandomizePercentage;

    private Rigidbody2D _rb;
    private VoidBall _parentVB;

	// Use this for initialization
	void Awake() {
        if (GetComponent<SpawnSystem>() == null)
        {
            _rb = GetComponent<Rigidbody2D>();

            if (transform.parent != null)
            {
                VoidBall spawnerSettings = null;
                spawnerSettings = transform.parent.gameObject.GetComponent<VoidBall>();
                if (spawnerSettings != null)
                {
                    _parentVB = spawnerSettings;
                    SelfCollide = _parentVB.SelfCollide;
                    enableGravity = _parentVB.enableGravity;
                    angle = _parentVB.angle;
                    angleRandomizePercentage = _parentVB.angleRandomizePercentage;
                    speed = _parentVB.speed;
                    speedRandomizePercentage = _parentVB.speedRandomizePercentage;
                }

                angle *= (1 - (Random.Range(-angleRandomizePercentage, angleRandomizePercentage) / 100));
                Vector2 angleVector = Quaternion.Euler(0, 0, -angle) * Vector2.up;

                speed *= (1 - (Random.Range(-speedRandomizePercentage, speedRandomizePercentage) / 100));
                if (!enableGravity)
                {
                    _rb.gravityScale = 0.0f;
                    _rb.isKinematic = true;
                    _rb.velocity = angleVector * speed;
                }
                else
                {
                    _rb.AddForce(angleVector * speed, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player" && GetComponent<SpawnSystem>() == null)
        {
            if (!SelfCollide)
            {
                if (other.gameObject.GetComponent<VoidBall>() == null)
                    Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
}
