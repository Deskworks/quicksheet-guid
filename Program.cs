using System;
using System.Collections.Generic;
using System.Text.Json;

namespace QuickSheetGuid;

class Program
{
    static void Main()
    {
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                using var doc = JsonDocument.Parse(line);
                var root = doc.RootElement;
                string type = root.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "";

                if (type == "init")
                {
                    var resp = new { type = "register", name = "quicksheet-guid", version = "1.0.0", prefix = "guid" };
                    Console.WriteLine(JsonSerializer.Serialize(resp));
                    Console.Out.Flush();
                }
                else if (type == "activate")
                {
                    string id = root.TryGetProperty("id", out var idEl) ? idEl.GetString() ?? "" : "";
                    string param = "";
                    if (root.TryGetProperty("params", out var paramsEl) && paramsEl.ValueKind == JsonValueKind.Array)
                    {
                        var arr = paramsEl.EnumerateArray();
                        if (arr.MoveNext()) param = arr.Current.GetString() ?? "";
                    }

                    var cells = Generate(param.Trim());
                    var response = new { type = "write", id, cells };
                    Console.WriteLine(JsonSerializer.Serialize(response));
                    Console.Out.Flush();
                }
            }
            catch { }
        }
    }

    static List<object> Generate(string param)
    {
        var cells = new List<object>();

        // Parse count (default 1, max 20)
        int count = 1;
        if (!string.IsNullOrEmpty(param))
        {
            // Check for format flags
            string format = "D"; // default: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
            string remaining = param;

            if (param.StartsWith("n ", StringComparison.OrdinalIgnoreCase) ||
                param.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                format = "N"; // 32 hex digits, no dashes
                remaining = param.Length > 2 ? param[2..].Trim() : "";
            }
            else if (param.StartsWith("b ", StringComparison.OrdinalIgnoreCase) ||
                     param.Equals("b", StringComparison.OrdinalIgnoreCase))
            {
                format = "B"; // {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
                remaining = param.Length > 2 ? param[2..].Trim() : "";
            }
            else if (param.StartsWith("upper ", StringComparison.OrdinalIgnoreCase) ||
                     param.Equals("upper", StringComparison.OrdinalIgnoreCase))
            {
                format = "D-UPPER";
                remaining = param.Length > 6 ? param[6..].Trim() : "";
            }

            if (!string.IsNullOrEmpty(remaining) && int.TryParse(remaining, out int n))
                count = Math.Clamp(n, 1, 20);

            for (int i = 0; i < count; i++)
            {
                string guid = Guid.NewGuid().ToString(format == "D-UPPER" ? "D" : format);
                if (format == "D-UPPER") guid = guid.ToUpperInvariant();
                cells.Add(new { r = i, c = 1, v = guid });
            }
        }
        else
        {
            cells.Add(new { r = 0, c = 1, v = Guid.NewGuid().ToString("D") });
        }

        return cells;
    }
}
