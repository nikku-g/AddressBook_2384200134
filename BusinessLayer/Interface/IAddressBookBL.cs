using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using ReposatoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        List<AddressBookDTO> GetAllAddresses();
        AddressBookDTO GetAddressById(int id);
        AddressBookDTO AddAddress(RequestModel contactRequest);
        AddressBookDTO UpdateAddress(int id, RequestModel contactRequest);
        bool DeleteAddress(int id);
    }
        
}
