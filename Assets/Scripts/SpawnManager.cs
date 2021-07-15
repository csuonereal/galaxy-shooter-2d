using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject _powerupContainer;

    private bool IsPlayerAlive = true;
    //void Start()
    //{
    //    StartCoroutine(SpawnEnemyRoutine());
    //    StartCoroutine(SpawnPowerupRoutine());
    //}

    public void AsteroidStartCoroutine()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    void Update()
    {

    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        while (IsPlayerAlive)
        {
            Vector3 pos = new Vector3(Random.Range(-8.5f, 8.5f), 6, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        while (IsPlayerAlive)
        {
            Vector3 pos = new Vector3(Random.Range(-8.5f, 8.5f), 6, 0);
            int randomPowerUp = Random.Range(0, 3);//0 olabilir 2 olamaz.
            GameObject newPowerup = Instantiate(powerups[randomPowerUp], pos, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            float WaitingTime = Random.Range(15f, 20f);
            yield return new WaitForSeconds(WaitingTime);
        }
    }
    public void OnPlayerDeath()
    {
        IsPlayerAlive = false;
    }
}
