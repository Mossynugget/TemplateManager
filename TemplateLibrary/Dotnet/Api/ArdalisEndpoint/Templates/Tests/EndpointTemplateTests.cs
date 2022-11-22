using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace $setting:namespace$;

[TestFixture]
public class $Action$$Domain$Tests
{
  $setting:solution$Context? _inMemory$setting:solution$Context;
  DbContextOptions? _dbInMemoryOptions;
  $Action:camelCase$$Domain$? _$Action:camelCase$$Domain$;
  List<$Domain$> _$Domain:camelCase$List;
  IMapper _mapper;

  [SetUp]
  public void Setup()
  {
    _mapper ??= new MapperConfiguration(mc => {
      mc.AddProfile(new $setting:solution$Mappings());
    }).CreateMapper();

    _dbInMemoryOptions = new DbContextOptionsBuilder<$setting:solution$Context>().UseInMemoryDatabase(databaseName: new Guid().ToString()).Options;
    _inMemory$setting:solution$Context = new $setting:solution$Context(_dbInMemoryOptions);
    _$Action:camelCase$$Domain$ = new $Action$$Domain$(_inMemory$setting:solution$Context, _mapper);
    this.setupTestData();
  }

  [TearDown]
  public void TearDown()
  {
    _inMemory$setting:solution$Context.Database.EnsureDeleted();
    _inMemory$setting:solution$Context = null;
    _$Action:camelCase$$Domain$ = null;
    _mapper = null;
    _dbInMemoryOptions = null;
    _$ReturnType:camelCase$ = null;

  }

  private async Task<ActionResult$if:withResult$<$ReturnType$>$endif:withResult$> InvokeArdalisEndpoint($if:withRequest$$Action$$Domain$Request request$endif:withRequest$)
  {
    var responseActionResult = await _$Action:camelCase$$Domain$.HandleAsync(request);
    var responseOkObject = responseActionResult.Result as OkObjectResult;

    return responseOkObject$if:withResult$.Value as $ReturnType$$endif: withResult$;
  }

  private void setupTestData()
  {
    // Setup test data here.    
    _$Domain:camelCase$List.AddRange();
    
    if(!_inMemory$setting:solution$Context.$Domain$.Any()) 
    {
        _inMemory$setting:solution$Context.$Domain$.AddRange(_$Domain:camelCase$List);
        _inMemory$setting:solution$Context.SaveChanges();
    }
  }
}