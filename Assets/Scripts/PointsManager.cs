using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public int totalPoints = 0;

    public void CriarUsuario()
    {
        
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
