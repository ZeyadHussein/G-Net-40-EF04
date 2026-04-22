using EF04.Data;
using EF04.Models;

namespace EF04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();

            #region Setup (Create Branch + Manager) REQUIRED

            var branch = context.Branches.FirstOrDefault();

            if (branch == null)
            {
                branch = new Branch
                {
                    Name = "Main Branch",
                    Code = "B001",
                    Address = "Cairo",
                    PhoneNumber = "123456"
                };

                context.Branches.Add(branch);
                context.SaveChanges();

                var manager = new Manager
                {
                    FullName = "Manager 1",
                    Email = "manager@test.com",
                    PhoneNumber = "010000000",
                    HireDate = DateTime.Now,
                    BranchId = branch.Id
                };

                context.Managers.Add(manager);
                context.SaveChanges();
            }

            #endregion


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

            Console.WriteLine("Q1 Done: Customer Added");

            #endregion


            #region Question 2: Open Account

            var account = new Account
            {
                AccountNumber = "ACC1001",
                AccountType = "Savings",
                OpeningDate = DateTime.Now,
                CurrentBalance = 1000,
                BranchId = branch.Id 
            };

            context.Accounts.Add(account);
            context.SaveChanges();

            var customerAccount = new CustomerAccount
            {
                CustomerId = customer.Id,
                AccountId = account.Id,
                OwnershipStartDate = DateTime.Now,
                OwnershipType = "Primary",
                AccountStatus = "Active"
            };

            context.CustomerAccounts.Add(customerAccount);
            context.SaveChanges();

            Console.WriteLine("Q2 Done: Account Created");

            #endregion


        }
    }
}
