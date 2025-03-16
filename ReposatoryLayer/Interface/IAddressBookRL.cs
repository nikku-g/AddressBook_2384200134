using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReposatoryLayer.Entity;

namespace ReposatoryLayer.Interface
{
    public interface IAddressBookRL
    {
        public List<AddressEntity> GetAddresses();
        public AddressEntity GetAddressById(int id);
        public AddressEntity AddAddress(AddressEntity addressEntity);
        public AddressEntity UpdateAddress(AddressEntity addressEntity);
        public bool DeleteAddress(int id);
        public UserEntity AddAddress(UserEntity userEntity);
        public List<UserEntity> GetAddresse();
        public UserEntity UpdateAddress(UserEntity userEntity);



    }
}
