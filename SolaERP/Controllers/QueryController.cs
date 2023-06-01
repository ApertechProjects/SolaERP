using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Common;
using System.Data;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IQueryBuilder _queryBuilder;

        public QueryController(IQueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        [HttpGet("{elementName}")]
        public async Task<IActionResult> GetScript(string elementName)
        {
            return Ok(await _queryBuilder.GenerateCLRQueryAsync(elementName));
        }

        [HttpGet("[action]")]
        public IActionResult GenerateClass(string className, string elementName, CommandType type)
        {
            return Ok(_queryBuilder.GenerateClassFieldsFromSqlElement(className, elementName, type));
        }
    }
}
