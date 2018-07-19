using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        private static HumaneSocietyDataContext db = new HumaneSocietyDataContext(@"C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\HumaneSociety.mdf");
        public static void RunEmployeeQueries(Employee employee, string action)
        {
            switch (action)
            {
                case "create":
                    db.Employees.InsertOnSubmit(employee);
                    break;
                case "read":
                    var readEmployee = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).Single();
                    UserInterface.DisplayEmployeeInfo(readEmployee);
                    break;
                case "update":
                    var updateEmployee = db.Employees.Where(e => e.UserName == employee.UserName).Single();
                    updateEmployee.FirstName = employee.FirstName;
                    updateEmployee.LastName = employee.LastName;
                    updateEmployee.EmployeeId = employee.EmployeeId;
                    updateEmployee.Email = employee.Email;
                    SubmitDBChanges();
                    break;
                case "delete":
                    db.Employees.DeleteOnSubmit(employee);
                    break;
                default:
                    break;
            }
        }
        public static void SubmitDBChanges()
        {
            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static Room GetRoom(int animalID)
        {
            var room = db.Rooms.Where(r => r.AnimalId == animalID).Single();
            return room;
        }
        public static List<Adoption> GetPendingAdoptions()
        {;
            var adoption = db.Adoptions.Where(m => m.ApprovalStatus == "Pending");
            return adoption.ToList();
        }
        public static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            var adopted = db.Adoptions.Where(a => a.AdoptionId == adoption.AdoptionId).SingleOrDefault();
            
            if(isAdopted)
            {
                adopted.ApprovalStatus = "Adopted";
            }
            else
            {
                adopted.ApprovalStatus = "Not Adopted";
            }
            
            SubmitDBChanges();
        }
        public static Client GetClient(string userName, string password)
        {
            var client = db.Clients.Where(c => c.UserName == userName && c.Password == password).SingleOrDefault();
            if(client != default(Client))
            {
                return client;
            }
            else
            {
                throw new Exception();
            }
        }
        public static List<Adoption> GetUserAdoptionStatus(Client client)
        {
            var adoptions = db.Adoptions.Where(c => c.ClientId == client.ClientId);
            return adoptions.ToList();
        }
        public static Animal GetAnimalByID(int iD)
        {
            var animal = db.Animals.Where(a => a.AnimalId == iD).SingleOrDefault();
            if (animal != default(Animal))
            {
                return animal;
            }
            throw new Exception();
        }
        public static void Adopt(Animal animal, Client client)
        {
            var Animal = db.Animals.Where(s => s.AnimalId == animal.AnimalId).Single();
            Animal.AdoptionStatus = "Adopted";
            SubmitDBChanges();
        }
        public static List<Client> RetrieveClients()
        {
            var clients = db.Clients.ToList();
            return clients;
        }
        public static List<USState> GetStates()
        {
            var states = db.USStates.ToList();
            return states;
        }
        public static void updateClient(Client client)
        {
            var updateClient = db.Clients.Where(s => s.ClientId == client.ClientId).Single();
            updateClient.Income = client.Income;
            updateClient.HomeSquareFootage = client.HomeSquareFootage;
            updateClient.NumberOfKids = client.NumberOfKids;
            updateClient.Password = client.Password;
            SubmitDBChanges();
        }
        public static void AddNewClient(string firstName, string lastName, string userName, string password, string email, string address, int zipCode, int state)
        {
            Client newClient = new Client
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Password = password,
                Email = email
            };
            Address newClientAddress = new Address
            {
                AddressLine1 = address,
                Zipcode = zipCode,
                USStateId = state
            };
            db.Addresses.InsertOnSubmit(newClientAddress);
            SubmitDBChanges();
            newClient.Address = db.Addresses.Where(c => c.AddressLine1 == address && c.Zipcode == zipCode).Single();
            SubmitDBChanges();
        }
        public static void UpdateEmail(Client client)
        {
            var clientToUpdate = db.Clients.Where(c => c.ClientId == client.ClientId).Single();
            clientToUpdate.Email = client.Email;
        }
        public static void UpdateAddress(Client client)
        {
            var clientToUpdate = db.Clients.Where(c => c.ClientId == client.ClientId).Single();
            clientToUpdate.Address = client.Address;
        }
        public static void UpdateUsername(Client client)
        {
            var clientToUpdate = db.Clients.Where(c => c.ClientId == client.ClientId).Single();
            clientToUpdate.UserName = client.UserName;
        }
        public static void UpdateFirstName(Client client)
        {
            var clientToUpdate = db.Clients.Where(c => c.ClientId == client.ClientId).Single();
            clientToUpdate.FirstName = client.FirstName;
        }
        public static void UpdateLastName(Client client)
        {
            var clientToUpdate = db.Clients.Where(c => c.ClientId == client.ClientId).Single();
            clientToUpdate.LastName = client.LastName;
        }
        public static List<AnimalShot> GetShots(Animal animal)
        {

            var animalShots = db.AnimalShots.Where(a => a.AnimalId == animal.AnimalId);
            return animalShots.ToList();
        }
        public static void UpdateShot(string shot, Animal animal)
        {
            AnimalShot animalShot = new AnimalShot
            {
                Animal = animal,
                AnimalId = animal.AnimalId,
                DateReceived = DateTime.Now
            };
            animalShot.Shot = new Shot
            {
                ShotId = db.Shots.Where(s => s.Name == shot).Select(s => s.ShotId).Single()
            };
            db.AnimalShots.InsertOnSubmit(animalShot);
            SubmitDBChanges();
        }
        public static void EnterUpdate(Animal animal, Dictionary<int,string> update)
        {
            var updateAnimal = db.Animals.Where(a => a.AnimalId == animal.AnimalId).Single();
            
        }
        public static Species GetSpecies()
        {
            var Species = db.Species.Single();
            return Species;
        }
        public static DietPlan GetDietPlan()
        {
            var dietPlan = db.DietPlans.Single();
            return dietPlan;
        }
        public static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            SubmitDBChanges();
        }
        public static void RemoveAnimal(Animal animal)
        {
            var removeAnimals = db.Animals.Where(r => r.AnimalId == animal.AnimalId).Single();
            db.Animals.DeleteOnSubmit(removeAnimals);
            SubmitDBChanges();
        }
        public static Employee EmployeeLogin(string userName, string password)
        {
            var employee = db.Employees.Where(e => e.UserName == userName && e.Password == password).Single();
            return employee;

        }
        public static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            var employeeUser = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).Single();
            return employeeUser;
        }
        public static void AddUsernameAndPassword(Employee employee)
        {
            db.Employees.InsertOnSubmit(employee);
        }
        public static bool CheckEmployeeUserNameExist(string userName)
        {
            var exists = db.Clients.Where(s => s.UserName == userName);
            if (exists != null)
            {
                return true;
            }
            else
                return false;

        }
    }
}
