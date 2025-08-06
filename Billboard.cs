using UnityEngine;

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
