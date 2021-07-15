using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _enemyHealth = 1f;


    Player player;

    private Animator _animator;

    [SerializeField]
    private AudioClip _explosionSFX;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("The Animator is NULL");
        }
    }


    void Update()
    {
        enemyMovement();
    }
    void enemyMovement()
    {
        transform.Translate(Vector3.up * -_speed * Time.deltaTime);
        if (transform.position.y <= -6)
        {
            transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 6, 0);
            //Destroy(gameObject);
        }
    }

    //aşağıdakini kullanabilmen için çarpanların en az birinde rigidbody olmak zorunda 


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            _enemyHealth--;

            Destroy(other.gameObject);
            if (_enemyHealth == 0)
            {
                _animator.SetTrigger("OnEnemyDeath");  //for animation startta tanımlamamızın sebebi her düşman vurulduğunda bunu yapmak yorucu olabilir bu yüzden startın içinde tanımlamamız daha uygun.
                _speed = 0.3f;//patlarken bize tekrar çarpmaması için,transletten hasexit time'ı kapat animasyonda gecikme olmaması için ve empty animasyonun boşyere beklememek için ayrıca tranlation durationuda 0 'a çek
                AudioSource.PlayClipAtPoint(_explosionSFX, transform.position);
                Destroy(gameObject, 2.3f);
              
                //for score
                if (player != null)
                {
                    int point = Random.Range(10, 21);
                    player.AddToScore(point);
                }


            }
        }
        if (other.gameObject.tag == "Player")
        {
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.3f;
            AudioSource.PlayClipAtPoint(_explosionSFX, transform.position);
            Destroy(this.gameObject, 2.3f);
           
            other.transform.GetComponent<Player>().damage();
        }
    }
}
