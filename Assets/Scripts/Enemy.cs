﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _anim;
    private AudioSource _audioSource;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if(_player == null)
        {
            Debug.LogError("Player Component NULL");
        }

        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5.0f)
        {
            float randomX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7.4f, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }
    }
}
