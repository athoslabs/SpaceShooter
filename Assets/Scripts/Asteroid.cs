using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 19.0f;
    [SerializeField]
    private GameObject _explosionPrefab;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * (_speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.15f);
        }
    }
}
