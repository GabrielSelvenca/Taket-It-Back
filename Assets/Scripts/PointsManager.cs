using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Instance { get; private set; }

    public int totalPoints = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SomarPontos()
    {
        totalPoints += 500;
    }

    public void DiminuirPontos()
    {
        totalPoints -= 750;
    }
}