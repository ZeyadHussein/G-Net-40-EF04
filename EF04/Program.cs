using EF04.Data;
using EF04.Models;

namespace EF04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();

            #region Question 1: Add New Customer
            var customer = new Customer
            {
                FullName = "Ahmed Ali",
                NationalId = "123456789",
                DateOfBirth = new DateTime(1995, 5, 1),
                Email = "ahmed@test.com",
                PhoneNumber = "0100000000",
                Address = "Cairo",
                CustomerType = "Individual"
            };

            context.Customers.Add(customer);
            context.SaveChanges();
            #endregion
        }
    }
}
