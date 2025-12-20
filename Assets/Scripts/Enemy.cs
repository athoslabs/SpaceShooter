using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _randomX;
    private Player _player;
    [SerializeField]
    private Animator _enemyAnim;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Enemy Player GameObject is NULL");
        }

        _enemyAnim = GetComponent<Animator>();

        if(_enemyAnim == null)
        {
            Debug.LogError("Enemy Animator is NULL");
        }
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
            if(_player != null)
            {
                _player.Damage();
            }

            _enemyAnim.SetTrigger("onEnemyDeath");
            _speed = 0.0f;
            Destroy(this.gameObject, 2.8f);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if(_player != null) {
                _player.AddScore(10);
            }

            _enemyAnim.SetTrigger("onEnemyDeath");
            _speed = 0.0f;
            Destroy(this.gameObject, 2.8f);
        }
    }
}
