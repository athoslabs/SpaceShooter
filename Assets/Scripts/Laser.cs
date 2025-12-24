using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    private Player _player;

    // Update is called once per frame
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * (_speed * Time.deltaTime));

        if (transform.position.y > 8.0f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            } 
            else
            {
                Destroy(this.gameObject);
            }
            
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser == true)
        {
            _player = other.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
