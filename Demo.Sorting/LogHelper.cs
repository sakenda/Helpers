
namespace Demo.Sorting;

public static class LogHelper
{
    public static void WriteLog(string testName, Action content)
    {
        var colors = new List<(ConsoleColor Light, ConsoleColor Dark)>()
        {
            (ConsoleColor.Cyan, ConsoleColor.DarkCyan),
            (ConsoleColor.Green, ConsoleColor.DarkGreen),
            (ConsoleColor.Yellow, ConsoleColor.DarkYellow),
            (ConsoleColor.Magenta, ConsoleColor.DarkMagenta),
            (ConsoleColor.Blue, ConsoleColor.DarkBlue),
            (ConsoleColor.Red, ConsoleColor.DarkRed),
        };

        var random = new Random();
        var (light, dark) = colors[random.Next(colors.Count)];

        // ┌ ┐ ─ └ ┘ │ ├ ┤ ┬ ┴ ┼

        Console.ForegroundColor = dark;
        Console.WriteLine($"┌─┬{new string('─', 80)}┬─┐");
        Console.ForegroundColor = light;
        Console.WriteLine($"│ │ {testName}{new string(' ', 79 - testName.Count())}│ │");
        Console.WriteLine($"└─┴{new string('─', 80)}┴─┘");
        Console.ForegroundColor = ConsoleColor.Gray;
        content.Invoke();
    }
}
