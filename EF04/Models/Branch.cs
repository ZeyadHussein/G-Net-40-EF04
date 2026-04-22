using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF04.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        
        public Manager Manager { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
