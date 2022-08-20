namespace $setting:namespace$
{
  $if:UseList$using System.Collections.Generic;
  $endif:UseList$using System;
  using MediatR;

  /// <summary>
  /// Request to $EventName$
  /// </summary>
  public partial class $EventName$ : IRequest<$ReturnedObject$>
  {
  }
}
