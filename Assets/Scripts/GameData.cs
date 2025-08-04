using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameData : MonoBehaviour
    {
        public static GameData Instance { get; private set; }

        [Header("JSONs de Dados")]
        public ListaFuncionarios funcionarios;
        public ListaConsumiveis consumiveis;
        public ListaFuncionariosConsumiveis funcionariosConsumiveis;
        public ListaRanking ranking;

        [Header("Referência para busca de personagens")]
        public GameObject listaPersonagens;
        public GameObject listaConsumiveisModels;

        private Dictionary<int, Funcionarios> funcionarioPorId = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else { Destroy(gameObject); return; }

            CarregarDados();
            GerarConsumiveis();
            IniciarVinculos();
        }

        private void CarregarDados()
        {
            string caminhoR = Path.Combine(Application.persistentDataPath, "ranking.json");
            if (File.Exists(caminhoR))
            {
                string json = File.ReadAllText(caminhoR);
                ranking = JsonUtility.FromJson<ListaRanking>(json);
                Debug.Log("Ranking carregado com sucesso.");
            }
            else
            {
                ranking = new ListaRanking();
                Debug.Log("Arquivo de ranking não encontrado. Criando novo.");
            }

            funcionarios = new ListaFuncionarios
            {
                funcionarios = new List<Funcionarios>
                {
                    new Funcionarios { id = 14254, nome = "Carla Menezes", funcao = "TI" },
                    new Funcionarios { id = 9572, nome = "Ricardo Torres", funcao = "TI" },
                    new Funcionarios { id = 31774, nome = "Duda Vasconcelos", funcao = "Áudio" },
                    new Funcionarios { id = 18995, nome = "Nara Kim", funcao = "Dev" },
                    new Funcionarios { id = 15537, nome = "Bruno Alves", funcao = "Dev" },
                    new Funcionarios { id = 44026, nome = "Thiago Duarte", funcao = "Marketing" },
                    new Funcionarios { id = 32137, nome = "Camila Bernardes", funcao = "Marketing" },
                }
            };

            consumiveis = new ListaConsumiveis
            {
                consumiveis = new List<Consumivel>
                {
                    new Consumivel { id = 1, nome = "Pilha", offSet = -1f },
                    new Consumivel { id = 2, nome = "Caneta", offSet = -1f },
                    new Consumivel { id = 3, nome = "Lápis", offSet = -1f },
                    new Consumivel { id = 4, nome = "Mouse", offSet = -3f },
                    new Consumivel { id = 5, nome = "Teclado", offSet = -5f },
                    new Consumivel { id = 6, nome = "Cálculadora", offSet = -3.5f }
                }
            };
        }

        public void GerarConsumiveis()
        {
            funcionariosConsumiveis = new ListaFuncionariosConsumiveis { funcionarioConsumiveis = new List<FuncionarioConsumivel>() };

            foreach (var func in funcionarios.funcionarios)
            {
                int qtdTipos = UnityEngine.Random.Range(3, UnityEngine.Random.Range(5, 8));
                List<int> usados = new();

                for (int i = 0; i < qtdTipos; i++)
                {
                    Consumivel item;
                    do
                    {
                        item = consumiveis.consumiveis[UnityEngine.Random.Range(0, consumiveis.consumiveis.Count)];
                    } while (usados.Contains(item.id));

                    usados.Add(item.id);
                    int qtd = UnityEngine.Random.Range(2, 5);

                    funcionariosConsumiveis.funcionarioConsumiveis.Add(new FuncionarioConsumivel
                    {
                        idFuncionario = func.id,
                        idConsumivel = item.id,
                        qtd = qtd
                    });
                }
            }
        }

        private void IniciarVinculos()
        {
            funcionarioPorId.Clear();

            foreach (var func in funcionarios.funcionarios)
            {
                funcionarioPorId[func.id] = func;

                Transform child = listaPersonagens.transform.Find(func.id.ToString());
                if (child != null)
                {
                    func.personagem3D = child.gameObject;
                    Debug.Log($"Funcionario {func.nome} vinculado ao objeto {child.name}");
                }
            }
        }

        public Funcionarios GetFuncionario(int id) => funcionarioPorId.TryGetValue(id, out var func) ? func : null;

        public List<FuncionarioConsumivel> GetConsFunc(int idFunc) => funcionariosConsumiveis.funcionarioConsumiveis.FindAll(f => f.idFuncionario == idFunc);

        public List<Consumivel> BuscarModelosConsumiveisFuncionario(int idFuncionario)
        {
            var resultado = new List<Consumivel>();
            var consumiveisFuncionario = GetConsFunc(idFuncionario);

            foreach (var entrega in consumiveisFuncionario)
            {
                var cons = consumiveis.consumiveis.Find(c => c.id == entrega.idConsumivel);
                if (cons == null) continue;

                resultado.Add(new Consumivel
                {
                    id = cons.id,
                    nome = cons.nome,
                    offSet = cons.offSet,
                    qtd = entrega.qtd
                });
            }

            var funcionario = GetFuncionario(idFuncionario);
            if (funcionario != null)
            {
                funcionario.consumiveisPegos = resultado;
            }

            return resultado;
        }

        public GameObject BuscarModeloConsumivelPorNome(string nomeConsumivel)
        {
            Transform objetoConsumivel = listaConsumiveisModels.transform.Find($"{nomeConsumivel}_atual");
            if (objetoConsumivel == null) return null;

            return objetoConsumivel.childCount > 0 ?
                objetoConsumivel.GetChild(0).gameObject :
                objetoConsumivel.gameObject;
        }

        public void MostrarConsumiveisNaMesa(int idFuncionario)
        {
            var funcionario = GetFuncionario(idFuncionario);
            if (funcionario == null || funcionario.consumiveisPegos == null) return;

            funcionario.devolucaoCorreta = UnityEngine.Random.value > 0.49f;

            foreach (var cons in funcionario.consumiveisPegos)
            {
                int qtdParaMostrar = cons.qtd;

                if (!funcionario.devolucaoCorreta)
                {
                    qtdParaMostrar = Mathf.Max(0, cons.qtd - 1);
                }

                MostrarConsumivelNaMesa(cons, qtdParaMostrar);
            }
        }


        private void MostrarConsumivelNaMesa(Consumivel cons, int quantidade)
        {
            Transform objetoBase = listaConsumiveisModels.transform.Find($"{cons.nome}_atual");
            if (objetoBase == null)
            {
                Debug.LogWarning($"Modelo base '{cons.nome}_atual' não encontrado.");
                return;
            }

            Vector3 posBase = objetoBase.localPosition;

            for (int i = 0; i < quantidade; i++)
            {
                GameObject copia = Instantiate(objetoBase.gameObject, listaConsumiveisModels.transform);
                copia.SetActive(true);

                float deslocamentoTotal = cons.offSet * i;
                Vector3 novaPos = new Vector3(posBase.x + deslocamentoTotal, posBase.y, posBase.z);
                copia.transform.localPosition = novaPos;

                copia.name = (i < quantidade - 1) ? cons.nome : $"{cons.nome}_atual";
            }
        }

        public void LimparMesa()
        {
            foreach (Transform item in listaConsumiveisModels.transform)
            {
                if (item.gameObject.activeSelf)
                {
                    Destroy(item.gameObject);
                }
            }
        }

        public void AvaliarAcao(bool aprovou, int idFuncionario)
        {
            var funcionario = GetFuncionario(idFuncionario);
            if (funcionario == null)
            {
                Debug.LogWarning("Funcionário não encontrado.");
                return;
            }

            bool devolucaoEstaCorreta = funcionario.devolucaoCorreta;

            bool acaoCorreta = (aprovou && devolucaoEstaCorreta) || (!aprovou && !devolucaoEstaCorreta);

            if (acaoCorreta)
            {
                PointsManager.Instance.SomarPontos();
                Debug.Log($"✅ Jogador acertou. (Total: {PointsManager.Instance.totalPoints})");
            }
            else
            {
                PointsManager.Instance.DiminuirPontos();
                Debug.Log($"❌ Jogador errou. (Total: {PointsManager.Instance.totalPoints})");
            }

            LimparMesa();
        }

        public void InserirRanking(string nomeJogador, int pontosGanhos)
        {
            var jogador = ranking.ranking.Find(j => j.nome == nomeJogador);

            if (jogador != null)
            {
                jogador.pontos += pontosGanhos;
                Debug.Log($"Jogador {jogador.nome} atualizado com +{pontosGanhos} pontos (Total: {jogador.pontos})");
            }
            else
            {
                ranking.ranking.Add(new PlayerRank { nome = nomeJogador, pontos = pontosGanhos });
                Debug.Log($"Jogador {nomeJogador} adicionado com {pontosGanhos} pontos.");
            }

            string caminho = Path.Combine(Application.persistentDataPath, "ranking.json");
            File.WriteAllText(caminho, JsonUtility.ToJson(ranking, true));
            Debug.Log($"Ranking salvo em {caminho}");
        }
    }
}