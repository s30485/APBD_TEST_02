using APBD_TEST_02.API.DAL;
using APBD_TEST_02.API.DTO;
using APBD_TEST_02.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

using ModelTask = APBD_TEST_02.Models.Task;

ModelTask task = new ModelTask
{
    Name = dto.TaskName!,
    Description = dto.TaskDescription!
};


namespace APBD_TEST_02.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class RecordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RecordsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecordResponseDto>>> GetRecords(
        [FromQuery] DateTime? fromDate,
        [FromQuery] int? languageId,
        [FromQuery] int? taskId)
    {
        var query = _context.Records
            .Include(r => r.Language)
            .Include(r => r.Student)
            .Include(r => r.Task)
            .AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(r => r.CreatedAt >= fromDate.Value);

        if (languageId.HasValue)
            query = query.Where(r => r.LanguageId == languageId);

        if (taskId.HasValue)
            query = query.Where(r => r.TaskId == taskId);

        var records = await query
            .OrderByDescending(r => r.CreatedAt)
            .ThenBy(r => r.Student.LastName)
            .Select(r => new RecordResponseDto
            {
                Id = r.Id,
                ExecutionTime = r.ExecutionTime,
                CreatedAt = r.CreatedAt,
                TaskName = r.Task.Name,
                LanguageName = r.Language.Name,
                StudentFullName = r.Student.FirstName + " " + r.Student.LastName
            })
            .ToListAsync();

        return Ok(records);
    }

    [HttpPost]
    public async Task<ActionResult<RecordResponseDto>> CreateRecord(CreateRecordRequestDto dto)
    {
        var language = await _context.Languages.FindAsync(dto.LanguageId);
        if (language == null)
            return NotFound($"Language with ID {dto.LanguageId} not found.");

        var student = await _context.Students.FindAsync(dto.StudentId);
        if (student == null)
            return NotFound($"Student with ID {dto.StudentId} not found.");

        Task? task = null;

        if (dto.TaskId.HasValue)
        {
            task = await _context.Tasks.FindAsync(dto.TaskId.Value);
            if (task == null)
            {
                if (!string.IsNullOrWhiteSpace(dto.TaskName) && !string.IsNullOrWhiteSpace(dto.TaskDescription))
                {
                    task = new Task
                    {
                        Name = dto.TaskName!,
                        Description = dto.TaskDescription!
                    };

                    _context.Tasks.Add(task);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound("Task not found and no valid name/description provided to create one.");
                }
            }
        }
        else
        {
            return BadRequest("Task ID must be provided.");
        }

        var record = new Record
        {
            ExecutionTime = dto.ExecutionTime,
            CreatedAt = dto.CreatedAt,
            LanguageId = language.Id,
            StudentId = student.Id,
            TaskId = task.Id
        };

        _context.Records.Add(record);
        await _context.SaveChangesAsync();

        var response = new RecordResponseDto
        {
            Id = record.Id,
            ExecutionTime = record.ExecutionTime,
            CreatedAt = record.CreatedAt,
            TaskName = task.Name,
            LanguageName = language.Name,
            StudentFullName = $"{student.FirstName} {student.LastName}"
        };

        return CreatedAtAction(nameof(GetRecords), new { id = record.Id }, response);
    }
}