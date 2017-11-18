using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToSelectValues
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
                string.Concat("SELECT * FROM (VALUES", Environment.NewLine, "  (",
                    string.Join(string.Concat(")", Environment.NewLine, ", ("),
                        rest
                            .Select(split)
                            .Select(take)
                            .Select((r, rix) =>
                                string.Join(", ",
                                    r.Select(c => c == "NULL" ? c : string.Concat("N'", c.Replace("'", "''"), "'"))))), ") )", Environment.NewLine, "_(", string.Join(", ", takeNames.Select(n => string.Join("", "[", n.Value, "]"))), ")"));
        }
    }
}
