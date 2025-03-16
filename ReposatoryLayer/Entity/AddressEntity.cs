using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryLayer.Entity
{
    /// <summary>
    /// Entity representing a greeting message stored in the database.
    /// </summary>
    public class AddressEntity
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
