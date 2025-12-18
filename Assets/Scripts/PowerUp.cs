using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    private Player _player;
    [SerializeField]    // 0 = TripleShot, 1 = Speed, 2 = Shields
    private int _powerUpID;

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < -7.0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player = other.GetComponent<Player>();
            if(_player != null)
            {
                switch(_powerUpID)
                {
                    case 0:
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        Debug.Log("Sheild Power Up");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }              
            }

            Destroy(this.gameObject);
        }
    }
}
