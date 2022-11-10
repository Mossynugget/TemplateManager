using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateManagerModels.Helpers;

public static class ListExtensions
{
  public static bool None<TSource>(this IEnumerable<TSource> source,
                                     Func<TSource, bool> predicate)
  {
    return !source.Any(predicate);
  }
}
