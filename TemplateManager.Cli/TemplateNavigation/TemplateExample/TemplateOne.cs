namespace $setting:namespace$
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  internal class TemplateOne
  {$if:includeString$Hello there, this is string$test$
    $test$$endif:includeString$
    $Test2$
  }$if:includeClosingBrace$
}
$endif:includeClosingBrace$