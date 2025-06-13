namespace APBD_TEST_02.Models.Models;

public class Language
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Record> Records { get; set; }
}