using UnityEngine;

public class CamScript : MonoBehaviour
{
    // Limita��es de rota��o da c�mera
    public float minRotationX = -40f;  // Limite m�nimo de rota��o no eixo X (para cima e para baixo)
    public float maxRotationX = 40f;   // Limite m�ximo de rota��o no eixo X (para cima e para baixo)
    public float rotationSpeed = 5f;    // Velocidade de rota��o da c�mera
    public float minRotationY = -80f;  // Limite m�nimo de rota��o no eixo Y (para esquerda e direita)
    public float maxRotationY = 80f;   // Limite m�ximo de rota��o no eixo Y (para esquerda e direita)

    // Vari�veis internas para armazenar a rota��o da c�mera
    float currentRotationX = 0f;
    float currentRotationY = 0f;

    bool bloqueado;
    void Update()
    {
        MouseLiberator();
        if (!bloqueado)
        {
            // Captura o movimento do mouse
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Atualiza a rota��o nas dire��es X e Y com base no movimento do mouse
            currentRotationX -= mouseY;
            currentRotationY += mouseX;

            // Limita a rota��o no eixo X (para cima e para baixo)
            currentRotationX = Mathf.Clamp(currentRotationX, minRotationX, maxRotationX);

            // Limita a rota��o no eixo Y (para esquerda e para direita)
            currentRotationY = Mathf.Clamp(currentRotationY, minRotationY, maxRotationY);

            // Aplica a rota��o na c�mera
            transform.localRotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }


    }

    void MouseLiberator()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Cursor.lockState = CursorLockMode.Confined;
            bloqueado = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            bloqueado = false;
        }
    }

}
