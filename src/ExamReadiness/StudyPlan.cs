namespace ExamReadiness;

public record Slice(string Domain, double Hours);

public static class StudyPlan
{
    // Splits the hours by "points still to win":
    // weight x (100 - hit rate). Heavy and weak domains
    // get more time than light ones.
    public static List<Slice> Allocate(
        Exam exam, Attempt last, double hours)
    {
        var need = exam.Domains
            .Select(d => (d.Name,
                Need: d.Weight * (100 - Pct(last, d.Name))))
            .ToList();
        var total = need.Sum(n => n.Need);

        return need
            .OrderByDescending(n => n.Need)
            .Select(n => new Slice(
                n.Name, Math.Round(hours * n.Need / total, 1)))
            .ToList();
    }

    static double Pct(Attempt a, string domain) =>
        a.Results.Single(r => r.Domain == domain).Pct;
}
