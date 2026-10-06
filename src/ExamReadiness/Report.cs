namespace ExamReadiness;

public static class Report
{
    static void Line(string s = "") => Console.WriteLine(s);

    public static void Print(
        Exam exam, List<Attempt> attempts, Policy policy,
        double hours)
    {
        var target = policy.Target(exam);
        var last = attempts[^1];
        var scores = attempts
            .Select(a => Scoring.Weighted(exam, a)).ToList();

        Line(exam.Name);
        Line($"Aprovação: {exam.PassingScore}, meta: {target}\n");
        Scores(attempts, scores);
        Gaps(exam, last, target);
        TrendLine(scores);
        Plan(exam, last, hours);
        Conclusion(Readiness.Assess(exam, attempts, policy));
    }

    static void Scores(List<Attempt> attempts, List<double> s)
    {
        Line("Simulados (nota ponderada):");
        for (var i = 0; i < attempts.Count; i++)
            Line($"  {attempts[i].Date:yyyy-MM-dd} {s[i],5:0.0}");
    }

    static void Gaps(Exam exam, Attempt last, double target)
    {
        Line("\nLacunas no último simulado:");
        var gaps = Scoring.Gaps(exam, last, target);
        if (gaps.Count == 0) Line("  nenhuma");
        foreach (var g in gaps)
        {
            var r = last.Results
                .Single(x => x.Domain == g.Domain);
            Line($"  {g.Domain,-11} {g.Pct,5:0.0}% " +
                 $"(±{Scoring.Error(r):0}) peso {g.Weight}% " +
                 $"perde {g.Lost:0.0} pts");
        }
    }

    static void TrendLine(List<double> scores)
    {
        var slope = Trend.Slope(scores.TakeLast(5).ToList());
        var name = Trend.Classify(slope) switch
        {
            Direction.Rising => "subindo",
            Direction.Falling => "caindo",
            _ => "estável"
        };
        Line($"\nTendência: {name} " +
             $"({slope:+0.0;-0.0} pts por simulado)");
    }

    static void Plan(Exam exam, Attempt last, double hours)
    {
        Line($"\nPlano de {hours:0}h até a prova:");
        foreach (var s in StudyPlan.Allocate(exam, last, hours))
            Line($"  {s.Domain,-11} {s.Hours,4:0.0}h");
    }

    static void Conclusion(Assessment a)
    {
        var label = a.Verdict switch
        {
            Verdict.Ready => "PRONTO",
            Verdict.Almost => "QUASE",
            Verdict.NotYet => "AINDA NÃO",
            _ => "SEM DADOS"
        };
        Line($"\nProntidão: {label}");
        foreach (var reason in a.Reasons)
            Line($"  - {reason}");
    }
}
