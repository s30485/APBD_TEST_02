using System.ComponentModel.DataAnnotations;

namespace APBD_TEST_02.Models.Models;

public class Task
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Record> Records { get; set; }
}