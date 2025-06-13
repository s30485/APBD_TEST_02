namespace APBD_TEST_02.API.DTO;

public class CreateRecordRequestDto
{
    public long ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LanguageId { get; set; }
    public int StudentId { get; set; }
    public int? TaskId { get; set; }
    public string? TaskName { get; set; }
    public string? TaskDescription { get; set; }
}
