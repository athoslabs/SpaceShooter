using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _laserOffset = 1.05f;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _direction;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _shieldVisualizer1;
    [SerializeField]
    private GameObject _shieldVisualizer2;
    [SerializeField]
    private int _shieldHits;

    [SerializeField]
    private GameObject _leftEngineDamage, _rightEngineDamage, _turnOffThruster;

    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _powerUpSound;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private bool _thrusterBoost = false;
    [SerializeField]
    private float _thrusterBoostMultiplier = 2.5f;
    [SerializeField]
    private Slider _thrusterBoostSlider;
    [SerializeField]
    private int _ammoCount = 15;


    // Start is called before the first frame update
    void Start()
    {
        GameInitialization();
    }

    void GameInitialization()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();



        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }

        _leftEngineDamage.SetActive(false);
        _rightEngineDamage.SetActive(false);
        _turnOffThruster.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }     
    }

    void CalculateMovement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _direction.x = _horizontalInput;
        _direction.y = _verticalInput;

        _thrusterBoost = Input.GetKey(KeyCode.LeftShift);

        if(_thrusterBoost == true && _uiManager._isThrusterBoostActive == true )
        {
            transform.Translate(_direction * (_speed * _thrusterBoostMultiplier * Time.deltaTime));
            StartCoroutine(_uiManager.ThrusterBoostSliderDown());
        } 
        else
        {
            transform.Translate(_direction * (_speed * Time.deltaTime));
        }

        if (_uiManager._isThrusterBoostActive == false)
        {
            StartCoroutine(_uiManager.ThrusterBoostSliderUp());
        }





        // Clamp Y position
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.7f, 0.0f), 0);

        if (transform.position.x > 10.97f)
        {
            transform.position = new Vector3(-10.97f, transform.position.y, 0);
        }
        else if (transform.position.x < -10.97f)
        {
            transform.position = new Vector3(10.97f, transform.position.y, 0);
        }
    }



    public void UpdateAmmoCount(int ammo)
    {
        _uiManager.UpdatePlayerAmmo(ammo);
    }

    void FireLaser()
    {
        _audioSource.clip = _laserSound;

        _canFire = Time.time + _fireRate;

        if(_isTripleShotActive == true && _ammoCount > 0)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _ammoCount -= 3;
            UpdateAmmoCount(_ammoCount);
        } 
        else if(_ammoCount > 0)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
            _ammoCount -= 1;
            UpdateAmmoCount(_ammoCount);
        }

        if(_ammoCount > 0)
        {
            _audioSource.PlayOneShot(_laserSound, 0.7f);
        }
       
        
    }

    public void Damage()
    {
        _audioSource.clip = _explosionSound;
        
        if (_isShieldActive == true && _shieldHits >= 2)
        {
            _shieldVisualizer.SetActive(false);
            _shieldVisualizer1.SetActive(true);
            _shieldHits -= 1;
            return;
        }
        else if (_isShieldActive == true && _shieldHits >= 1)
        {
            _shieldVisualizer1.SetActive(false);
            _shieldVisualizer2.SetActive(true);
            _shieldHits -= 1;
            return;
        }
        else if (_isShieldActive == true && _shieldHits <= 0)
        {
            _isShieldActive = false;
            _shieldVisualizer2.SetActive(false);
            return;
        }


        _lives--;

        if(_lives == 2)
        {
            _leftEngineDamage.SetActive(true);
            _audioSource.Play();

          
        }
        else if(_lives == 1)
        {
            _rightEngineDamage.SetActive(true);
            _audioSource.Play();
        }

        _uiManager.UpdateLives(_lives);

        if(_lives <= 0)
        {
            DestroyPlayer();
        }
    }

    void DestroyPlayer()
    {
        _spawnManager.OnPlayerDeath();
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _leftEngineDamage.SetActive(false);
        _rightEngineDamage.SetActive(false);
        _turnOffThruster.SetActive(false);
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
    }

    // Handle PowerUp Methods

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _audioSource.PlayOneShot(_powerUpSound);
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(1.8f);

        _isTripleShotActive = false;

    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _audioSource.PlayOneShot(_powerUpSound);
        _speed *=_speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _shieldHits = 2;
        _isShieldActive = true;
        _audioSource.PlayOneShot(_powerUpSound);
        _shieldVisualizer.SetActive(true);
    }

    public void UpdateAmmo()
    {
        _ammoCount = 15;
        _audioSource.PlayOneShot(_powerUpSound);
        UpdateAmmoCount(_ammoCount);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

}
