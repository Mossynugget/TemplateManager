using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TemplateManager.Models.Helpers;

public static class ListExtensions
{
  public static bool None<TSource>(this IEnumerable<TSource> source,
                                     Func<TSource, bool>? predicate = null)
  {
    if (predicate == null)
      return !source.Any();

    return !source.Any(predicate);
  }

  public static bool ContainsAny(List<string?> haystacks, params string[] needles)
  {
    foreach (string haystack in haystacks.Where(x => string.IsNullOrEmpty(x) == false))
    {
      foreach (string needle in needles)
      {
        if (haystack.Contains(needle))
          return true;
      }
    }

    return false;
  }
}
