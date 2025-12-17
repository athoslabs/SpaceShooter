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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(_stopSpawning == false)
        {
            _positionToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7.0f, 0.0f);
            _newEnemy = Instantiate(_enemyPrefab, _positionToSpawn, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
