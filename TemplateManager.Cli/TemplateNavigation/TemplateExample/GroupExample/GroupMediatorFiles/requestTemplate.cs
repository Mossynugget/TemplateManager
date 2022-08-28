namespace $setting:namespace$
{
  $if:UseList$using System.Collections.Generic;
  $endif:UseList$using System;
  using MediatR;

  /// <summary>
  /// Request to $ContractName:Comment$
  /// </summary>
  public partial class $ContractName$ : IRequest<$ReturnedObject$>
  {
  }
}
