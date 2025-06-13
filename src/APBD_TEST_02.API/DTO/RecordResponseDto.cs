namespace APBD_TEST_02.API.DTO;

public class RecordResponseDto
{
    public int Id { get; set; }
    public long ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public string TaskName { get; set; }
    public string LanguageName { get; set; }
    public string StudentFullName { get; set; }
}