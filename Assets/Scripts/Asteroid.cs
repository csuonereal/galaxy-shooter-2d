using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;

    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioClip _explosionSFX;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);//z aksisinde dönmesini istediğimdeen forwardi seçtim   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(this.gameObject, .25f);
            AudioSource.PlayClipAtPoint(_explosionSFX, transform.position);
            _spawnManager.AsteroidStartCoroutine();


        }
    }
}
