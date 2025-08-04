using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public void ResetarJogo()
    {
        if (PointsManager.Instance != null)
        {
            Debug.Log($"Pontua��o antes de resetar: {PointsManager.Instance.totalPoints}");
            PointsManager.Instance.totalPoints = 0;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
