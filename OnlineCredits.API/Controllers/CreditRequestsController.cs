using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCredits.Core.Entities;
using OnlineCredits.Infrastructure.Data;
using System.Linq;
using System.Security.Claims;
using OnlineCredits.Core.DTOs.CreditRequest;
using Microsoft.EntityFrameworkCore;

namespace OnlineCredits.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CreditRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/creditrequests
        [HttpPost]
        [Authorize(Roles = "Solicitante")]
        public IActionResult Create([FromBody] CreateCreditRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = new CreditRequest
            {
                UserId = userId,
                Amount = dto.Amount,
                TermInMonths = dto.TermInMonths,
                MonthlyIncome = dto.MonthlyIncome,
                WorkSeniority = dto.WorkSeniority,
                EmploymentType = dto.EmploymentType,
                CurrentDebt = dto.CurrentDebt,
                Purpose = dto.Purpose,
                Status = dto.MonthlyIncome >= 1500 ? "Aprobado" : "Pendiente",
                CreatedAt = DateTime.UtcNow
            };
            _context.CreditRequests.Add(request);
            _context.SaveChanges();
            return Ok(new { message = "Solicitud creada exitosamente.", request });
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
        public IActionResult Update(int id, [FromBody] UpdateCreditRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = _context.CreditRequests.FirstOrDefault(r => r.Id == id && r.UserId == userId);
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
            _context.SaveChanges();
            return Ok(new { message = "Solicitud actualizada exitosamente.", request });
        }

        // PUT: api/creditrequests/{id}/status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Analista")]
        public IActionResult UpdateStatus(int id, [FromBody] string newStatus)
        {
            var request = _context.CreditRequests.Find(id);
            if (request == null) return NotFound();
            request.Status = newStatus;
            request.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
            return Ok(new { message = "Estado actualizado.", request });
        }

        // DELETE: api/creditrequests/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Solicitante")]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var request = _context.CreditRequests.FirstOrDefault(r => r.Id == id && r.UserId == userId);
            if (request == null) return NotFound(new { message = "Solicitud no encontrada." });
            if (request.Status != "Pendiente") return BadRequest(new { message = "Solo se puede eliminar una solicitud en estado Pendiente." });
            _context.CreditRequests.Remove(request);
            _context.SaveChanges();
            return Ok(new { message = "Solicitud eliminada exitosamente." });
        }
    }
} 