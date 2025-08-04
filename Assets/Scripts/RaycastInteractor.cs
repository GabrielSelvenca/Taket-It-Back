using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject Interagir;
    public Action action;
    public UiAnim cIA;
    public int actualEmplo;
    public MovimentoFunc movimentoFunc;

    Outline outline;
    float range = 100f;
    bool aberto;

    private GameObject funcionarioAtual = null;
    private GameObject Tablet;

    public AudioSource audioSource;
    public List<AudioClip> buttonSounds = new();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        actualEmplo = 1;
    }

    void Update()
    {
        Selector();

        if (Input.GetKeyDown(KeyCode.E) && action != null)
        {
            action.Invoke();
        }
    }

    void Selector()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.CompareTag("Empregado"))
            {
                Enable(hit);
                funcionarioAtual = hit.collider.gameObject;
                action = ShowId;
            }
            else if (hit.collider.CompareTag("Tablet"))
            {
                Enable(hit);
                Tablet = hit.collider.gameObject;
                action = ShowTablet;
            }
            else if (hit.collider.CompareTag("Aprovar"))
            {
                EnableA(hit);
                action = Aprovado;
            }
            else if (hit.collider.CompareTag("Reprovar"))
            {
                EnableR(hit);
                action = Reprovado;
            }
            else if (hit.collider.CompareTag("Item"))
            {
                Enable(hit);
                action = null;
            }
        }
        else if (aberto)
        {
            Reseting();
        }
    }

    void ShowId()
    {
        if (funcionarioAtual != null)
        {
            funcionarioAtual.GetComponent<BoxCollider>().enabled = false;

            if (int.TryParse(funcionarioAtual.name.Trim(), out int id))
            {
                GameData.Instance.BuscarModelosConsumiveisFuncionario(id);
                GameData.Instance.MostrarConsumiveisNaMesa(id);
                cIA.StartIdOpenAnim(id);
            }
            else
            {
                Debug.LogError("ID do funcionário inválido.");
            }
        }
    }

    public void CloseId()
    {
        if (funcionarioAtual != null)
        {
            cIA.StartIdCloseAnim();
            funcionarioAtual.GetComponent<BoxCollider>().enabled = true;
        }
    }

    void ShowTablet()
    {
        if (Tablet != null)
        {
            cIA.StartTabletOpenAnim();
            Tablet.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void HideTablet()
    {
        if (Tablet != null)
        {
            cIA.StartTabletCloseAnim();
            Tablet.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void Aprovado()
    {
        movimentoFunc.MoverManual();
        audioSource.Play();
        Debug.Log("Aprovado");

        GameData.Instance.AvaliarAcao(true, int.Parse(funcionarioAtual.name.Trim()));
    }

    private void Reprovado()
    {
        movimentoFunc.MoverManual();
        audioSource.Play();
        Debug.Log("Reprovado");

        GameData.Instance.AvaliarAcao(false, int.Parse(funcionarioAtual.name.Trim()));
    }

    void Enable(RaycastHit hit)
    {
        aberto = true;
        outline = hit.collider.gameObject.GetComponent<Outline>();
        outline.OutlineColor = new Color(0f, 0f, 1f, 1f);
        Interagir.SetActive(true);
    }

    void EnableA(RaycastHit hit)
    {
        aberto = true;
        outline = hit.collider.gameObject.GetComponent<Outline>();
        outline.OutlineColor = new Color(0f, 1f, 0f, 1f);
        Interagir.SetActive(true);
    }

    void EnableR(RaycastHit hit)
    {
        aberto = true;
        outline = hit.collider.gameObject.GetComponent<Outline>();
        outline.OutlineColor = new Color(1f, 0f, 0f, 1f);
        Interagir.SetActive(true);
    }

    void Reseting()
    {
        aberto = false;
        if (outline != null)
        {
            outline.OutlineColor = new Color(0f, 1f, 0f, 0f);
        }
        Interagir.SetActive(false);
        action = null;
        outline = null;
    }
}