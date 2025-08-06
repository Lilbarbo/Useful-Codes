using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Animators References")]
    [SerializeField] Animator animatorMao;
    [SerializeField] Animator animatorLivro;


    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;   
    public float gravity = -9.81f;

    [Header("Jump Settings")]
    public float jumpHeight = 1.5f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    [Header("Mouse Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;

    [Header("Impulse Settings")]
    [SerializeField] float airDrag = 2f;   // menor = dura mais tempo
    [SerializeField] float groundDrag = 15f; // maior = freia quase instantâneo ao pousar

    private CharacterController controller;
    private Vector3 velocity;
    private float pitch = 0f;
    private Vector3 externalImpulse;      // ← novo! armazena o momentum do teleporte
    private bool hasImpulse;              // aux. para saber se ainda há impulso
    private bool isRunning;               // ← novo! controla o estado de corrida

    // ← novo! Controla se a gravidade deve ser aplicada
    public bool gravityEnabled = true;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        UpdateAnimations(); // ← novo! atualiza as animações
    }

    void HandleMovement()
    {
        /* 1) Ground check */
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;                     // “cola” no chão

        /* 2) WASD */
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        /* 3) Sprint */
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        
        // ← novo! Atualiza o estado de corrida
        isRunning = isSprinting && (x != 0f || z != 0f);

        controller.Move(move * currentSpeed * Time.deltaTime);

        /* 4) Jump */
        if (isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        /* 5) Gravidade */
        if (gravityEnabled)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        /* 6) Aplica impulso externo (se houver) */
        if (hasImpulse)
        {
            controller.Move(externalImpulse * Time.deltaTime);

            float drag = isGrounded ? groundDrag : airDrag;
            externalImpulse = Vector3.Lerp(externalImpulse, Vector3.zero, drag * Time.deltaTime);

            // se ficou quase zero, desliga a flag
            if (externalImpulse.sqrMagnitude < 0.01f)
                hasImpulse = false;
        }

        /* 7) Move vertical (gravidade) */
        controller.Move(velocity * Time.deltaTime);
    }

    // ← novo! Método para atualizar as animações
    void UpdateAnimations()
    {
        if (animatorMao != null)
        {
            animatorMao.SetBool("Run_Bool", isRunning);
        }
        
        if (animatorLivro != null)
        {
            animatorLivro.SetBool("Run_Bool", isRunning);
        }
    }

    void HandleMouseLook()
    {
        // Rotação horizontal (yaw)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // Rotação vertical (pitch)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -verticalLookLimit, verticalLookLimit);
        playerCamera.localEulerAngles = Vector3.right * pitch;
    }

    public void SetExternalVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;   // 'velocity' já existe no script
    }

    public void AddImpulse(Vector3 worldVelocity)
    {
        ResetGravity();
        externalImpulse += worldVelocity;
        hasImpulse = true;
    }

    public void ResetGravity()
    {
        velocity.y = 0f;
    }

    // ← novo! Métodos para controlar a gravidade
    public void DisableGravity()
    {
        gravityEnabled = false;
        velocity.y = 0f; // Reseta a velocidade vertical quando desabilita a gravidade
    }

    public void EnableGravity()
    {
        gravityEnabled = true;
    }

    public bool IsGravityEnabled()
    {
        return gravityEnabled;
    }

    // Visualização do ground check no Editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
