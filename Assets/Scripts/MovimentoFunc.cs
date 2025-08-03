using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MovimentoFunc : MonoBehaviour
{
    public Transform paiDaFila;
    public Transform pontoFrente;
    public Transform pontoFinal;
    public float velocidade = 6f;

    [Header("Sound")]
    public AudioSource blabbingAudioSource;
    public List<AudioClip> blabs = new List<AudioClip>();
    private bool tocando = false;

    private List<Transform> fila = new();
    private int indiceAtual = 0;
    private bool movendo = false;
    private Transform retanguloAtual = null;

    public GameData gameData;

    private enum EstadoMovimento { Parado, IndoParaFrente, IndoParaFinal }
    private EstadoMovimento estado = EstadoMovimento.Parado;

    void Start()
    {
        fila.Clear();
        for (int i = 0; i < paiDaFila.childCount; i++)
        {
            fila.Add(paiDaFila.GetChild(i));
        }

        fila = fila.OrderBy(x => Random.value).ToList();
    }

    public void MoverManual()
    {
        if (movendo) return;

        if (estado == EstadoMovimento.Parado)
        {
            movendo = true;

            if (fila.Count == 0) return;
            
            retanguloAtual = fila[indiceAtual];
            fila.RemoveAt(indiceAtual);

            StartCoroutine(MoverPara(retanguloAtual, pontoFrente.position));
            estado = EstadoMovimento.IndoParaFrente;

            movendo = false;

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
                
                MoverManual();
            }));

            movendo = true;
        }

        if(movendo == false && estado == EstadoMovimento.IndoParaFrente)
        {
            Blabbs();
        }
    }

    private IEnumerator MoverPara(Transform obj, Vector3 destino, System.Action aoFinalizar = null)
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

        if(aoFinalizar != null)
            aoFinalizar?.Invoke();
    }

    private void AndarFila()
    {
        for (int i = 0; i < fila.Count; i++)
        {
            Vector3 pos = fila[i].position;
            fila[i].position = new Vector3(pos.x, pos.y, pos.z);
        }
    }

    public void Blabbs()
    {
        if (tocando || blabs.Count == 0)
            return;

        int index = Random.Range(0, blabs.Count);
        AudioClip escolhido = blabs[index];

        Debug.Log($"Tocando: {escolhido.name} - duração: {escolhido.length}");

        blabbingAudioSource.clip = escolhido;
        StartCoroutine(EsperarChegar());

    }

    private IEnumerator EsperarChegar()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("Chegou");
        blabbingAudioSource.Play(); 
    }

    //public void DroparConsumiveis(int idFuncionario)
    //{
    //    var modelos = gameData.BuscarModelosConsumiveisFuncionario(idFuncionario);
    //    if (modelos == null || modelos.Count == 0)
    //    {
    //        Debug.LogWarning("Nenhum consumível encontrado para este funcionário.");
    //        return;
    //    }

    //    float offset = 0f;
    //    float distanciaEntreItens = 1.0f;

    //    foreach (var modelo in modelos)
    //    {
    //        for (int i = 0; i < modelo.quantidade; i++)
    //        {
    //            // Pega um modelo aleatório do grupo
    //            GameObject prefab = modelo.modelos[UnityEngine.Random.Range(0, modelo.modelos.Count)];
    //            GameObject instancia = Instantiate(prefab);

    //            instancia.transform.position = gameData.DropItemsArea.position + new Vector3(offset, 0, 0);
    //            offset += distanciaEntreItens;

    //            instancia.name = $"{modelo.nomeConsumivel}_{i + 1}";
    //        }
    //    }
    //}
}