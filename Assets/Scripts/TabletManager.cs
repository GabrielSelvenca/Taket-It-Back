using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabletManager : MonoBehaviour
{
    public TMP_InputField inputId;
    public Button btnBuscar;
    public RectTransform errorPopUp;
    public TextMeshProUGUI textoPegos;
    public GameObject pesquisarObj;
    public GameObject listaObj;

    public void Buscar()
    {
        if (int.TryParse(inputId.text.Trim(), out int id))
        {
            var funcionario = GameData.Instance.GetFuncionario(id);
            var consumiveis = GameData.Instance.BuscarModelosConsumiveisFuncionario(id);

            if (funcionario != null && consumiveis != null && consumiveis.Count > 0)
            {
                pesquisarObj.SetActive(false);
                listaObj.SetActive(true);

                textoPegos.text = $"Pegou:\n";
                foreach (var cons in consumiveis)
                {
                    textoPegos.text += $"{cons.nome} x{cons.qtd}\n";
                }
            }
            else
            {
                Debug.LogWarning("Funcionário não encontrado ou sem consumíveis.");
                ErrorPopUp();
            }
        }
        else
        {
            Debug.LogWarning("ID inválido");
            ErrorPopUp();
        }
    }

    public void Voltar()
    {
        pesquisarObj.SetActive(true);
        listaObj.SetActive(false);
        textoPegos.text = string.Empty;
    }

    public void ErrorPopUp()
    {
        errorPopUp.gameObject.SetActive(true);
        pesquisarObj.SetActive(false);
        StartCoroutine(ErrorPopOut());
    }

    private IEnumerator ErrorPopOut()
    {
        yield return new WaitForSeconds(5);
        errorPopUp.gameObject.SetActive(false);
        pesquisarObj.SetActive(true);
    }
}