namespace ExamReadiness;

public enum Direction { Rising, Stable, Falling }

public static class Trend
{
    // Least squares slope: points per practice test.
    public static double Slope(IReadOnlyList<double> scores)
    {
        var n = scores.Count;
        if (n < 2) return 0;
        var mx = (n - 1) / 2.0;
        var my = scores.Average();
        var num = 0.0;
        var den = 0.0;
        for (var i = 0; i < n; i++)
        {
            num += (i - mx) * (scores[i] - my);
            den += (i - mx) * (i - mx);
        }
        return num / den;
    }

    public static Direction Classify(double slope) => slope switch
    {
        > 1 => Direction.Rising,
        < -1 => Direction.Falling,
        _ => Direction.Stable
    };
}
