using APBD_TEST_02.API.Models;

namespace APBD_TEST_02.Models.Models;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public ICollection<Record> Records { get; set; }
}
