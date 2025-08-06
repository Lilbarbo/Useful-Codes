using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script para pré-carregar objetos de UI e evitar stutter na primeira ativação.
/// Use este script quando tiver elementos de UI que são ativados/desativados durante o gameplay.
/// </summary>
public class UIPreloader : MonoBehaviour
{
    [Header("Objetos para Pré-carregar")]
    [SerializeField] private GameObject[] objetosParaPrecarregar;
    
    [Header("Configurações")]
    [SerializeField] private bool precarregarAutomaticamente = true;
    [SerializeField] private float delayEntreObjetos = 0.01f; // Delay entre cada objeto para distribuir a carga
    
    private bool preCarregamentoConcluido = false;
    
    public bool PreCarregamentoConcluido => preCarregamentoConcluido;
    
    void Start()
    {
        if (precarregarAutomaticamente)
        {
            StartCoroutine(PrecarregarObjetos());
        }
    }
    
    /// <summary>
    /// Pré-carrega todos os objetos definidos
    /// </summary>
    public IEnumerator PrecarregarObjetos()
    {
        if (preCarregamentoConcluido) yield break;
        
        foreach (GameObject obj in objetosParaPrecarregar)
        {
            if (obj != null)
            {
                // Ativa o objeto
                obj.SetActive(true);
                
                // Aguarda uma frame para que todos os componentes sejam inicializados
                yield return null;
                
                // Aguarda um pequeno delay para distribuir a carga
                if (delayEntreObjetos > 0)
                {
                    yield return new WaitForSeconds(delayEntreObjetos);
                }
                
                // Desativa o objeto
                obj.SetActive(false);
            }
        }
        
        preCarregamentoConcluido = true;
        Debug.Log($"[UIPreloader] Pré-carregamento concluído para {objetosParaPrecarregar.Length} objetos");
    }
    
    /// <summary>
    /// Pré-carrega um objeto específico
    /// </summary>
    public IEnumerator PrecarregarObjeto(GameObject objeto)
    {
        if (objeto != null)
        {
            objeto.SetActive(true);
            yield return null;
            objeto.SetActive(false);
        }
    }
    
    /// <summary>
    /// Pré-carrega uma lista de objetos
    /// </summary>
    public IEnumerator PrecarregarLista(List<GameObject> objetos)
    {
        foreach (GameObject obj in objetos)
        {
            yield return StartCoroutine(PrecarregarObjeto(obj));
        }
    }
    
    /// <summary>
    /// Verifica se um objeto está pré-carregado e o ativa de forma otimizada
    /// </summary>
    public void AtivarObjetoOtimizado(GameObject objeto)
    {
        if (objeto != null && !objeto.activeSelf)
        {
            objeto.SetActive(true);
        }
    }
    
    /// <summary>
    /// Desativa um objeto de forma otimizada
    /// </summary>
    public void DesativarObjetoOtimizado(GameObject objeto)
    {
        if (objeto != null && objeto.activeSelf)
        {
            objeto.SetActive(false);
        }
    }
} 