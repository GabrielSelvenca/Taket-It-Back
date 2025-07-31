using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Approval : MonoBehaviour
{
    public int funcionarioAtualId;
    PointsManager pm = new PointsManager();

    public void OnAprovar()
    {
        if (GameData.Instance.VerificarEntregasValidas(funcionarioAtualId))
        {
            Debug.Log("Aprovado corretamente.");
            pm.SomarPontos();
        }
        else
        {
            Debug.Log("Erro! Voc� aprovou algu�m com itens inv�lidos.");
            pm.DiminuirPontos();
        }
    }

    public void OnReprovar()
    {
        if (!GameData.Instance.VerificarEntregasValidas(funcionarioAtualId))
        {
            Debug.Log("Reprovado corretamente.");
            pm.SomarPontos();
        }
        else
        {
            Debug.Log("Erro! Voc� reprovou uma entrega v�lida.");
            pm.DiminuirPontos();
        }
    }
}
