using System;
using System.Linq;
using System.Windows.Forms;

namespace ToCteView
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var t = Clipboard.GetText(TextDataFormat.UnicodeText);
            var rows = t.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var split = new Func<string, string[]>(s => s.Split(new[] { '\t' }, StringSplitOptions.None));
            var names = split(rows.First());
            var rest = rows.Skip(1);
            var ixs = names.Select((i, ix) => new { i, ix }).Where(i => !string.IsNullOrWhiteSpace(i.i)).Select(i => i.ix);
            var take = new Func<string[], string[]>(s => s.Select((i, ix) => new { i, ix }).Where(i => ixs.Contains(i.ix)).Select(i => i.i).ToArray());
            var takeNames = take(names).Select((i, ix) => new { i, ix }).ToDictionary(i => i.ix, i => i.i);

            Clipboard.SetText(
                string.Concat("WITH mycte AS ( SELECT ",
                string.Join(string.Concat(Environment.NewLine, " UNION ALL SELECT "),
                rest
                .Select(split)
                .Select(take)
                .Select((r, rix) =>
                    string.Join(", ",
                    r.Select((c, cix) =>
                        string.Concat(c == "NULL" ? c : string.Concat("N'", c.Replace("'", "''"), "'"), " AS [", takeNames[cix], "]"))))),
                        ") SELECT * FROM mycte"));
        }
    }
}
