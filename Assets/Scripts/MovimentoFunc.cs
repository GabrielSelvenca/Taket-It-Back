using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoFunc : MonoBehaviour
{
    public Transform paiDaFila;
    public Transform pontoFrente;
    public Transform pontoFinal;
    public float velocidade = 6f;

    private List<Transform> fila = new();
    private int indiceAtual = 0;
    private bool movendo = false;
    private Transform retanguloAtual = null;

    private enum EstadoMovimento { Parado, IndoParaFrente, IndoParaFinal }
    private EstadoMovimento estado = EstadoMovimento.Parado;

    void Start()
    {
        fila.Clear();
        for (int i = 0; i < paiDaFila.childCount; i++)
        {
            fila.Add(paiDaFila.GetChild(i));
        }
    }

    public void MoverManual()
    {
        if (movendo) return;

        if (estado == EstadoMovimento.Parado)
        {
            if (fila.Count == 0) return;

            retanguloAtual = fila[indiceAtual];
            fila.RemoveAt(indiceAtual);
            StartCoroutine(MoverPara(retanguloAtual, pontoFrente.position, () =>
            {
                estado = EstadoMovimento.IndoParaFrente;
            }));

            movendo = true;
        }
        else if (estado == EstadoMovimento.IndoParaFrente)
        {
            if (retanguloAtual == null) return;

            StartCoroutine(MoverPara(retanguloAtual, pontoFinal.position, () =>
            {
                retanguloAtual.gameObject.SetActive(false);
                AndarFila();
                if (indiceAtual >= fila.Count)
                    indiceAtual = 0;

                retanguloAtual = null;
                estado = EstadoMovimento.Parado;
            }));

            movendo = true;
        }
    }

    private IEnumerator MoverPara(Transform obj, Vector3 destino, System.Action aoFinalizar)
    {
        obj.gameObject.SetActive(true);
        while (Vector3.Distance(obj.position, destino) > 0.05f)
        {
            obj.position = Vector3.MoveTowards(obj.position, destino, velocidade * Time.deltaTime);
            yield return null;
        }

        obj.position = destino;
        movendo = false;
        GameData.Instance.BuscarModelosConsumiveisFuncionario(int.Parse(obj.gameObject.name));

        aoFinalizar?.Invoke();
    }

    private void AndarFila()
    {
        float distanciaZ = 1f;

        for (int i = 0; i < fila.Count; i++)
        {
            Vector3 pos = fila[i].position;
            fila[i].position = new Vector3(pos.x, pos.y, pos.z + distanciaZ);
        }
    }
}