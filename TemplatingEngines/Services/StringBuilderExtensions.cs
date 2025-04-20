using System.Text;
using System.Text.RegularExpressions;
using TemplatingEngines.Common;

namespace TemplatingEngines.Services;

public static class StringBuilderExtensions
{

    /// <summary>Append a copy of the specified string to this instance, left-padded to the specified total width</summary>
    /// <param name="sb">An instance of <see cref="System.Text.StringBuilder" /></param>
    /// <param name="value">The string to append</param>
    /// <param name="totalWidth">The total width to left-pad the value to before appending</param>
    /// <returns>A reference to the StringBuilder after the append operation has completed.</returns>
    public static StringBuilder AppendPadLeft(this StringBuilder sb, string value, int totalWidth)
    {
        return sb.Append(value.PadLeft(totalWidth));
    }

    /// <summary>Append a copy of the specified string to this instance, right-padded to the specified total width</summary>
    /// <param name="sb">An instance of <see cref="System.Text.StringBuilder" /></param>
    /// <param name="value">The string to append</param>
    /// <param name="totalWidth">The total width to right-pad the value to before appending</param>
    /// <returns>A reference to the StringBuilder after the append operation has completed.</returns>
    public static StringBuilder AppendPadRight(this StringBuilder sb, string value, int totalWidth)
    {
        return sb.Append(value.PadRight(totalWidth));
    }

    /// <summary>
    ///     Append a copy of the specified string to this instance, wrapped to the specified total width,
    ///     with the final line right-padded to the specified total width.
    /// </summary>
    /// <param name="sb">An instance of <see cref="System.Text.StringBuilder" /></param>
    /// <param name="value">The string to append</param>
    /// <param name="totalWidth">The total width to left-pad the value to before appending</param>
    /// <param name="indent">A string to be prepended to lines after the first line if the value requires wrapping</param>
    /// <returns>A reference to the StringBuilder after the append operation has completed.</returns>
    public static StringBuilder AppendWrapPadRight(this StringBuilder sb, string value, int totalWidth, string indent = "")
    {
        var tokens = Regex.Split(value, "\\b");
        var line = new StringBuilder();
        for (var i = 0; i < tokens.Length - 1; i++)
        {
            var token = tokens[i];
            if (String.IsNullOrEmpty(token)) continue;
            while (token.Length > totalWidth)
            {
                sb.AppendLine(token[..totalWidth]);
                token = token[totalWidth..];
            }

            line.Append(token);
            if (line.Length + tokens[i + 1].Length <= totalWidth) continue;
            sb.AppendLine(line.ToString().TrimEnd());
            line.Clear().Append(indent);
            if (String.IsNullOrWhiteSpace(tokens[i + 1])) i++;
        }

        var pad = totalWidth - line.Length;
        line.Append(String.Empty.PadRight(pad));
        sb.Append(line);
        return sb;
    }

    public static StringBuilder AppendPriceGbp(this StringBuilder sb, decimal price)
        => sb.AppendPadLeft($"GBP {price:0.00}", 12);

    public static StringBuilder AppendOrderItem(this StringBuilder sb, OrderItem item)
    {
        sb.AppendWrapPadRight(item.Name, 44, "  ");
        sb.Append(item.Quantity.ToString().PadLeft(4));
        sb.AppendPriceGbp(item.UnitPrice);
        sb.AppendPriceGbp(item.Total);
        return sb.AppendLine();
    }
}
