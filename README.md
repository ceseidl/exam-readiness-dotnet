English | [Português](README.pt-BR.md)

# exam-readiness-dotnet

A small .NET 10 console tool to plan and measure certification exam preparation: it reads practice test results (JSON), computes a **weighted score by exam domain**, points out weak domains, shows the **trend** and gives a **readiness** recommendation.

It follows the article "Certificações Técnicas: Planejamento e Medição de Prontidão com C#" (companion to "Melhores práticas para certificações").

## What it does

- **Weighted score**: sum of (domain weight x hit rate), the way the exam outline weights each domain.
- **Gaps**: points of the final score each domain loses against an internal target (passing score + margin), largest first, with a rough 95% margin of error per domain.
- **Trend**: least squares slope of the last practice tests (points per test).
- **Study plan**: splits the available hours by `weight x (100 - hit rate)`.
- **Readiness**: `PRONTO` (ready), `QUASE` (almost), `AINDA NÃO` (not yet) or `SEM DADOS`, with the reasons.

Rules of thumb (class `Policy`, adjust to your exam): window of the last 3 practice tests, margin of 10 points above the passing score, no domain below 60%, scores not trending down. They are heuristics of this tool, not official criteria of any exam.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download). No NuGet packages.

## How to run

```bash
dotnet build
dotnet run --project src/ExamReadiness --no-build
```

Optional arguments: exam file, practice tests file, study hours:

```bash
dotnet run --project src/ExamReadiness --no-build -- \
  src/ExamReadiness/data/exame.json \
  src/ExamReadiness/data/simulados-pronto.json 10
```

The sample data is fictional. Output (in Portuguese):

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

## Input format

`exame.json`: exam name, passing score and domains whose weights sum to 100. `simulados.json`: list of practice tests, each with a date and, per domain, `correct` and `total`. Every domain must appear in every test.

## Structure

```
exam-readiness-dotnet.slnx
src/ExamReadiness/
  Models.cs      Exam, Domain, Attempt, Policy
  Loader.cs      JSON loading and validation
  Scoring.cs     weighted score, gaps, margin of error
  Trend.cs       slope and direction
  StudyPlan.cs   hours by domain
  Readiness.cs   readiness verdict
  Report.cs      console report
  Program.cs     entry point
  data/          sample exam and practice tests
```

## License

[MIT](LICENSE). Author: Carlos Eduardo Seidl.
