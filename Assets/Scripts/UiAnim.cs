using Assets.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;

public class UiAnim : MonoBehaviour
{
    Coroutine anim;
    Coroutine anim2;
    int uiForX = 0;
    int rangeId = 80;
    int tabletForY = 0;
    int rangeTablet = 146;
    public RectTransform uiRectTransform;
    public RectTransform tabletRectTransform;
    public GameObject tabletModel;

    public TextMeshProUGUI crachaId;
    public TextMeshProUGUI crachaNome;
    public TextMeshProUGUI crachaFuncao;

    //ID ANIM START

    public void StartIdOpenAnim(int idFunc)
    {
        var func = GameData.Instance.GetFuncionario(idFunc);

        if (func != null)
        {
            crachaFuncao.text = func.funcao;
            crachaId.text = func.id.ToString();
            crachaNome.text = func.nome;
        }

        if (anim != null) { anim = null; }

        anim = StartCoroutine(IdAnimOpening());
    }

    public void StartIdCloseAnim()
    {
        if (anim != null) { anim = null; }

        anim = StartCoroutine(IdAnimClosing());
    }

    //Tablet ANIM START

    public void StartTabletOpenAnim()
    {
        if (anim2 != null) { anim2 = null; }

        anim2 = StartCoroutine(TabletAnimOpening());
    }

    public void StartTabletCloseAnim()
    {
        if (anim2 != null) { anim2 = null; }

        anim2 = StartCoroutine(TabletAnimClosing());
    }

    IEnumerator IdAnimOpening()
    {
        
        for (int i = uiForX; i < rangeId; i++)
        {
            uiRectTransform.position = uiRectTransform.position + new Vector3 (-4f, 0, 0);
            uiForX++;
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 40; i++)
        {
            uiRectTransform.position = uiRectTransform.position + new Vector3(1f, 0, 0);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 40; i++)
        {
            uiRectTransform.position = uiRectTransform.position + new Vector3(-1f, 0, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator IdAnimClosing()
    {
        
        for (int i = uiForX; i > 0; i--)
        {
            uiRectTransform.position = uiRectTransform.position + new Vector3(4f, 0, 0);
            uiForX--;
            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator TabletAnimOpening()
    {
        tabletModel.SetActive(false);
        for (int i = tabletForY; i < rangeTablet; i++)
        {
            tabletRectTransform.position = tabletRectTransform.position + new Vector3(0, 4f, 0);
            tabletForY++;
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 40; i++)
        {
            tabletRectTransform.position = tabletRectTransform.position + new Vector3(0, -1f, 0);
            yield return new WaitForSeconds(0.001f);
        }
        for (int i = 0; i < 40; i++)
        {
            tabletRectTransform.position = tabletRectTransform.position + new Vector3(0, 1f, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator TabletAnimClosing()
    {
        tabletModel.SetActive(true);
        for (int i = tabletForY; i > 0; i--)
        {
            tabletRectTransform.position = tabletRectTransform.position + new Vector3(0, -4f, 0);
            tabletForY--;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
