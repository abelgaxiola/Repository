using System;

namespace RepositoryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GetFromRepositoryTable(2);
            Console.WriteLine();
            var userId = AddUserToRepositoryTable();
            Console.WriteLine();
            GetFromRepositoryTable(userId);
            Console.WriteLine();
            DeleteUserFromRepositoryTable();
            Console.WriteLine();
            GetFromRepositoryTable(3);
            Console.ReadLine();
        }

        private static void GetFromRepositoryTable(int userId)
        {
            using (var repository = new Repository<User>("LocalDB"))
            {
                if (repository.HasError)
                    Console.WriteLine(repository.ErrorMessage);

                var users = repository.GetAll();
                Console.WriteLine($"There are {users.Count.ToString()} users in the table");
                var user = repository.Get(userId);
                Console.WriteLine($"User with ID = {user.Id} Full Name: {user.GetFullName()}");
            }
        }

        private static int AddUserToRepositoryTable()
        {
            using (var repository = new Repository<User>("LocalDB"))
            {
                if (repository.HasError)
                    Console.WriteLine(repository.ErrorMessage);

                User user = GetNewUserRecord(repository);

                repository.Add(user);
                repository.SaveChanges();

                if (repository.HasError)
                    Console.WriteLine(repository.ErrorMessage);
                else
                    Console.WriteLine($"User {user.GetFullName()} has been added to the Users table");

                return repository.GetAll().Find(u => (u.FirstName == "Test")).Id;
            }
        }

        private static User GetNewUserRecord(Repository<User> repository)
        {
            User user = repository.GetNew();

            user.FirstName = "Test";
            user.LastName = "Tester";
            user.Age = 19;
            user.City = "Chicago";
            user.State = "IL";

            return user;
        }

        private static void DeleteUserFromRepositoryTable()
        {
            using (var repository = new Repository<User>("LocalDB"))
            {
                if (repository.HasError)
                    Console.WriteLine(repository.ErrorMessage);

                var user = repository.GetAll().Find(u => (u.FirstName == "Test"));

                repository.Delete(user.Id);
                repository.SaveChanges();

                if (repository.HasError)
                    Console.WriteLine(repository.ErrorMessage);
                else
                    Console.WriteLine($"User {user.GetFullName()} has successfully been deleted");
            }
        }
    }
}
