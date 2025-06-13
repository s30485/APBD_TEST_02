namespace APBD_TEST_02.API.Models;

public class TaskTodo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<Record> Records { get; set; }
}