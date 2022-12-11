using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
namespace tp3sandramourali.Models
{
    public class PersonalData
    { private readonly IConfiguration _configuration;

    public PersonalData(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<Person> GetAllPerson()
    {
        var list = new List<Person>();

        using (var connection = new SQLiteConnection(_configuration.GetConnectionString("SQLite")))
        {
            connection.Open();
            const string query = @"
                    SELECT *
                    FROM personal_info
                ";
            var command = new SQLiteCommand(query, connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = (int)reader["id"];
                    string firstName = (string)reader["first_name"];
                    string lastName = (string)reader["last_name"];
                    string email = (string)reader["email"];
                    string image = (string)reader["image"];
                    string country = (string)reader["country"];
                    list.Add(new Person(id, firstName, lastName, email, image, country));
                }
            }
        }

        return list;
    }

    public Person? GetPerson(int id)
    {
        var persons = GetAllPerson();
        return persons.Find((person) => person.Id == id);
    }

    delegate bool Validator(string x);

    public List<Person> GetPersons(string firstName, string lastName, string email, string country)
    {
        var persons = GetAllPerson();
        return persons.FindAll(person =>
        {
            Validator firstNameValidator = firstName != null
                ? (firstName) => firstName == person.FirstName
                : (x) => true;
            Validator lastNameValidator = lastName != null
                ? (lastName) => lastName == person.LastName
                : (x) => true;
            Validator emailValidator = lastName != null
                ? (email) => email == person.Email
                : (x) => true;
            Validator countryValidator = lastName != null
                ? (country) => country == person.Country
                : (x) => true;

            return firstNameValidator(firstName) &&
                   lastNameValidator(lastName) &&
                   emailValidator(email) &&
                   countryValidator(country);
        }
        );
    }
}
   
