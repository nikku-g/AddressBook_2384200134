using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using ReposatoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressBookApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookService _addressBookBL;  // Changed to IAddressBookBL

        public AddressBookController(IAddressBookService addressBookBL)  // Changed to IAddressBookBL
        {
            _addressBookBL = addressBookBL;  // Injecting IAddressBookBL
        }

        [HttpGet]
        public ActionResult GetAllContacts()
        {
            var contacts = _addressBookBL.GetAllAddresses();  // Changed to GetAddresses()
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public ActionResult GetContactById(int id)
        {
            var contact = _addressBookBL.GetAddressById(id);  // Changed to GetAddressById()
            if (contact == null)
            {
                return NotFound($"Contact with ID {id} not found.");
            }
            return Ok(contact);
        }

        [HttpPost("{ID}")]
        public ActionResult AddContact(RequestModel contactRequest)
        {
            if (contactRequest == null)
            {
                return BadRequest("Invalid contact data.");
            }

            var newContact = _addressBookBL.AddAddress(contactRequest);  // Changed to AddAddress()
            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, newContact);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateContact(int id, RequestModel contactRequest)
        {
            if (contactRequest == null)
            {
                return BadRequest("Invalid contact data.");
            }

            var updatedContact = _addressBookBL.UpdateAddress(id, contactRequest);  // Changed to UpdateAddress()
            if (updatedContact == null)
            {
                return NotFound($"Contact with ID {id} not found.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            var deletedContact = _addressBookBL.DeleteAddress(id);  // Changed to DeleteAddress()
            if (!deletedContact)
            {
                return NotFound($"Contact with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
