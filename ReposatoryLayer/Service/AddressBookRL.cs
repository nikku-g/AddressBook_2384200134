using ReposatoryLayer.Context;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReposatoryLayer.Service
{
    public class AddressBookRL : IAddressBookRL
    {
        private readonly AddressContext _context;

        public AddressBookRL(AddressContext context)
        {
            _context = context;
        }

        public List <AddressEntity> GetAddresses()
        {
            return _context.AddressEntities.ToList();
        }

        public AddressEntity GetAddressById(int id)
        {
            return _context.AddressEntities.Find(id);
        }

        public AddressEntity AddAddress(AddressEntity addressEntity)
        {
            _context.AddressEntities.Add(addressEntity);
            _context.SaveChanges();
            return addressEntity;
        }

        public AddressEntity UpdateAddress(AddressEntity addressEntity)
        {
            _context.AddressEntities.Update(addressEntity);
            _context.SaveChanges();
            return addressEntity;
        }

        public bool DeleteAddress(int id)
        {
            var address = _context.AddressEntities.Find(id);
            if (address == null)
            {
                return false;
            }

            _context.AddressEntities.Remove(address);
            _context.SaveChanges();
            return true;
        }
        public UserEntity AddAddress(UserEntity userEntity)
        {
            _context.UserEntities.Add(userEntity);
            _context.SaveChanges();
            return userEntity;
        }
        public List<UserEntity> GetAddresse()
        {
            return _context.UserEntities.ToList();
        }
        public UserEntity UpdateAddress(UserEntity userEntity)
        {
            _context.UserEntities.Update(userEntity);
            _context.SaveChanges();
            return userEntity;
        }
    }
}
