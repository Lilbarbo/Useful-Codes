using System;
using UnityEngine;
using System.Collections.Generic;

public class MovimentStateMachine : MonoBehaviour
{
    public enum Estados
    {
        PATRULHA,
        PERSEGUIR,
        ATACAR,
        FUGIR,
        RETORNAR,
        CURAR,
    }
    
    private Estados estadoAtual = Estados.PATRULHA;

    [SerializeField] private List<Vector2> posicoesPatrulha;
    [SerializeField] private Transform enemyTransform;

    [SerializeField] private float vidaMaxima = 10f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float atackRange = 2f;
    [SerializeField] private float atackCooldown = 1f;
    [SerializeField] private float healCooldown = 1f;
    [SerializeField] private float vidaAtual;


    private int _indexPatrulha;
    private float _lastAtackTime;
    private float _lastHealTime;
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
        vidaAtual = vidaMaxima; 
        _indexPatrulha = 0;  
    }

    private void Update()
    {
        switch (estadoAtual)
        {
            case Estados.PATRULHA:
                Patrulhar();
                break;
            case Estados.PERSEGUIR:
                Perseguir();
                break;
            case Estados.ATACAR:
                Atacar();
                break;
            case Estados.FUGIR:
                Fugir();
                break;
            case Estados.RETORNAR:
                RetornarBase();
                break;
            case Estados.CURAR:
                Curar();
                break;
        }
    }

    private void Patrulhar()
    {
        if (vidaAtual == vidaMaxima)
        {
            transform.position = Vector2.MoveTowards(transform.position, posicoesPatrulha[_indexPatrulha], moveSpeed * Time.deltaTime );
            if (Vector2.Distance(transform.position, posicoesPatrulha[_indexPatrulha]) < 0.1f) //se eu cheguei na posição
            {
                if (_indexPatrulha + 1 >= posicoesPatrulha.Count)
                {
                    _indexPatrulha = 0;
                }
                else
                {
                    _indexPatrulha++;
                }
            }

            if (InimigoProximo())
            {
                estadoAtual = Estados.PERSEGUIR;
            }
        }
        else
        {
            estadoAtual = Estados.RETORNAR;
        }
    }

    private void Perseguir()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyTransform.position, moveSpeed * Time.deltaTime );

        if (!VidaBaixa())
        {
            if (!InimigoProximo())
            {
                if (vidaAtual < vidaMaxima)
                {
                    estadoAtual =  Estados.RETORNAR;
                }
                else
                {
                    estadoAtual = Estados.PATRULHA;
                }
            }

            if (PossoAtacar())
            {
                estadoAtual = Estados.ATACAR;
            }
        }
        else
        {
            estadoAtual = Estados.FUGIR;
        }
    }

    private void Atacar()
    {
        Debug.Log("Ataquei");
        _lastAtackTime = Time.time + atackCooldown;
        
        if (!PossoAtacar())
        {
            estadoAtual = Estados.PERSEGUIR;
        }
    }

    private void Fugir()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyTransform.position, -moveSpeed * Time.deltaTime );
        if (!InimigoProximo())
        {
            estadoAtual = Estados.RETORNAR;
        }
    }

    private void RetornarBase()
    {
        if (InimigoProximo())
        {
            if (VidaBaixa())
            {
                estadoAtual =  Estados.FUGIR;
            }
            else
            {
                estadoAtual = Estados.PERSEGUIR;
            }
        }
        
        transform.position = Vector2.MoveTowards(transform.position, _initialPosition, moveSpeed * Time.deltaTime );
        if (Vector2.Distance(transform.position, _initialPosition) < 0.1f)
        {
            estadoAtual = Estados.CURAR;
        }
    }

    private void Curar()
    {
        _lastHealTime += Time.deltaTime;
        if (_lastHealTime >= healCooldown)
        {
            vidaAtual++;
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
            _lastHealTime = 0f;
        }

        if (vidaAtual >= vidaMaxima)
        {
            estadoAtual = Estados.PATRULHA;
        }
    }
    
    

    private bool PossoAtacar()
    {
        if (_lastAtackTime <= Time.time && Vector2.Distance(transform.position, enemyTransform.position) <= atackRange)
        {
            return true;
        }
        return false;
    }
    private bool InimigoProximo()
    {
        if (Vector2.Distance(transform.position, enemyTransform.position) <= detectionRange)
        {
            return true;    
        }
        return false;
    }
    private bool VidaBaixa()
    {
        if (vidaAtual <= vidaMaxima * 0.3f)
        {
            return true;
        }
        return false;   
    }
}
