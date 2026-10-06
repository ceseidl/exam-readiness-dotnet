using System.Text.Json;

namespace ExamReadiness;

public static class Loader
{
    static readonly JsonSerializerOptions Json =
        new(JsonSerializerDefaults.Web);

    public static Exam LoadExam(string path)
    {
        var exam = JsonSerializer.Deserialize<Exam>(
            File.ReadAllText(path), Json)
            ?? throw new InvalidDataException("Prova vazia.");

        var sum = exam.Domains.Sum(d => d.Weight);
        if (Math.Abs(sum - 100) > 0.001)
            throw new InvalidDataException(
                $"Os pesos devem somar 100 (soma: {sum}).");
        return exam;
    }

    public static List<Attempt> LoadAttempts(
        string path, Exam exam)
    {
        var list = JsonSerializer.Deserialize<List<Attempt>>(
            File.ReadAllText(path), Json) ?? [];

        foreach (var a in list)
        foreach (var d in exam.Domains)
        {
            var r = a.Results
                .SingleOrDefault(x => x.Domain == d.Name);
            if (r is null || r.Total <= 0 || r.Correct > r.Total)
                throw new InvalidDataException(
                    $"{a.Date}: resultado inválido ({d.Name}).");
        }
        return list.OrderBy(a => a.Date).ToList();
    }
}
