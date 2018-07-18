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
        public static void RunEmployeeQueries(Employee employee, string somestring)
        {

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
            Room room = new Room();
            room.AnimalId = animalID;
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
            // TODO: Complete Method
            List<Adoption> adoptions = new List<Adoption>();
            //var adoptionList = db.Clients.Where(x => x.ClientId = x.Adoptions.Cl);
            //foreach(Adoption a in adoptionList)
            //{
            //    adoptions.Add(a);
            //}
            return adoptions;
        }
        public static Animal GetAnimalByID(int iD)
        {
            Animal a = new Animal();
            return a;
        }
        public static void Adopt(Animal animal, Client client)
        {

        }
        public static List<Client> RetrieveClients()
        {
            List<Client> a = new List<Client>();
            return a;
        }
        public static List<USState> GetStates()
        {
            List<USState> a = new List<USState>();
            return a;
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
            List<AnimalShot> s = new List<AnimalShot>();
            return s;
        }
        public static void UpdateShot(string shot, Animal animal)
        {

        }
        public static void EnterUpdate(Animal animal, Dictionary<int,string> update)
        {

        }
        public static Species GetSpecies()
        {
            Species s = new Species();
            return s;
        }
        public static DietPlan GetDietPlan()
        {
            DietPlan d = new DietPlan();
            return d;
        }
        public static void AddAnimal(Animal animal)
        {

        }
        public static void RemoveAnimal(Animal animal)
        {

        }
        public static Employee EmployeeLogin(string userName, string password)
        {
            Employee e = new Employee();
            return e;
        }
        public static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee q = new Employee();
            return q;
        }
        public static void AddUsernameAndPassword(Employee employee)
        {

        }
        public static bool CheckEmployeeUserNameExist(string userName)
        {
            return true;
        }
    }
}
