namespace $setting:namespace$
{
  $if:UseList$using System.Collections.Generic;
  $endif:UseList$using System.Threading;
  using System.Threading.Tasks;
  using MediatR;
  using $setting:projectName$.$Domain$.Contracts.$RequestType$

  /// <summary>
  /// The $ContractName:comment$ handler.
  /// </summary>
  public class $ContractName$Handler : IRequestHandler<$ContractName$, $ReturnedObject$>
  {
    private readonly I$Domain$Repository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="$ContractName$Handler"/> class.
    /// </summary>
    /// <param name="I$Domain$Repository">The $Domain:comment$ repository.</param>
    public $ContractName$Handler(I$Domain$Repository repository)
    {
      _repository = repository;
    }

    /// <inheritdoc/>
    public async Task<$ReturnedObject$> Handle($ContractName$ request, CancellationToken cancellationToken)
    {
      return await _repository.$ContractName$().ConfigureAwait(false);
    }
  }
}
