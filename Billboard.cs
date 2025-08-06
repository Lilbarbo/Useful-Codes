using UnityEngine;
//<<summary>>
//Script que aplica o efeito de billboard ao objeto, fazendo com que este sempre fique se aparentando 'plano' em relação a camera
//Recomendado o uso para elementos de UI
//<<summary>>

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private bool cameraInicializada = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InicializarCamera();
    }

    void InicializarCamera()
    {
        if (!cameraInicializada)
        {
            mainCamera = Camera.main;
            cameraInicializada = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Garante que a câmera está inicializada
        if (!cameraInicializada)
        {
            InicializarCamera();
        }
        
        // Verifica se a câmera é válida antes de usar
        if (mainCamera != null)
        {
            Quaternion rotation = mainCamera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }
    }
}

