using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    private Player _player;
    [SerializeField]    // 0 = TripleShot, 1 = Speed, 2 = Shields, 3 = Ammo
    private int _powerUpID;
    [SerializeField]
    private AudioClip _powerUpAudioClip;

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

            AudioSource.PlayClipAtPoint(_powerUpAudioClip, transform.position, 1.0f);

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
                        _player.ShieldActive();
                        break;
                    case 3:
                        _player.UpdateAmmo();
                        break;
                    case 4:
                        _player.AddLife();
                        break;
                    case 5:
                        _player.FireBall();
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
