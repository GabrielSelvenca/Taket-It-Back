using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelsController : MonoBehaviour
{
    public NomesData nomesData;
    public FuncoesData funcoesData;
    public ConsumiveisData consumiveisData;
    public List<Funcionario> funcionarios;

    private HashSet<string> nomesUsados = new();
    private int proximoId = 1;

    void Start()
    {
        CarregarJsons();
    }

    public void GerarFuncionariosComConsumiveis(int quantidadeDeFuncionarios, int diaAtual, int porcentagemDeErro)
    {
        funcionarios.Clear();
        float chanceErro = porcentagemDeErro <= 100 ? porcentagemDeErro / 100 : 1.0f;

        for (int i = 0; i < quantidadeDeFuncionarios; i++)
        {
            Funcionario newFuncionario = new Funcionario
            {
                id = proximoId,
                nome = ObterNomeUnicoAleatorio(),
                funcao = ObterFuncaoAleatoria(),
                Credencial = "MTZs" + Random.Range(Random.Range(0, 100), Random.Range(101, 999)),
                itensPegosHoje = GerarConsumiveisAleatorios(diaAtual, chanceErro)
            };

            funcionarios.Add(newFuncionario);
        }

        Debug.Log($"qtd funcionarios: {funcionarios.Count}");
    }

    public string ObterNomeUnicoAleatorio()
    {
        if (nomesData == null || nomesData.nomes.Count == 0 || nomesData.sobrenomes.Count == 0)
            return "Desconhecido";

        int maxComb = nomesData.nomes.Count * nomesData.sobrenomes.Count;

        for (int t = 0; t < maxComb * 2; t++)
        {
            int i = Random.Range(0, nomesData.nomes.Count);
            int j = Random.Range(0, nomesData.sobrenomes.Count);

            string novoNome = $"{nomesData.nomes[i]} {nomesData.sobrenomes[j]}";

            if (!nomesUsados.Contains(novoNome))
            {
                nomesUsados.Add(novoNome);
                return novoNome;
            }
        }

        return "Fulano de tal";
    }

    public ListaConsumiveis GerarConsumiveisAleatorios(int tiposDeConsumiveis, float chanceErro = 0.1f)
    {
        ListaConsumiveis lista = new ListaConsumiveis();

        if (consumiveisData == null || consumiveisData.listaConsumiveis.Count == 0)
        {
            Debug.LogWarning("Consumíveis não carregados.");
            return lista;
        }

        List<Consumivel> cp = new List<Consumivel>(consumiveisData.listaConsumiveis);
        tiposDeConsumiveis = Mathf.Clamp(tiposDeConsumiveis, 1, cp.Count);

        for (int i = 0; i < tiposDeConsumiveis; i++)
        {
            if (cp.Count == 0) break;

            int index = Random.Range(0, cp.Count);
            Consumivel baseItem = cp[index];
            cp.RemoveAt(index);

            int qtdCorreta = Random.Range(1, baseItem.limitePorFuncionario + 1);

            int qtdEntregue = qtdCorreta;
            if (Random.Range(0.01f, 1.0f) < chanceErro)
                qtdEntregue = Mathf.Max(0, qtdCorreta > 1 ? qtdCorreta - 1 : qtdCorreta);

            Consumivel newItem = new Consumivel
            {
                id = baseItem.id,
                nome = baseItem.nome,
                qtd = qtdEntregue,
                limitePorFuncionario = baseItem.limitePorFuncionario
            };

            lista.listaConsumiveis.Add(newItem);
        }

        return lista;
    }

    public string ObterFuncaoAleatoria()
    {
        return funcoesData.nome[Random.Range(0, funcoesData.nome.Count)];
    }

    public void CarregarJsons()
    {
        TextAsset jsonNomes = Resources.Load<TextAsset>("nomes");
        TextAsset jsonFuncoes = Resources.Load<TextAsset>("funcoes");
        TextAsset jsonConsumiveis = Resources.Load<TextAsset>("consumiveis");
        if (jsonNomes != null && jsonFuncoes != null)
        {
            nomesData = JsonUtility.FromJson<NomesData>(jsonNomes.text);
            funcoesData = JsonUtility.FromJson<FuncoesData>(jsonFuncoes.text);
            consumiveisData = JsonUtility.FromJson<ConsumiveisData>(jsonConsumiveis.text);
        }
        else
        {
            Debug.LogError("Arquivo nomes.json ou funcoes.json não encontrado!!!");
        }
    }
}
