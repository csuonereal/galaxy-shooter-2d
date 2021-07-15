using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 3.0f;
    private Player _player;

    [SerializeField]
    private int powerupID; //0->triple shot, 1->speed boost , 2->shield

    [SerializeField]
    private AudioClip _powerupSFX;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerupSFX, transform.position);
            Destroy(this.gameObject);
            switch (powerupID)
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
                default:
                    Debug.Log("Default Value");
                    break;
            }

        }
    }
}
