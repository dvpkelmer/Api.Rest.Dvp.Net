using apiBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiBackend.Controllers;

[ApiController]
[Route("api/person")]
public class PersonController : ControllerBase
{

    private readonly PruebaDvpContext _DBContext;

    public PersonController(PruebaDvpContext dBContext)
    {
        _DBContext = dBContext;
    }

    [HttpGet]
    [Route("getAll")]
    public dynamic GetAll()
    {
        var persons = _DBContext.Persons.ToList();

       return new
            {
                success = true,
                message = "successful listing",
                result = persons
            };
    }

    [HttpPost]
    [Route("create")]
    public dynamic Create([FromBody] Person _person)
    {
        var person = _DBContext.Persons.FirstOrDefault(o => o.IdentificationNumber == _person.IdentificationNumber);

        if (person != null)
        {
            return new
            {
                success = false,
                message = "Person already exists",
                result = ""
            };
        }
        else
        {
             _person.CreatedAt = DateTime.Today;
            _DBContext.Persons.Add(_person);
            _DBContext.SaveChanges();
            
             return new
            {
                success = true,
                message = "Person created successfully",
                result = _person
            };
        }

    }
}
