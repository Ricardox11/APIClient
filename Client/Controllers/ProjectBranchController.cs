using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppClient.Models;

namespace AppClient.Controllers
{

    // ruta master detail
    [Route("api/Project/{ProjectId}/Branch/{BranchId}/[controller]")]
    [ApiController]
    public class ProjectBranchController : ControllerBase
    {
        private readonly ClientContext _context;

        public ProjectBranchController(ClientContext context)
        {
            _context = context;
        }

        // combinar consulta por ruta project projectbranch
        // GET: api/ProjectBranch
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectBranch>>> GetProjectBranches(Guid ProjectId, Guid BranchId)
        {
            // buscar el id master detail
            return await _context.ProjectBranches.Where(x => x.ProjectId == ProjectId && x.BranchId == BranchId).ToListAsync();
        }

        // GET: api/ProjectBranch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectBranch>> GetProjectBranch(Guid id, Guid ProjectId, Guid BranchId)
        {
            var projectBranch = await _context.ProjectBranches.FindAsync(ProjectId, BranchId);

            if (projectBranch == null)
            {
                return NotFound();
            }

            return projectBranch;
        }

        // PUT: api/ProjectBranch/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProjectBranch(Guid ProjectId, Guid BranchId, ProjectBranch projectBranch)
        {
            if (ProjectId != projectBranch.ProjectId  && BranchId != projectBranch.BranchId)
            {
               return BadRequest();
            }

            _context.Entry(projectBranch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectBranchExists(projectBranch.ProjectId, projectBranch.BranchId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // combinar ruta master detail
        // POST: api/ProjectBranch
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectBranch>> PostProjectBranch(ProjectBranch projectBranch, Guid ProjectId, Guid BranchId)
        {

            // cargar valores master detail
            projectBranch.BranchId = BranchId;
            projectBranch.ProjectId = ProjectId;


            _context.ProjectBranches.Add(projectBranch);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjectBranchExists(projectBranch.ProjectId, projectBranch.BranchId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            // crea dinamcamente llamado get con id
            return CreatedAtAction("GetProjectBranch", new { id = projectBranch.ProjectId, ProjectId = projectBranch.ProjectId, BranchId = projectBranch.BranchId }, projectBranch);
        }

        // DELETE: api/ProjectBranch/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectBranch(Guid id, Guid ProjectId, Guid BranchId)
        {
            var projectBranch = await _context.ProjectBranches.FindAsync(ProjectId, BranchId);
            if (projectBranch == null)
            {
                return NotFound();
            }

            _context.ProjectBranches.Remove(projectBranch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectBranchExists(Guid ProjectId, Guid BranchId)
        {
            return _context.ProjectBranches.Any(e => e.ProjectId == ProjectId && e.BranchId == BranchId);
        }
    }
}
