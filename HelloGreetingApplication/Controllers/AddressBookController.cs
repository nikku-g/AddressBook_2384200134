using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using ReposatoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;

        public AddressBookController(IAddressBookBL addressBookBL)
        {
            _addressBookBL = addressBookBL;
        }

        /// <summary>
        /// GET: api/addressbook
        /// Fetch all contacts from the address book.
        /// </summary>
        [HttpGet]
        public ActionResult GetAllContacts()
        {
            var contacts = _addressBookBL.GetAddresses();
            return Ok(contacts);
        }

        /// <summary>
        /// GET: api/addressbook/{id}
        /// Get a contact by ID.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult GetContactById(int id)
        {
            var contact = _addressBookBL.GetAddressById(id);
            if (contact == null)
            {
                return NotFound($"Contact with ID {id} not found.");
            }
            return Ok(contact);
        }

        /// <summary>
        /// POST: api/addressbook
        /// Add a new contact to the address book.
        /// </summary>
        [HttpPost]
        public ActionResult AddContact( RequestModel contactRequest)
        {
            if (contactRequest == null)
            {
                return BadRequest("Invalid contact data.");
            }

            var newContact = _addressBookBL.AddAddress(contactRequest);
            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, newContact);
        }

        /// <summary>
        /// PUT: api/addressbook/{id}
        /// Update a contact in the address book.
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateContact(int id, RequestModel contactRequest)
        {
            if (contactRequest == null)
            {
                return BadRequest("Invalid contact data.");
            }

            var updatedContact = _addressBookBL.UpdateAddress(id, contactRequest);
            if (updatedContact == null)
            {
                return NotFound($"Contact with ID {id} not found.");
            }

            return NoContent(); 
            // Successful update, but no content to return.
        }

        /// <summary>
        /// DELETE: api/addressbook/{id}
        /// Delete a contact from the address book.
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            var deletedContact = _addressBookBL.DeleteAddress(id);
            if (deletedContact == false)
            {
                return NotFound($"Contact with ID {id} not found.");
            }

            return NoContent(); 
            // Successful deletion, no content to return.
        }
    }
}
