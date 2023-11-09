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
    public IActionResult GetAll()
    {
        var persons = _DBContext.Persons.ToList();

        return Ok(persons);
    }

    [HttpPost]
    [Route("create")]
    public IActionResult Create([FromBody] Person _person)
    {
        var person = _DBContext.Persons.FirstOrDefault(o => o.IdentificationNumber == _person.IdentificationNumber);

        if (person != null)
        {
            return Ok(person);
        }
        else
        {
            _DBContext.Persons.Add(_person);
            _DBContext.SaveChanges();
            return Ok(true);
        }

    }
}
