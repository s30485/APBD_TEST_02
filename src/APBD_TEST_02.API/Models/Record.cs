using APBD_TEST_02.Models.Models;

namespace APBD_TEST_02.API.Models;

public class Record
{
    public int Id { get; set; }
    public string ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }

    public int TaskId { get; set; }
    public TaskTodo TaskTodo { get; set; }

    public int LanguageId { get; set; }
    public Language Language { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}


