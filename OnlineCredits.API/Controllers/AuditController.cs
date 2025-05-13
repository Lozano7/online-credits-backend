using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCredits.Application.DTOs;
using OnlineCredits.Application.Services;

namespace OnlineCredits.API.Controllers
{
    [Authorize(Roles = "Analista")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAuditLogs()
        {
            var logs = await _auditService.GetAuditLogsAsync();
            return Ok(logs);
        }

        [HttpGet("entity/{entityType}/{entityId}")]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAuditLogsByEntity(string entityType, int entityId)
        {
            var logs = await _auditService.GetAuditLogsByEntityAsync(entityType, entityId);
            return Ok(logs);
        }
    }
} 