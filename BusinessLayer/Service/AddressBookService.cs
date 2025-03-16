using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.Model;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interface;
using System;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Messaging;
using ModelLayer.Event;

namespace BusinessLayer.Service
{
    public class AddressBookService : IAddressBookService
    {
        private readonly IAddressBookRL _addressBookRL;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _defaultCacheDuration = TimeSpan.FromMinutes(5);

        public AddressBookService(IAddressBookRL addressBookRL, IMapper mapper, IDistributedCache cache)
        {
            _addressBookRL = addressBookRL;
            _mapper = mapper;
            _cache = cache;
        }
        public void AddContact(UserDTO userDto)
        {
            // Contact saving logic
            var contactAddedEvent = new ContactAddedEvent
            {
                ContactId = userDto.Id,
                ContactName = userDto.Username
            };

            var publisher = new RabbitMQPublisher();
            publisher.Publish(contactAddedEvent, "contact.added");
        }



        public List<AddressBookDTO> GetAllAddresses()
        {
            // Check if the data is cached
            var cachedData = _cache.GetString("AllAddresses");
            if (!string.IsNullOrEmpty(cachedData))
            {
                // Deserialize and return the cached data
                return JsonConvert.DeserializeObject<List<AddressBookDTO>>(cachedData);
            }

            // If not cached, fetch from the database
            var addresses = _addressBookRL.GetAddresses();
            var addressDTOs = _mapper.Map<List<AddressBookDTO>>(addresses);

            // Cache the data
            var serializedData = JsonConvert.SerializeObject(addressDTOs);
            _cache.SetString("AllAddresses", serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _defaultCacheDuration // Set cache duration
            });

            return addressDTOs;
        }

        public AddressBookDTO GetAddressById(int id)
        {
            var cacheKey = $"Address_{id}";

            // Check if the address is in cache
            var cachedAddress = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(cachedAddress))
            {
                return JsonConvert.DeserializeObject<AddressBookDTO>(cachedAddress);
            }

            // If not cached, fetch from the database
            var address = _addressBookRL.GetAddressById(id);
            if (address == null)
            {
                return null;
            }

            var addressDTO = _mapper.Map<AddressBookDTO>(address);

            // Cache the address
            var serializedAddress = JsonConvert.SerializeObject(addressDTO);
            _cache.SetString(cacheKey, serializedAddress, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _defaultCacheDuration
            });

            return addressDTO;
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
