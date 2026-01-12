using BudgetZen.Api.Extensions;
using BudgetZen.Application.Dtos;
using BudgetZen.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetZen.Api.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/budgets")]
public class BudgetsController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetsController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BudgetDto>>> GetAll(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var budgets = await _budgetService.GetAllAsync(userId, cancellationToken);
        return Ok(budgets);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BudgetDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var budget = await _budgetService.GetByIdAsync(userId, id, cancellationToken);
        return budget is null ? NotFound() : Ok(budget);
    }

    [HttpPost]
    public async Task<ActionResult<BudgetDto>> Create(CreateBudgetRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var budget = await _budgetService.CreateAsync(userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = budget.Id, version = "1.0" }, budget);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BudgetDto>> Update(Guid id, UpdateBudgetRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var budget = await _budgetService.UpdateAsync(userId, id, request, cancellationToken);
        return budget is null ? NotFound() : Ok(budget);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var deleted = await _budgetService.DeleteAsync(userId, id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
