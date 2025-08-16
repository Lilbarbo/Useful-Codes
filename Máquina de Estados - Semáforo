using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

public class Enemy_Moviment : MonoBehaviour
{
  public enum Estados
  {
    VERDE,
    AMARELO,
    VERMELHO,
    AZUL,
  }
  
  private float tempoDeEsperaVermelho = 5f;
  private float tempoDeEsperaVerde = 5f;
  private float tempoDeEsperaAmarelo = 1f;

  [SerializeField] private SpriteRenderer spriteSemaforo;

  private Estados _estadoAtual = Estados.AMARELO;
  private bool estouEmUmEstado = false;
  private bool pedestrePassando = false;

  private void Update()
  {
    switch (_estadoAtual)
    {
      case Estados.VERMELHO:
        if (!estouEmUmEstado)
        {
          StartCoroutine(SinalVermelho());
        }
        break;
        
      case Estados.AMARELO:
        if (!estouEmUmEstado)
        {
          StartCoroutine(SinalAmarelo()); 
        }
        break;
        
      case Estados.VERDE:
        if (!estouEmUmEstado)
        {
          StartCoroutine(SinalVerde());
        }
        break;
      
      case Estados.AZUL:
        if (!estouEmUmEstado)
        {
          StartCoroutine(SinalAzul());
        }

        break;
    }
    VerificarInput();
  }

  private IEnumerator SinalVermelho()
  {
    estouEmUmEstado = true;
    if (spriteSemaforo.color != Color.red)
    {
      spriteSemaforo.color =  Color.red;
    }
    Debug.Log("Entrei no Vermelho");
    
    yield return new WaitForSeconds(tempoDeEsperaVermelho);
    
    estouEmUmEstado = false;
    pedestrePassando = false;
    Debug.Log("Sai do Vermelho");
    _estadoAtual = Estados.VERDE;
  }
  
  private IEnumerator SinalVerde()
  {
    estouEmUmEstado = true;
    if (spriteSemaforo.color != Color.green)
    {
      spriteSemaforo.color =  Color.green;
    }
    Debug.Log("Entrei no Verde");
    
    yield return new WaitForSeconds(tempoDeEsperaVerde);
    
    if (!pedestrePassando)
    {
      Debug.Log("Sai do Verde Naturalmente");
      estouEmUmEstado = false;
      _estadoAtual = Estados.AMARELO;
    }
    
  }
  
  private IEnumerator SinalAmarelo()
  {
    estouEmUmEstado = true; 
    if (spriteSemaforo.color != Color.yellow)
    {
      spriteSemaforo.color =  Color.yellow;
    }
    Debug.Log("Entrei no Amarelo");
    
    yield return new WaitForSeconds(tempoDeEsperaAmarelo);
    

    if (pedestrePassando)
    {
      Debug.Log("Sai do Amarelo para o Azul");
      estouEmUmEstado = false;
      _estadoAtual = Estados.AZUL;
    }
    else
    {
      Debug.Log("Sai do Amarelo para o Vermelho");
      estouEmUmEstado = false;
      _estadoAtual = Estados.VERMELHO;
    }
  }

  private IEnumerator SinalAzul()
  {
    estouEmUmEstado = true;
    if (spriteSemaforo.color != Color.blue)
    {
      spriteSemaforo.color =  Color.blue;
    }
    Debug.Log("Entrei no Azul");
    
    yield return new WaitForSeconds(tempoDeEsperaVerde - 2f);
    
    estouEmUmEstado = false;
    Debug.Log("Sai do Azul");
    _estadoAtual = Estados.VERMELHO;  
  }

  void VerificarInput()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (!pedestrePassando && _estadoAtual == Estados.VERDE)
      {
          pedestrePassando = true;
          estouEmUmEstado = false;
          Debug.Log("Sai do Verde por Input");
          _estadoAtual = Estados.AMARELO;
      }
    }
  }
}
