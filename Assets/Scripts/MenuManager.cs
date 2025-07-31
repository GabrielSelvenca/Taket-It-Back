using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{
    public Canvas canvasMenu;
    public Image painelFade;
    public Button startButton;
    public float duracaoFade = 1.5f;

    private void Start()
    {
        if (painelFade != null)
        {
            StartCoroutine(FadeInMenu());
        }
    }

    public void Jogar()
    {
        StartCoroutine(FadeOutMenu());
    }

    IEnumerator FadeInMenu()
    {
        float tempo = 0f;
        Color corInicial = new Color(0, 0, 0, 1f);
        Color corFinal = new Color(0, 0, 0, 0f);

        painelFade.color = corInicial;

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            painelFade.color = Color.Lerp(corInicial, corFinal, tempo / duracaoFade);
            yield return null;
        }

        painelFade.color = corFinal;
    }

    IEnumerator FadeOutMenu()
    {
        float tempo = 0f;
        Color corInicial = painelFade.color;
        Color corFinal = new Color(0, 0, 0, 1f);
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            painelFade.color = Color.Lerp(corInicial, corFinal, tempo / duracaoFade);
            yield return null;
        }

        painelFade.color = corFinal;
        canvasMenu.gameObject.SetActive(false);
    }

    public void Tutorial()
    {
        Debug.Log("Tela de tutorial ainda não implementada.");
    }

    public void Sair()
    {
        Application.Quit();
    }
}