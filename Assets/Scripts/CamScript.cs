using UnityEngine;

public class CamScript : MonoBehaviour
{
    // Limitações de rotação da câmera
    public float minRotationX = -40f;  // Limite mínimo de rotação no eixo X (para cima e para baixo)
    public float maxRotationX = 40f;   // Limite máximo de rotação no eixo X (para cima e para baixo)
    public float rotationSpeed = 5f;    // Velocidade de rotação da câmera
    public float minRotationY = -80f;  // Limite mínimo de rotação no eixo Y (para esquerda e direita)
    public float maxRotationY = 80f;   // Limite máximo de rotação no eixo Y (para esquerda e direita)

    // Variáveis internas para armazenar a rotação da câmera
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

            // Atualiza a rotação nas direções X e Y com base no movimento do mouse
            currentRotationX -= mouseY;
            currentRotationY += mouseX;

            // Limita a rotação no eixo X (para cima e para baixo)
            currentRotationX = Mathf.Clamp(currentRotationX, minRotationX, maxRotationX);

            // Limita a rotação no eixo Y (para esquerda e para direita)
            currentRotationY = Mathf.Clamp(currentRotationY, minRotationY, maxRotationY);

            // Aplica a rotação na câmera
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
