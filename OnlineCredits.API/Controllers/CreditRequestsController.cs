using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCredits.Core.Entities;
using OnlineCredits.Infrastructure.Data;
using System.Linq;
using System.Security.Claims;
using OnlineCredits.Core.DTOs.CreditRequest;
using Microsoft.EntityFrameworkCore;
using OnlineCredits.Application.Services;

namespace OnlineCredits.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public CreditRequestsController(AppDbContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        // POST: api/creditrequests
        [HttpPost]
        [Authorize(Roles = "Solicitante")]
        public async Task<ActionResult<CreditRequestResponseDto>> Create(CreateCreditRequestDto createDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
                return NotFound("Usuario no encontrado");

            var creditRequest = new CreditRequest
            {
                UserId = user.Id,
                Amount = createDto.Amount,
                TermInMonths = createDto.TermInMonths,
                MonthlyIncome = createDto.MonthlyIncome,
                WorkSeniority = createDto.WorkSeniority,
                EmploymentType = createDto.EmploymentType,
                CurrentDebt = createDto.CurrentDebt,
                Purpose = createDto.Purpose,
                Status = createDto.MonthlyIncome >= 1500 ? "Aprobado" : "Pendiente",
                CreatedAt = DateTime.UtcNow
            };

            _context.CreditRequests.Add(creditRequest);
            await _context.SaveChangesAsync();

            // Registrar la acción en los logs de auditoría
            await _auditService.LogActionAsync(
                "CREATE",
                "CreditRequest",
                creditRequest.Id,
                $"Solicitud de crédito creada por {user.Username}",
                userId,
                user.Username
            );

            return CreatedAtAction(nameof(GetById), new { id = creditRequest.Id }, new CreditRequestResponseDto
            {
                Id = creditRequest.Id,
                Amount = creditRequest.Amount,
                TermInMonths = creditRequest.TermInMonths,
                MonthlyIncome = creditRequest.MonthlyIncome,
                WorkSeniority = creditRequest.WorkSeniority,
                EmploymentType = creditRequest.EmploymentType,
                CurrentDebt = creditRequest.CurrentDebt,
                Purpose = creditRequest.Purpose,
                Status = creditRequest.Status,
                CreatedAt = creditRequest.CreatedAt,
                UpdatedAt = creditRequest.UpdatedAt,
                RejectionReason = creditRequest.RejectionReason,
                ApprovedAmount = creditRequest.ApprovedAmount,
                InterestRate = creditRequest.InterestRate,
                MonthlyPayment = creditRequest.MonthlyPayment,
                UserId = creditRequest.UserId,
                UserName = user.Username,
                UserEmail = user.Email
            });
        }

        // GET: api/creditrequests/mine
        [HttpGet("mine")]
        [Authorize(Roles = "Solicitante")]
        public IActionResult GetMyRequests()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var requests = _context.CreditRequests
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToList();

            var response = requests.Select(r => new CreditRequestResponseDto
            {
                Id = r.Id,
                Amount = r.Amount,
                TermInMonths = r.TermInMonths,
                MonthlyIncome = r.MonthlyIncome,
                WorkSeniority = r.WorkSeniority,
                EmploymentType = r.EmploymentType,
                CurrentDebt = r.CurrentDebt,
                Purpose = r.Purpose,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                RejectionReason = r.RejectionReason,
                ApprovedAmount = r.ApprovedAmount,
                InterestRate = r.InterestRate,
                MonthlyPayment = r.MonthlyPayment,
                UserId = r.UserId,
                UserName = r.User?.Username,
                UserEmail = r.User?.Email
            }).ToList();

            return Ok(response);
        }

        // GET: api/creditrequests
        [HttpGet]
        [Authorize(Roles = "Analista")]
        public IActionResult GetAll([FromQuery] string? status = null)
        {
            var query = _context.CreditRequests
                .Include(r => r.User)
                .AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);
            var requests = query.ToList();

            var response = requests.Select(r => new CreditRequestResponseDto
            {
                Id = r.Id,
                Amount = r.Amount,
                TermInMonths = r.TermInMonths,
                MonthlyIncome = r.MonthlyIncome,
                WorkSeniority = r.WorkSeniority,
                EmploymentType = r.EmploymentType,
                CurrentDebt = r.CurrentDebt,
                Purpose = r.Purpose,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                RejectionReason = r.RejectionReason,
                ApprovedAmount = r.ApprovedAmount,
                InterestRate = r.InterestRate,
                MonthlyPayment = r.MonthlyPayment,
                UserId = r.UserId,
                UserName = r.User?.Username,
                UserEmail = r.User?.Email
            }).ToList();

            return Ok(response);
        }

        // PUT: api/creditrequests/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Solicitante")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCreditRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.CreditRequests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (request == null) return NotFound(new { message = "Solicitud no encontrada." });
            if (request.Status != "Pendiente") return BadRequest(new { message = "Solo se puede editar una solicitud en estado Pendiente." });

            request.Amount = dto.Amount;
            request.TermInMonths = dto.TermInMonths;
            request.MonthlyIncome = dto.MonthlyIncome;
            request.WorkSeniority = dto.WorkSeniority;
            request.EmploymentType = dto.EmploymentType;
            request.CurrentDebt = dto.CurrentDebt;
            request.Purpose = dto.Purpose;
            // Reaplicar evaluación automática
            request.Status = dto.MonthlyIncome >= 1500 ? "Aprobado" : "Pendiente";
            request.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Log de auditoría
            var user = await _context.Users.FindAsync(userId);
            await _auditService.LogActionAsync(
                "UPDATE",
                "CreditRequest",
                request.Id,
                $"Solicitud actualizada por {user?.Username}",
                userId.ToString(),
                user?.Username ?? ""
            );

            var response = new CreditRequestResponseDto
            {
                Id = request.Id,
                Amount = request.Amount,
                TermInMonths = request.TermInMonths,
                MonthlyIncome = request.MonthlyIncome,
                WorkSeniority = request.WorkSeniority,
                EmploymentType = request.EmploymentType,
                CurrentDebt = request.CurrentDebt,
                Purpose = request.Purpose,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                RejectionReason = request.RejectionReason,
                ApprovedAmount = request.ApprovedAmount,
                InterestRate = request.InterestRate,
                MonthlyPayment = request.MonthlyPayment,
                UserId = request.UserId,
                UserName = user?.Username,
                UserEmail = user?.Email
            };
            return Ok(new { message = "Solicitud actualizada exitosamente.", solicitud = response });
        }

        // PUT: api/creditrequests/{id}/status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Analista")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var request = await _context.CreditRequests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
            if (request == null) return NotFound();
            request.Status = newStatus;
            request.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Log de auditoría
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(int.Parse(userId));
            await _auditService.LogActionAsync(
                "STATUS_CHANGE",
                "CreditRequest",
                request.Id,
                $"Estado cambiado a {newStatus} por {user?.Username}",
                userId,
                user?.Username ?? ""
            );

            var response = new CreditRequestResponseDto
            {
                Id = request.Id,
                Amount = request.Amount,
                TermInMonths = request.TermInMonths,
                MonthlyIncome = request.MonthlyIncome,
                WorkSeniority = request.WorkSeniority,
                EmploymentType = request.EmploymentType,
                CurrentDebt = request.CurrentDebt,
                Purpose = request.Purpose,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                RejectionReason = request.RejectionReason,
                ApprovedAmount = request.ApprovedAmount,
                InterestRate = request.InterestRate,
                MonthlyPayment = request.MonthlyPayment,
                UserId = request.UserId,
                UserName = request.User?.Username,
                UserEmail = request.User?.Email
            };
            return Ok(new { message = "Estado actualizado.", solicitud = response });
        }

        // DELETE: api/creditrequests/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Solicitante")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = await _context.CreditRequests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (request == null) return NotFound(new { message = "Solicitud no encontrada." });
            if (request.Status != "Pendiente") return BadRequest(new { message = "Solo se puede eliminar una solicitud en estado Pendiente." });
            _context.CreditRequests.Remove(request);
            await _context.SaveChangesAsync();

            // Log de auditoría
            var user = await _context.Users.FindAsync(userId);
            await _auditService.LogActionAsync(
                "DELETE",
                "CreditRequest",
                request.Id,
                $"Solicitud eliminada por {user?.Username}",
                userId.ToString(),
                user?.Username ?? ""
            );

            var response = new CreditRequestResponseDto
            {
                Id = request.Id,
                Amount = request.Amount,
                TermInMonths = request.TermInMonths,
                MonthlyIncome = request.MonthlyIncome,
                WorkSeniority = request.WorkSeniority,
                EmploymentType = request.EmploymentType,
                CurrentDebt = request.CurrentDebt,
                Purpose = request.Purpose,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                RejectionReason = request.RejectionReason,
                ApprovedAmount = request.ApprovedAmount,
                InterestRate = request.InterestRate,
                MonthlyPayment = request.MonthlyPayment,
                UserId = request.UserId,
                UserName = request.User?.Username,
                UserEmail = request.User?.Email
            };
            return Ok(new { message = "Solicitud eliminada exitosamente.", solicitud = response });
        }

        // GET: api/creditrequests/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Analista")]
        public async Task<ActionResult<CreditRequestResponseDto>> GetById(int id)
        {
            var creditRequest = await _context.CreditRequests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
            if (creditRequest == null)
                return NotFound();

            return new CreditRequestResponseDto
            {
                Id = creditRequest.Id,
                Amount = creditRequest.Amount,
                TermInMonths = creditRequest.TermInMonths,
                MonthlyIncome = creditRequest.MonthlyIncome,
                WorkSeniority = creditRequest.WorkSeniority,
                EmploymentType = creditRequest.EmploymentType,
                CurrentDebt = creditRequest.CurrentDebt,
                Purpose = creditRequest.Purpose,
                Status = creditRequest.Status,
                CreatedAt = creditRequest.CreatedAt,
                UpdatedAt = creditRequest.UpdatedAt,
                RejectionReason = creditRequest.RejectionReason,
                ApprovedAmount = creditRequest.ApprovedAmount,
                InterestRate = creditRequest.InterestRate,
                MonthlyPayment = creditRequest.MonthlyPayment,
                UserId = creditRequest.UserId,
                UserName = creditRequest.User?.Username,
                UserEmail = creditRequest.User?.Email
            };
        }
    }
} 