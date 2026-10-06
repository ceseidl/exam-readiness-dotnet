namespace ExamReadiness;

// How many points of the final score a domain is losing.
public record Gap(
    string Domain, double Weight, double Pct, double Lost);

public static class Scoring
{
    static double PctOf(Attempt a, string domain) =>
        a.Results.Single(r => r.Domain == domain).Pct;

    // Final score: sum of (weight x domain hit rate), 0..100.
    public static double Weighted(Exam exam, Attempt a) =>
        exam.Domains.Sum(d => d.Weight * PctOf(a, d.Name) / 100);

    // Points lost against the target, largest first.
    public static List<Gap> Gaps(
        Exam exam, Attempt a, double target) =>
        exam.Domains
            .Select(d =>
            {
                var pct = PctOf(a, d.Name);
                var lost = Math.Max(0, target - pct)
                    * d.Weight / 100;
                return new Gap(d.Name, d.Weight, pct, lost);
            })
            .Where(g => g.Lost > 0)
            .OrderByDescending(g => g.Lost)
            .ToList();

    // Rough 95% margin for a domain hit rate (normal approx.).
    public static double Error(DomainResult r)
    {
        var p = r.Pct / 100;
        return 196 * Math.Sqrt(p * (1 - p) / r.Total);
    }
}
