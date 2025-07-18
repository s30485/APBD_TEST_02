﻿using APBD_TEST_02.API.DAL;
using APBD_TEST_02.API.DTO;
using APBD_TEST_02.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            .Include(r => r.TaskTodo)
            .AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(r => r.CreatedAt >= fromDate.Value);

        if (languageId.HasValue)
            query = query.Where(r => r.LanguageId == languageId.Value);

        if (taskId.HasValue)
            query = query.Where(r => r.TaskId == taskId.Value);

        var records = await query
            .OrderByDescending(r => r.CreatedAt)
            .ThenBy(r => r.Student.LastName)
            .Select(r => new RecordResponseDto
            {
                Id = r.Id,
                ExecutionTime = r.ExecutionTime,
                Created = r.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                Language = new LanguageDto
                {
                    Id = r.Language.Id,
                    Name = r.Language.Name
                },
                Student = new StudentDto
                {
                    Id = r.Student.Id,
                    FirstName = r.Student.FirstName,
                    LastName = r.Student.LastName,
                    Email = r.Student.Email
                },
                Task = new TaskTodoDtoFull
                {
                    Id = r.TaskTodo.Id,
                    Name = r.TaskTodo.Name,
                    Description = r.TaskTodo.Description
                }
            })
            .ToListAsync();

        return Ok(records);
    }
    
    [HttpPost]
    public async Task<ActionResult<RecordResponseDto>> CreateRecord([FromBody] CreateRecordRequestDto dto)
    {
        var language = await _context.Languages.FindAsync(dto.LanguageId);
        if (language == null)
            return NotFound($"Language with ID {dto.LanguageId} not found.");

        var student = await _context.Students.FindAsync(dto.StudentId);
        if (student == null)
            return NotFound($"Student with ID {dto.StudentId} not found.");

        TaskTodo? taskTodo = null;

        if (dto.TaskId.HasValue)
        {
            taskTodo = await _context.Tasks.FindAsync(dto.TaskId.Value);
            if (taskTodo == null)
                return NotFound($"TaskTodo with ID {dto.TaskId.Value} not found.");
        }
        else if (dto.Task != null &&
                 !string.IsNullOrWhiteSpace(dto.Task.Name) &&
                 !string.IsNullOrWhiteSpace(dto.Task.Description))
        {
            taskTodo = new TaskTodo
            {
                Name = dto.Task.Name,
                Description = dto.Task.Description
            };
            _context.Tasks.Add(taskTodo);
            await _context.SaveChangesAsync();
        }
        else
        {
            return BadRequest("Either TaskId or Task (name & description) must be provided.");
        }

        if (!DateTime.TryParse(dto.Created, out var createdAt))
        {
            return BadRequest("Invalid date format. Use 'MM/dd/yyyy HH:mm:ss'.");
        }

        var record = new Record
        {
            ExecutionTime = dto.Created,
            CreatedAt = createdAt,
            LanguageId = language.Id,
            StudentId = student.Id,
            TaskId = taskTodo.Id
        };

        _context.Records.Add(record);
        await _context.SaveChangesAsync();

        var response = new RecordResponseDto
        {
            Id = record.Id,
            ExecutionTime = record.ExecutionTime,
            Created = record.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
            Language = new LanguageDto
            {
                Id = language.Id,
                Name = language.Name
            },
            Student = new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email
            },
            Task = new TaskTodoDtoFull
            {
                Id = taskTodo.Id,
                Name = taskTodo.Name,
                Description = taskTodo.Description
            }
        };

        return CreatedAtAction(nameof(GetRecords), new { id = record.Id }, response);
    }
}