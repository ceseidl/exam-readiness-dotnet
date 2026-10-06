namespace ExamReadiness;

// Official exam outline: domains and their weight (sums to 100).
public record Domain(string Name, double Weight);

public record Exam(
    string Name, double PassingScore, List<Domain> Domains);

// Answers of one practice test in one domain.
public record DomainResult(string Domain, int Correct, int Total)
{
    public double Pct => Total == 0 ? 0 : 100.0 * Correct / Total;
}

public record Attempt(DateOnly Date, List<DomainResult> Results);

// Rules of thumb of this tool (adjust to your exam).
public record Policy(
    int Window = 3,        // last N practice tests
    double Margin = 10,    // points above the passing score
    double MinDomain = 60) // no domain below this, in %
{
    public double Target(Exam exam) => exam.PassingScore + Margin;
}
