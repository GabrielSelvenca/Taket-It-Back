using System;
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

            cIA.StartIdOpenAnim(int.Parse(funcionarioAtual.name));
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

        Debug.Log("Aprovado");

        // Aqui fica a lógica dos pontos e o que mais precisar
    }

    private void Reprovado()
    {
        movimentoFunc.MoverManual();

        Debug.Log("Reprovado");
        // Aqui fica a lógica dos pontos e o que mais precisar
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
        outline.OutlineColor = new Color(0f, 1f, 0f, 0f);
        Interagir.SetActive(false);
        action = null;
        outline = null;
    }
}
