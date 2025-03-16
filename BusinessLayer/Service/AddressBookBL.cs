using BusinessLayer.Interface;
using ModelLayer.Model;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;

        public AddressBookBL(IAddressBookRL addressBookRL)
        {
            _addressBookRL = addressBookRL;
        }

        // Get all addresses
        public List<RequestModel> GetAddresses()
        {
            var addresses = _addressBookRL.GetAddresses();

            // Map each AddressEntity to RequestModel
            return addresses.Select(address => new RequestModel
            {
                Street = address.Street,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode
            }).ToList(); // Convert to List<RequestModel>
        }

        // Get a specific address by ID
        public RequestModel GetAddressById(int id)
        {
            var address = _addressBookRL.GetAddressById(id);
            if (address == null)
                return null;

            // Map AddressEntity to RequestModel
            return new RequestModel
            {
                Street = address.Street,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode
            };
        }

        // Add a new address
        public AddressEntity AddAddress(RequestModel contactRequest)
        {
            var addressEntity = new AddressEntity
            {
                // Map RequestModel to AddressEntity
                Street = contactRequest.Street,
                City = contactRequest.City,
                State = contactRequest.State,
                PostalCode = contactRequest.PostalCode
            };

            return _addressBookRL.AddAddress(addressEntity);
        }

        // Update an existing address
        public AddressEntity UpdateAddress(int id, RequestModel contactRequest)
        {
            var existingAddress = _addressBookRL.GetAddressById(id);
            if (existingAddress == null)
                return null;

            // Update fields in the existing AddressEntity
            existingAddress.Street = contactRequest.Street;
            existingAddress.City = contactRequest.City;
            existingAddress.State = contactRequest.State;
            existingAddress.PostalCode = contactRequest.PostalCode;

            return _addressBookRL.UpdateAddress(existingAddress);
        }

        // Delete an address asynchronously
        public bool DeleteAddress(int id)
        {
            return _addressBookRL.DeleteAddress(id);
        }
    }
}
