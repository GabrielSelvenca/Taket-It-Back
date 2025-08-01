using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float lookSpeed = 1f;   // Velocidade de rotação da câmera
    public float lookXLimit = 90f; // Limite vertical (90° para cima e 90° para baixo)
    public float lookYLimit = 60f;  // Limite horizontal (60° para esquerda e 60° para direita)

    private float rotationX = 0;   // Armazena o ângulo acumulado de rotação vertical (eixo X)
    private float rotationY = 0;   // Armazena o ângulo acumulado de rotação horizontal (eixo Y)

    [Header("Mouse")]
    public Texture2D cursorPoint;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        float x = (Screen.width - cursorPoint.width) / 2;
        float y = (Screen.height - cursorPoint.height) / 2;
        GUI.DrawTexture(new Rect(x, y, cursorPoint.width, cursorPoint.height), cursorPoint);
    }

    void Update()
    {
        var mouse = Mouse.current;

        if (mouse != null)
        {
            // Rotação VERTICAL (cima/baixo)
            rotationX += -mouse.delta.y.ReadValue() * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Rotação HORIZONTAL (esquerda/direita)
            rotationY += mouse.delta.x.ReadValue() * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);

            // Aplica a rotação na câmera (sem afetar o corpo do jogador)
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        }
    }
}