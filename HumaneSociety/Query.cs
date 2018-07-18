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
            Room room = new Room
            {
                AnimalId = animalID
            };
            db.Rooms.InsertOnSubmit(room);
            SubmitDBChanges();
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
                adopted.ApprovalStatus = "Approved";
            }
            else
            {
                adopted.ApprovalStatus = "Pending";
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

        }
        public static void AddNewClient(string firstName, string lastName, string userName, string password, string email, string address, int zipCode, int state)
        {

        }
        public static void UpdateEmail(Client client)
        {

        }
        public static void UpdateAddress(Client client)
        {

        }
        public static void UpdateUsername(Client client)
        {

        }
        public static void UpdateFirstName(Client client)
        {

        }
        public static void UpdateLastName(Client client)
        {

        }
        public static List<AnimalShot> GetShots(Animal animal)
        {
           
        }
        public static void UpdateShot(string shot, Animal animal)
        {

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
