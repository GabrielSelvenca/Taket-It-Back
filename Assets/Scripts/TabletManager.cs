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
    private List<FuncionarioConsumivel> listaConsumiveisFuncionario;
    private Funcionarios funcionario;

    public void Buscar()
    {
        if (int.TryParse(inputId.text.Trim(), out int id))
        {
            funcionario = GameData.Instance.GetFuncionario(id);
            listaConsumiveisFuncionario = GameData.Instance.GetConsFunc(id);

            if (funcionario != null && listaConsumiveisFuncionario != null && listaConsumiveisFuncionario.Count > 0)
            {
                Debug.Log($"Funcionario {funcionario.nome} encontrado com {listaConsumiveisFuncionario.Count} consumiveis.");
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

    public void ErrorPopUp()
    {
        errorPopUp.gameObject.SetActive(true);
        btnBuscar.gameObject.SetActive(false);
        StartCoroutine(ErrorPopOut());
    }

    private IEnumerator ErrorPopOut()
    {
        yield return new WaitForSeconds(5);
        errorPopUp.gameObject.SetActive(false);
    }
}