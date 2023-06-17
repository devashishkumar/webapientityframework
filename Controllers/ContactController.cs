using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactsAPIDbContext dbContext;

        public ContactController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET: api/<ContactController>
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            // return new string[] { "value1", "value2" };
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpPost(Name ="AddContact")]
        public async Task<IActionResult> AddContact(AddContactRequest contactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = contactRequest.Name,
                Address = contactRequest.Address,
                Email = contactRequest.Email,
                Phone = contactRequest.Phone
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut(Name = "UpdateContact")]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, AddContactRequest contactRequest)
        {
            var data = await dbContext.Contacts.FindAsync(id);
            if (data != null)
            {
                /** var contact = new Contact()
                {
                    Id = Guid.NewGuid(),
                    Name = contactRequest.Name,
                    Address = contactRequest.Address,
                    Email = contactRequest.Email,
                    Phone = contactRequest.Phone
                };
                await dbContext.Contacts.AddAsync(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact); **/
                data.Name = contactRequest.Name;
                data.Address = contactRequest.Address;
                data.Email = contactRequest.Email;
                data.Phone = contactRequest.Phone;
                await dbContext.SaveChangesAsync();
                return Ok(data);


            }
            return NotFound();
        }

        [HttpGet(Name = "GetContact")]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var data = await dbContext.Contacts.FindAsync(id);
            if (data != null)
            {
                return Ok(data);
            }
            return NotFound(data);
        }

        [HttpDelete(Name = "DeleteContact")]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var data = await dbContext.Contacts.FindAsync(id);
            if (data != null)
            {
                dbContext.Remove(data);
                await dbContext.SaveChangesAsync();
                return Ok(data);
            }
            return NotFound(data);
        }
    }
}
