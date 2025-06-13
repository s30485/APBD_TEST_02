namespace APBD_TEST_02.Models.Models;

public class Record
{
    public int Id { get; set; }
    public long ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }

    public int TaskId { get; set; }
    public Task Task { get; set; }

    public int LanguageId { get; set; }
    public Language Language { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}


