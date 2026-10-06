using System.Globalization;
using ExamReadiness;

CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

var dir = Path.Combine(AppContext.BaseDirectory, "data");
var examPath = args.ElementAtOrDefault(0)
    ?? Path.Combine(dir, "exame.json");
var testsPath = args.ElementAtOrDefault(1)
    ?? Path.Combine(dir, "simulados.json");
var hours = double.TryParse(args.ElementAtOrDefault(2), out var h)
    ? h : 20;

var exam = Loader.LoadExam(examPath);
var attempts = Loader.LoadAttempts(testsPath, exam);

Report.Print(exam, attempts, new Policy(), hours);
