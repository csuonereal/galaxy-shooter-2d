using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _speed;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = .1f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 6;

    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private int _speedBoostMultiplier = 2;
    private bool _isShieldActive;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;


    [SerializeField]
    public int _score;

    private UIManager _uiManager;


    [SerializeField]
    private AudioClip _laserSFX;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        //önce gameobjesine ulaştık sonra onun componenti olan spawnmanager scriptine ulaştık.aşağıdada _spawnManageri kullanarak metoduna ulaşıcaz.
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("_spawnManager is null");
        }
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("_uiManager is null");
        }
    }

    void Update()
    {
        shoot();
    }
    private void FixedUpdate()
    {
        move();
    }
    void move()
    {
        //transform.Translate(new Vector3(_speed * Time.deltaTime, transform.position.y,transform.position.z));
#if UNITY_EDITOR_WIN
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //aşağıdaki gibide yapabilirdin ama tek satırda yapmak daha optimize olucaktır
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
#endif
#if UNITY_ANDROID
        float horInput = CrossPlatformInputManager.GetAxis("Horizontal");
        float verInput = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 direction2 = new Vector3(horInput, verInput, 0);
        transform.Translate(direction2 * _speed * Time.deltaTime);
#endif

        if (transform.position.x >= 9.2f)
        {
            transform.position = new Vector3(-9.2f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.2f)
        {
            transform.position = new Vector3(9.2f, transform.position.y, 0);
        }
        //mathf ile yapıcam aşağıdakileri şimdi
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 4), 0);

        //if (transform.position.y >= 5)
        //{
        //    transform.position = new Vector3(transform.position.x, 5f, 0);
        //}
        //else if (transform.position.y <= -5)
        //{
        //    transform.position = new Vector3(transform.position.x, -5f, 0);
        //}

    }
    void shoot()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire")) && Time.time > _canFire)//Time.time oyunun başlamasından sonuna kadar geçen süreyi gösteriyor.
        {
            AudioSource.PlayClipAtPoint(_laserSFX, transform.position);
            _canFire = Time.time + _fireRate; //bu sayede sonsuza kadar ateş edemiyecek...
            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                if (transform.position.y >= 7f)
                {
                    Destroy(_tripleShotPrefab);
                }
            }
            else
            {
                var l = Instantiate(_laserPrefab, transform.position + new Vector3(0f, .85f, 0f), Quaternion.identity);
            }
        }
    }
    public void damage()
    {

        if (_isShieldActive == true)//shieldi aldıgında buraya giricek ve shield kalkıcak hasar 1 defasına almıyacak.isshieldi false yapsam buraya hiç girmiyecekti ve onu yazmasam sonsuz loopa girecekti..
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives--;
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        _uiManager.updateLives(_lives);
        if (_lives < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
            _uiManager.displayGameOver();
        }

    }

    public void TripleShotActive()//triple_shotu aldığımızda powerup etkin olucak ve 3 lü ateş edebileceğiz 5sn için...
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedActive = true;
        _speed *= _speedBoostMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }
    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedBoostMultiplier;
        _isSpeedActive = false;
    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        //shieldin bir süresi olmuyacak hasar aldığım an gidicek kalkanlar bir hasar alma hakkı veriyo bana kısaca. bu yüzden powerdownu damagenin içine yazıcam
        _shieldVisualizer.SetActive(true);

    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.updateScore(_score);
    }
}
