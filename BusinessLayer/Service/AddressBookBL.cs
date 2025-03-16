using AutoMapper;
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
        private readonly IMapper _mapper;

        public AddressBookBL(IAddressBookRL addressBookRL, IMapper mapper)
        {
            _addressBookRL = addressBookRL;
            _mapper = mapper;
        }

        public List<AddressBookDTO> GetAllAddresses()
        {
            var addresses = _addressBookRL.GetAddresses();
            return _mapper.Map<List<AddressBookDTO>>(addresses);
        }

        public AddressBookDTO GetAddressById(int id)
        {
            var address = _addressBookRL.GetAddressById(id);
            return address == null ? null : _mapper.Map<AddressBookDTO>(address);
        }

        public AddressBookDTO AddAddress(RequestModel contactRequest)
        {
            var addressEntity = _mapper.Map<AddressEntity>(contactRequest);
            var addedAddress = _addressBookRL.AddAddress(addressEntity);
            return _mapper.Map<AddressBookDTO>(addedAddress);
        }

        public AddressBookDTO UpdateAddress(int id, RequestModel contactRequest)
        {
            var existingAddress = _addressBookRL.GetAddressById(id);
            if (existingAddress == null) return null;

            _mapper.Map(contactRequest, existingAddress);
            var updatedAddress = _addressBookRL.UpdateAddress(existingAddress);
            return _mapper.Map<AddressBookDTO>(updatedAddress);
        }

        public bool DeleteAddress(int id)
        {
            return _addressBookRL.DeleteAddress(id);
        }
    }
}
