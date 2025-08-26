namespace Demo.Results;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;

    private Person(int id, string name, string email, DateTime dateOfBirth, string address)
    {
        Id = id;
        Name = name;
        Email = email;
        DateOfBirth = dateOfBirth;
        Address = address;
    }

    public static Person[] CreateDemoPersons() =>
    [
        new Person(1, "John Doe", "jdoe@company.com", new DateTime(1990, 1, 1), "123 Main St"),
        new Person(2, "Jane Smith", "jsmith@company.com", new DateTime(1985, 5, 15), "456 Elm St"),
        new Person(3, "Alice Johnson", "ajohnson@othercompany.com" , new DateTime(2000, 3, 20), "789 Oak St"),
        new Person(4, "Bob Brown", "bbrown@company.com" , new DateTime(1995, 7, 30), "321 Pine St")
    ];

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Email: {Email}, DateOfBirth: {DateOfBirth.ToShortDateString()}, Address: {Address}";
    }

}
