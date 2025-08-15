using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

//summaty<>

public class Enemy_Moviment : MonoBehaviour
{
    [SerializeField] private List<Vector2> patrolPositions;
    [SerializeField] private Transform enemyPosition;

    [Header("Configs")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float currentHealth;

    private Vector2 _startPosition;
    private float _lastHitTime;
    private float _lastHealTime;
    private int _index;

    void Start()
    {
        _startPosition = transform.position;
        currentHealth =  maxHealth;
        _index = 0;  
    }
    
    void Update()
    {
        if (!IsEnemyNearby())
        {
            if (currentHealth < maxHealth)
            {
                RetornarParaOrigem();
            }
            else
            {
                Patrulhar();   
            }
        }
        else
        {
            if (!LowHealth())
            {
                PerseguirInimimgo();
            }
            else
            {
                FugirInimigo();
            }
        }
    }

    void Patrulhar()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPositions[_index], moveSpeed * Time.deltaTime);
        VerificarProximidadePatrulha();
    }

    void PerseguirInimimgo()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyPosition.position, moveSpeed * Time.deltaTime );
        if (Vector2.Distance(transform.position, enemyPosition.position) <= 2 && PodeAtacar())
        {
            Atacar();
        }
    }
    void VerificarProximidadePatrulha()
    {
        if (Vector3.Distance(transform.position, patrolPositions[_index]) < 0.1f)
        {
            if (_index + 1 < patrolPositions.Count)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }
        }
    }

    void FugirInimigo()
    {
        transform.position =  Vector2.MoveTowards(transform.position, enemyPosition.position, - moveSpeed * Time.deltaTime);
    }

    void RetornarParaOrigem()
    {
        transform.position = Vector2.MoveTowards(transform.position, _startPosition,  moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _startPosition) < 0.1f && _lastHealTime <= Time.time)
        {
            Curar();
        }
    }
    
    void Atacar()
    {
        _lastHitTime = Time.time + attackCooldown;
        Debug.Log("Toma");
    }

    void Curar()
    {
        _lastHealTime = Time.time + 1f; //cura a cada (1) segundo
        currentHealth += 1f;
        Debug.Log("Curei 1");
        currentHealth = Mathf.Clamp(currentHealth, 0 , maxHealth); 
    }

    bool IsEnemyNearby()
    {
        if (Vector2.Distance(transform.position, enemyPosition.position) < 5f)
        {
            return true;
        }
        return false;
    }

    bool PodeAtacar()
    {
        if (_lastHitTime <= Time.time)
        {
            return true;
        }
        return false;
    }

    bool LowHealth()
    {
        if (currentHealth <= maxHealth * 0.3f)
        {
            return true;
        }
        return false;
    }
}

