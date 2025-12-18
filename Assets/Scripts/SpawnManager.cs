using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;
    private GameObject _newEnemy;
    private Vector3 _positionToSpawn;
    [SerializeField]
    private float _randomPowerUpTime;
    private Vector3 _positionToSpawnPowerUp;
    [SerializeField]
    private GameObject[] _powerUps;
    private int _randomPowerUp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(_stopSpawning == false)
        {
            _positionToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7.0f, 0.0f);
            _newEnemy = Instantiate(_enemyPrefab, _positionToSpawn, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while(_stopSpawning == false)
        { 
           _positionToSpawnPowerUp = new Vector3(Random.Range(-9.0f, 9.0f), 7.0f, 0.0f);
           _randomPowerUp = Random.Range(0, 3);
           Instantiate(_powerUps[_randomPowerUp], _positionToSpawnPowerUp, Quaternion.identity);

            _randomPowerUpTime = Random.Range(3.0f, 8.0f);

            yield return new WaitForSeconds(_randomPowerUpTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
