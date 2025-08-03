using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using Assets.Scripts;

public class MenuManager : MonoBehaviour
{

    [Header("Menu")]
    public Canvas canvasMenu;
    public Image painelFade;
    public float duracaoFade = 1.5f;

    [Header("Panel")]
    public GameObject Panel;
    public TMP_InputField InputName;
    public GameData gameData;

    [Header("Player")] //Perdão pela gambiarra ;-; ---- love u <3
    public GameObject Player;

    [Header("Configs")]
    public MovimentoFunc movimentoFunc;

    private void Awake()
    {
        Panel.SetActive(false);

        Player.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (painelFade != null)
        {
            StartCoroutine(FadeInMenu());
        }
    }

    public void BringMenu()
    {
        if (Panel != null)
        {
            Panel.SetActive (true);
        }
    }

    public void Jogar()
    {
        string nomeJogador = InputName.text.Trim();

        //if (nomeJogador != null) 

        // nomeJodador != null sempre vai ser true
        // pq mesmo que seja nomeJogador = "" ela não é null,
        // pra string sempre use string.IsNullOrEmpty() ou string.IsNullOrWhiteSpace()

        if (string.IsNullOrWhiteSpace(nomeJogador))
        {
            gameData.InserirRanking(nomeJogador, 0);
            Debug.Log($"O nome do jogador é {nomeJogador}");
        }
        else
        {
            Debug.LogWarning("Nome do jogador está vazio.");
            return;
        }

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
        painelFade.enabled = false;
    }

    IEnumerator FadeOutMenu()
    {
        painelFade.enabled = true;

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
        Player.gameObject.SetActive(true);

        movimentoFunc.MoverManual();
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