namespace ExamReadiness;

public enum Verdict { NoData, NotYet, Almost, Ready }

public record Assessment(Verdict Verdict, List<string> Reasons);

public static class Readiness
{
    public static Assessment Assess(
        Exam exam, List<Attempt> attempts, Policy p)
    {
        if (attempts.Count < p.Window)
            return new(Verdict.NoData,
                [$"Faltam simulados: mínimo {p.Window}."]);

        var recent = attempts.TakeLast(p.Window).ToList();
        var scores = recent
            .Select(a => Scoring.Weighted(exam, a)).ToList();
        var target = p.Target(exam);
        var reasons = new List<string>();

        var below = scores.Count(s => s < target);
        if (below > 0)
            reasons.Add($"{below} de {p.Window} abaixo da " +
                        $"meta ({target:0.#}).");

        foreach (var r in recent[^1].Results
                     .Where(r => r.Pct < p.MinDomain))
            reasons.Add($"{r.Domain} abaixo de {p.MinDomain}%.");

        var slope = Trend.Slope(scores);
        if (Trend.Classify(slope) == Direction.Falling)
            reasons.Add("Notas em queda.");

        if (reasons.Count == 0)
            return new(Verdict.Ready, ["Critérios atendidos."]);

        var enough = scores.Average() >= exam.PassingScore;
        return new(enough ? Verdict.Almost : Verdict.NotYet,
            reasons);
    }
}
