using EF04.Data;
using EF04.Models;
using Microsoft.EntityFrameworkCore;

namespace EF04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();

            SeedData(context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== BANK MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Add New Customer");
                Console.WriteLine("2. Open Account");
                Console.WriteLine("3. Update Account Status");
                Console.WriteLine("4. Remove Account from Customer");
                Console.WriteLine("5. List Customers with Accounts");
                Console.WriteLine("0. Exit");
                Console.Write("\nSelect Option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCustomer(context);
                        break;

                    case "2":
                        OpenAccount(context);
                        break;

                    case "3":
                        UpdateAccountStatus(context);
                        break;

                    case "4":
                        RemoveAccount(context);
                        break;

                    case "5":
                        ListCustomers(context);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice!");
                        Pause();
                        break;
                }
            }
        }

        #region Seed Data (Branch + Manager)
        static void SeedData(AppDbContext context)
        {
            if (!context.Branches.Any())
            {
                var branch = new Branch
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
        }
        #endregion

        #region Question 1: Add Customer
        static void AddCustomer(AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("=== Add New Customer ===");

            Console.Write("Full Name: ");
            var name = Console.ReadLine();

            Console.Write("National ID: ");
            var nationalId = Console.ReadLine();

            Console.Write("Date of Birth (yyyy-mm-dd): ");
            DateTime dob = DateTime.Parse(Console.ReadLine());

            Console.Write("Email: ");
            var email = Console.ReadLine();

            Console.Write("Phone: ");
            var phone = Console.ReadLine();

            Console.Write("Address: ");
            var address = Console.ReadLine();

            Console.Write("Type (Individual / Business): ");
            var type = Console.ReadLine();

            var customer = new Customer
            {
                FullName = name,
                NationalId = nationalId,
                DateOfBirth = dob,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = type
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            Console.WriteLine("\nCustomer added successfully!");
            Pause();
        }
        #endregion

        #region Question 2: Open Account
        static void OpenAccount(AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("=== Open Account ===");

            Console.Write("Account Number: ");
            var accNum = Console.ReadLine();

            Console.Write("Account Type: ");
            var type = Console.ReadLine();

            Console.Write("Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            var customer = context.Customers.Find(customerId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found!");
                Pause();
                return;
            }

            var branch = context.Branches.First();

            var account = new Account
            {
                AccountNumber = accNum,
                AccountType = type,
                OpeningDate = DateTime.Now,
                CurrentBalance = 0,
                BranchId = branch.Id
            };

            context.Accounts.Add(account);
            context.SaveChanges();

            var ca = new CustomerAccount
            {
                CustomerId = customerId,
                AccountId = account.Id,
                OwnershipStartDate = DateTime.Now,
                OwnershipType = "Primary",
                AccountStatus = "Active"
            };

            context.CustomerAccounts.Add(ca);
            context.SaveChanges();

            Console.WriteLine("\nAccount created successfully!");
            Pause();
        }
        #endregion

        #region Question 3: Update Status
        static void UpdateAccountStatus(AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("=== Update Account Status ===");

            Console.Write("Account Number: ");
            var accNum = Console.ReadLine();

            var account = context.Accounts
                .Include(a => a.CustomerAccounts)
                .FirstOrDefault(a => a.AccountNumber == accNum);

            if (account == null)
            {
                Console.WriteLine("Account not found!");
                Pause();
                return;
            }

            foreach (var ca in account.CustomerAccounts)
            {
                ca.AccountStatus = ca.AccountStatus == "Active" ? "Closed" : "Active";
            }

            context.SaveChanges();

            Console.WriteLine("\nStatus updated successfully!");
            Pause();
        }
        #endregion

        #region Question 4: Remove Account
        static void RemoveAccount(AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("=== Remove Account from Customer ===");

            Console.Write("Account Number: ");
            var accNum = Console.ReadLine();

            Console.Write("Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            var ca = context.CustomerAccounts
                .FirstOrDefault(x => x.Account.AccountNumber == accNum && x.CustomerId == customerId);

            if (ca == null)
            {
                Console.WriteLine("Relation not found!");
                Pause();
                return;
            }

            context.CustomerAccounts.Remove(ca);
            context.SaveChanges();

            Console.WriteLine("\nRemoved successfully!");
            Pause();
        }
        #endregion

        #region Question 5: List Customers
        static void ListCustomers(AppDbContext context)
        {
            Console.Clear();
            Console.WriteLine("=== Customers List ===\n");

            var customers = context.Customers
                .Include(c => c.CustomerAccounts)
                .ThenInclude(ca => ca.Account)
                .ToList();

            foreach (var c in customers)
            {
                Console.WriteLine($"Customer: {c.FullName}");

                foreach (var ca in c.CustomerAccounts)
                {
                    Console.WriteLine($"   Account: {ca.Account.AccountNumber} | Status: {ca.AccountStatus}");
                }

                Console.WriteLine("----------------------------------");
            }

            Pause();
        }
        #endregion

        static void Pause()
        {
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}