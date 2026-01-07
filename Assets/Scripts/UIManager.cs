using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _LivesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;
    [SerializeField]
    private Slider _thrusterBoostSlider;
    [SerializeField]
    private float _thrusterBoostSliderMaxValue = 100.0f;
    public bool _isThrusterBoostActive = true;
    [SerializeField]
    private Text _ammoLeft;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoLeft.text = "Ammo: " + 15;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _thrusterBoostSlider.value = _thrusterBoostSliderMaxValue;

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    public void UpdatePlayerAmmo(int ammo)
    {
        if(ammo < 0)
        {
            ammo = 0;
        }

        _ammoLeft.text = "Ammo: " + ammo.ToString();
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if(currentLives < 0)
        {
            currentLives = 0;
        }

        _livesImage.sprite = _LivesSprites[currentLives];

        if(currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }


    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator ThrusterBoostSliderDown()
    {
        _thrusterBoostSlider.value -= 0.15f;
        yield return new WaitForSeconds(1.5f);

        if(_thrusterBoostSlider.value <= 0.0f)
        {
            _thrusterBoostSlider.value = 0.0f;
            _isThrusterBoostActive = false;
        }
    }

    public IEnumerator ThrusterBoostSliderUp()
    {
        while(_thrusterBoostSlider.value < _thrusterBoostSliderMaxValue && _isThrusterBoostActive == false)
        {
            _thrusterBoostSlider.value += 0.15f;
            yield return new WaitForSeconds(2.5f);

            if(_thrusterBoostSlider.value == _thrusterBoostSliderMaxValue)
            {
                _isThrusterBoostActive = true;
            }
        }
    }

}
