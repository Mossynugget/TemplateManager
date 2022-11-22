using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using $setting:solution$.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using Integration.MasterData.Models;

namespace $setting:namespace$;

public class $Action$$Domain$ : EndpointBaseSync
  .$if:withRequest$WithRequest<$Action$$Domain$Request>$endif:withRequest$$else:withRequest$_$_WithoutRequest$endelse:withRequest$
  .$if:withResult$_$_WithActionResult<$ReturnType$>$endif:withResult$$else:withResult$_$_WithoutResult$endelse:withResult$
{
    private readonly ILogger _logger;

    public $Action$$Domain$(ILogger<$Action$$Domain$> logger)
    {
        _logger = logger;
    }

    [Http$select:RequestType:Get|Post|Delete|Patch|Put$("$Action$")]
    [SwaggerOperation(
        Summary = "$Action$ $Domain:comment$",
        Description = "$Action$ $Domain:comment$",
        OperationId = "$Domain$.$Action$",
        Tags = new[] { "$Domain$Endpoint" })]
    public override async Task<ActionResult$if:withResult$<$ReturnType$>$endif:withResult$> HandleAsync($if:withRequest$[FromQuery] $Action$$Domain$Request request$endif:withRequest$, CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}$if:withResult$

public class $Action$$Domain$Request
{

}$endif:withResult$