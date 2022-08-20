namespace $setting:namespace$
{
  $if:UseList$using System.Collections.Generic;
  $endif:UseList$using System.Threading;
  using System.Threading.Tasks;
  using MediatR;
  using $setting:projectName$.$Domain$.Contracts.Queries

  /// <summary>
  /// The $EventName:comment$ handler.
  /// </summary>
  public class $EventName$Handler : IRequestHandler<$EventName$, $ReturnedObject$>
  {
    private readonly I$Domain$Repository repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="$EventName$Handler"/> class.
    /// </summary>
    /// <param name="I$Domain$Repository">The $Domain:comment$ repository.</param>
    public $EventName$Handler(I$Domain$Repository repository)
    {
      this.repository = repository;
    }

    /// <inheritdoc/>
    public async Task<$ReturnedObject$> Handle($EventName$ request, CancellationToken cancellationToken)
    {
      return await this.repository.$EventName$().ConfigureAwait(false);
    }
  }
}
