using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.GridLayout;
using SolaERP.Controllers;

namespace SolaERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GridLayoutController : CustomBaseController
    {
        private readonly IGridLayoutService _gridLayoutService;

        public GridLayoutController(IGridLayoutService gridLayoutService)
        {
            _gridLayoutService = gridLayoutService;
        }

        [HttpGet("{userId}/{layoutName}")]
        public async Task<IActionResult> Get(int userId, string layoutName)
            => CreateActionResult(await _gridLayoutService.GetAsync(userId, layoutName));

        [HttpPost]
        public async Task<IActionResult> Add(GridLayoutDto gridLayoutDto)
        {
            await _gridLayoutService.AddAsync(gridLayoutDto);
            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(GridLayoutDto gridLayoutDto)
        {
            await _gridLayoutService.UpdateAsync(gridLayoutDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _gridLayoutService.RemoveAsync(id);
            return Ok();
        }

    }
}
