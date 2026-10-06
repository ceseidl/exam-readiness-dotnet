[English](README.md) | Português

# exam-readiness-dotnet

Ferramenta de console .NET 10 para planejar e medir a preparação para provas de certificação: lê resultados de simulados (JSON), calcula a **nota ponderada por domínio** da prova, aponta os domínios fracos, mostra a **tendência** e dá uma recomendação de **prontidão**.

Acompanha o artigo "Certificações Técnicas: Planejamento e Medição de Prontidão com C#" (complemento de "Melhores práticas para certificações").

## O que faz

- **Nota ponderada**: soma de (peso do domínio x taxa de acerto), como o guia da prova pesa cada domínio.
- **Lacunas**: pontos da nota final que cada domínio perde em relação a uma meta interna (nota de aprovação + margem), do maior para o menor, com margem de erro aproximada de 95% por domínio.
- **Tendência**: inclinação por mínimos quadrados dos últimos simulados (pontos por simulado).
- **Plano de estudo**: divide as horas disponíveis por `peso x (100 - taxa de acerto)`.
- **Prontidão**: `PRONTO`, `QUASE`, `AINDA NÃO` ou `SEM DADOS`, com os motivos.

Regras práticas (classe `Policy`, ajuste para a sua prova): janela dos 3 últimos simulados, margem de 10 pontos acima da nota de aprovação, nenhum domínio abaixo de 60%, notas sem queda. São heurísticas desta ferramenta, não critérios oficiais de nenhuma prova.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download). Sem pacotes NuGet.

## Como rodar

```bash
dotnet build
dotnet run --project src/ExamReadiness --no-build
```

Argumentos opcionais: arquivo da prova, arquivo dos simulados, horas de estudo:

```bash
dotnet run --project src/ExamReadiness --no-build -- \
  src/ExamReadiness/data/exame.json \
  src/ExamReadiness/data/simulados-pronto.json 10
```

Os dados de exemplo são fictícios. Saída esperada:

```
Exame Exemplo de Nuvem (fictício)
Aprovação: 70, meta: 80

Simulados (nota ponderada):
  2026-08-03  47,5
  2026-08-17  57,5
  2026-08-31  65,0
  2026-09-14  75,0
  2026-09-28  77,5

Lacunas no último simulado:
  Segurança    75,0% (±25) peso 30% perde 1,5 pts
  Custos       75,0% (±30) peso 20% perde 1,0 pts

Tendência: subindo (+7,8 pts por simulado)

Plano de 20h até a prova:
  Segurança    6,7h
  Conceitos    4,4h
  Computação   4,4h
  Custos       4,4h

Prontidão: QUASE
  - 3 de 3 abaixo da meta (80).
```

## Formato de entrada

`exame.json`: nome da prova, nota de aprovação e domínios com pesos que somam 100. `simulados.json`: lista de simulados, cada um com data e, por domínio, `correct` (acertos) e `total`. Todo domínio precisa aparecer em todo simulado.

## Estrutura

```
exam-readiness-dotnet.slnx
src/ExamReadiness/
  Models.cs      Exam, Domain, Attempt, Policy
  Loader.cs      leitura e validação do JSON
  Scoring.cs     nota ponderada, lacunas, margem de erro
  Trend.cs       inclinação e direção
  StudyPlan.cs   horas por domínio
  Readiness.cs   veredito de prontidão
  Report.cs      relatório no console
  Program.cs     ponto de entrada
  data/          prova e simulados de exemplo
```

## Licença

[MIT](LICENSE). Autor: Carlos Eduardo Seidl.
