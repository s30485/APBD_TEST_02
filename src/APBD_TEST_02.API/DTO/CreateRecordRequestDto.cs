namespace APBD_TEST_02.API.DTO;

public class CreateRecordRequestDto
{
    public double ExecutionTime { get; set; }
    public string Created { get; set; } = null!;
    public int LanguageId { get; set; }
    public int StudentId { get; set; }
    public int? TaskId { get; set; }

    public TaskDtoForCreate? Task { get; set; }
}
