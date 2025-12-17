using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _randomX;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -6.82f)
        {
            _randomX = Random.Range(-9.38f, 9.38f);
            transform.position = new Vector3(_randomX, 7.65f, 0.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player = other.transform.GetComponent<Player>();

            if(_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
