using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using EPES.Data;
using EPES.Models;

namespace EPES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataGridPointOfEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataGridPointOfEvaluationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DataGridPointOfEvaluations
        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_context.PointOfEvaluations, loadOptions);
        }

        // GET: api/DataGridPointOfEvaluations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PointOfEvaluation>> GetPointOfEvaluation(int id)
        {
            var pointOfEvaluation = await _context.PointOfEvaluations.FindAsync(id);

            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            return pointOfEvaluation;
        }

        // PUT: api/DataGridPointOfEvaluations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPointOfEvaluation(int id, PointOfEvaluation pointOfEvaluation)
        {
            if (id != pointOfEvaluation.Id)
            {
                return BadRequest();
            }

            _context.Entry(pointOfEvaluation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PointOfEvaluationExists(id))
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

        // POST: api/DataGridPointOfEvaluations
        [HttpPost]
        public async Task<ActionResult<PointOfEvaluation>> PostPointOfEvaluation(PointOfEvaluation pointOfEvaluation)
        {
            _context.PointOfEvaluations.Add(pointOfEvaluation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPointOfEvaluation", new { id = pointOfEvaluation.Id }, pointOfEvaluation);
        }

        // DELETE: api/DataGridPointOfEvaluations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PointOfEvaluation>> DeletePointOfEvaluation(int id)
        {
            var pointOfEvaluation = await _context.PointOfEvaluations.FindAsync(id);
            if (pointOfEvaluation == null)
            {
                return NotFound();
            }

            _context.PointOfEvaluations.Remove(pointOfEvaluation);
            await _context.SaveChangesAsync();

            return pointOfEvaluation;
        }

        private bool PointOfEvaluationExists(int id)
        {
            return _context.PointOfEvaluations.Any(e => e.Id == id);
        }
    }
}
