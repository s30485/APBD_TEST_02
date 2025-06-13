namespace APBD_TEST_02.API.DTO;

public class RecordResponseDto
{
    public int Id { get; set; }
    public LanguageDto Language { get; set; }
    public StudentDto Student { get; set; }
    public TaskTodoDtoFull Task { get; set; }
    public string ExecutionTime { get; set; }
    public string Created { get; set; }
}