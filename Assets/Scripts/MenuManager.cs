using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using Assets.Scripts;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{

    [Header("Menu")]
    public Canvas canvasMenu;
    public Image painelFade;
    public GameObject telaInicial;
    public float duracaoFade = 1.5f;
    public Camera MenuCam;

    [Header("Tutorial")]
    public Canvas canvasTutorial;
    public Button leftArrow;
    public Button rightArrow;
    public Transform paiSlides;
    private List<Transform> Slides = new();
    private int slideAtual = 0;

    [Header("Panel")]
    public GameObject Panel;
    public TMP_InputField InputName;
    public GameData gameData;

    [Header("Player")] //Perdão pela gambiarra ;-; ---- love u <3
    public GameObject Player;

    [Header("Configs")]
    public MovimentoFunc movimentoFunc;

    [Header("Audio")]
    public AudioSource menuAudioSource;
    public AudioSource gameAudioSource;

    private void Awake()
    {
        if(Panel != null)
            Panel.SetActive(false);

        if (canvasTutorial != null )
            canvasTutorial.gameObject.SetActive(false);

        if (Player != null)
            Player.gameObject.SetActive(false);

        gameAudioSource.enabled = false;
        menuAudioSource.enabled = true;
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
            telaInicial.SetActive(false);
            Panel.SetActive(true);
        }
    }

    public void Jogar()
    {
        string nomeJogador = InputName.text.Trim();

        //if (nomeJogador != null) 

        // nomeJodador != null sempre vai ser true
        // pq mesmo que seja nomeJogador = "" ela não é null,
        // pra string sempre use string.IsNullOrEmpty() ou string.IsNullOrWhiteSpace()

        if (!string.IsNullOrWhiteSpace(nomeJogador))
        {
            gameData.InserirRanking(nomeJogador, 0);
        }
        else
        {
            StartCoroutine(PreencherCampo(InputName));
            Debug.LogWarning("Nome do jogador está vazio.");
            return;
        }

        StartCoroutine(FadeOutMenu());
    }

    IEnumerator PreencherCampo(TMP_InputField inputText)
    {
        TextMeshProUGUI placeholder = inputText.placeholder as TextMeshProUGUI;

        if (placeholder != null)
        {
            var cor1 = placeholder.color;
            placeholder.color = Color.red;
            placeholder.text = "Preencha o seu nome";
            yield return new WaitForSeconds(3);
            placeholder.color = cor1;
            placeholder.text = "Insira seu nome aqui";
        }

        yield return null;
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
        MenuCam.gameObject.SetActive(false);
        menuAudioSource.enabled = false;
        gameAudioSource.enabled = true;

        movimentoFunc.MoverManual();
    }

    public void Tutorial()
    {
        if (canvasTutorial != null)
        {
            canvasMenu.gameObject.SetActive(false);
            canvasTutorial.gameObject.SetActive(true);

            

            if (Slides != null)
            {
                for (int i = 0; i < paiSlides.childCount; i++)
                    Slides.Add(paiSlides.GetChild(i));

                foreach (var slide in Slides)
                    slide.gameObject.SetActive(false);

                if (Slides.Count > 0)
                    Slides[0].gameObject.SetActive(true);
            }
        }
    }

    public void LeftArrow()
    {
        if (slideAtual == 0)
        {
            return;
        }
        if (slideAtual == 1)
        {
            leftArrow.image.color = new Color(1, 1, 1, 0.1f);
        }

        rightArrow.image.color = new Color(1, 1, 1, 1);

        Slides[slideAtual].gameObject.SetActive(false);

        slideAtual--;

        Slides[slideAtual].gameObject.SetActive(true);
    }

    public void RightArrow()
    {
        if (slideAtual == 4)
        { 
            return;
        }
        if (slideAtual == 3)
        {
            rightArrow.image.color = new Color(1, 1, 1, 0.1f);
        }

        leftArrow.image.color = new Color(1, 1, 1, 1);

        Slides[slideAtual].gameObject.SetActive(false);

        slideAtual++;

        Slides[slideAtual].gameObject.SetActive(true);
    }

    public void Voltar()
    {
        canvasMenu.gameObject.SetActive(true);
        canvasTutorial.gameObject.SetActive(false);

        Slides.Clear();
    }

    public void Sair()
    {
        Application.Quit();
    }
}