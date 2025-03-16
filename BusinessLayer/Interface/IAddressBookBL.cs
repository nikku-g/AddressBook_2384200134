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
        public List<RequestModel> GetAddresses();
        public RequestModel GetAddressById(int id);
        public AddressEntity AddAddress(RequestModel contactRequest);
        public AddressEntity UpdateAddress(int id, RequestModel contactRequest);
        public bool DeleteAddress(int id);
    }
        
}
