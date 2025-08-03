using System;
using UnityEngine;

public class RaycastInteractor : MonoBehaviour
{

    public Camera playerCamera;

    public GameObject Interagir;

    public Action action;

    public UiAnim cIA;

    public int actualEmplo;

    Outline outline;

    float range = 100f;

    bool aberto;

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
                action = ShowId;
            }
            else if (hit.collider.CompareTag("Tablet"))
            {
                Enable(hit);
                action = ShowTablet;
            }
            else if (hit.collider.CompareTag("Aprovar"))
            {
                Aprovar(hit);
                // Lógica de aprovar aqui ou (melhor) chamar função da lógica
            }
            else if (hit.collider.CompareTag("Reprovar"))
            {
                Reprovar(hit);
                // Lógica de reprovar aqui ou (melhor) chamar função da lógica
            }
        }
        else if (aberto)
        {
            Reseting();
        }

    }

    void ShowId()
    {
        cIA.StartIdOpenAnim();
    }

    void ShowTablet()
    {
        cIA.StartTabletOpenAnim();
    }

    void Enable(RaycastHit hit)
    {
        aberto = true;
        outline = hit.collider.gameObject.GetComponent<Outline>();
        outline.OutlineColor = new Color(0f, 0f, 1f, 1f);
        Interagir.SetActive(true);
    }

    void Aprovar(RaycastHit hit)
    {
        aberto = true;
        outline = hit.collider.gameObject.GetComponent<Outline>();
        outline.OutlineColor = new Color(0f, 1f, 0f, 1f);
        Interagir.SetActive(true);
    }

    void Reprovar(RaycastHit hit)
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
