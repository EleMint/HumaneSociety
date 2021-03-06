﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class UserEmployee : User
    {
        Employee employee;
        
        public override void LogIn()
        {
            if (CheckIfNewUser())
            {
                CreateNewEmployee();
                LogInPreExistingUser();
            }
            else
            {
                Console.Clear();
                LogInPreExistingUser();
            }
            RunUserMenus();
        }
        protected override void RunUserMenus()
        {
            List<string> options = new List<string>() { "What would you like to do? (select number of choice)", "1. Add animal", "2. Remove Anmial", "3. Check Animal Status",  "4. Approve Adoption", "5. Diplay Rooms", "6. Move Animal", "7. Bulk Add Animals", "8. Check Shots" };
            UserInterface.DisplayUserOptions(options);
            string input = UserInterface.GetUserInput();
            RunUserInput(input);
        }
        private void RunUserInput(string input)
        {
            switch (input)
            {
                case "1":
                    AddAnimal();
                    RunUserMenus();
                    return;
                case "2":
                    RemoveAnimal();
                    RunUserMenus();
                    return;
                case "3":
                    CheckAnimalStatus();
                    RunUserMenus();
                    return;
                case "4":
                    CheckAdoptions();
                    RunUserMenus();
                    return;
                case "5":
                    DisplayRooms();
                    RunUserMenus();
                    return;
                case "6":
                    MoveAnimal();
                    RunUserMenus();
                    return;
                case "7":
                    BulkAdd();
                    RunUserMenus();
                    return;
                case "8":
                    CheckShots();
                    RunUserMenus();
                    return;
                default:
                    UserInterface.DisplayUserOptions("Input not accepted please try again");
                    RunUserMenus();
                    return;
            }
        }
        private void CheckShots()
        {
            var animal = SearchForAnimal().Single();
            var shots = Query.CheckShotsNeededById(animal);
            UserInterface.DisplayAnimal(animal);
            UserInterface.DisplayUserOptions(shots);
            if(shots.Count == 5)
            {
                UserInterface.DisplayUserOptions($"{animal.Name} is current on all shots");
            }
            else
            {
                string result = UserInterface.GetUserInput("Would you like to administer remaining shots?").ToLower();
                if (result == "yes")
                {
                    Query.GiveShots(animal, shots);
                }
            }
           
            RunUserMenus();
        }
        private void BulkAdd()
        {
            UserInterface.DisplayUserOptions("Please enter a .csv file path: ");
            string filePath = UserInterface.GetUserInput();
            CSVParse.ParseCSV(filePath);
        }
        private void MoveAnimal()
        {
            var animals = SearchForAnimal().ToList();
            if (animals.Count > 1)
            {
                UserInterface.DisplayUserOptions("Several animals found please refine your search.");
                UserInterface.DisplayAnimals(animals);
                UserInterface.DisplayUserOptions("Press enter to continue");
                Console.ReadLine();
                return;
            }
            else if (animals.Count < 1)
            {
                UserInterface.DisplayUserOptions("Animal not found please use different search criteria");
                return;
            }
            var animal = animals[0];
            Room room;
            do
            {
                int roomNumber = UserInterface.AskForRoom(animal);
                room = Query.GetRoomById(roomNumber);
            }
            while (room == default(Room));
            Room oldRoom = Query.GetRoom(animal.AnimalId);
            Query.MoveAnimal(animal, oldRoom, room);
            RunUserMenus();
        }
        private void DisplayRooms()
        {
            var rooms = Query.DisplayRooms();
            UserInterface.DisplayAnimals(rooms);
        }
        private void CheckAdoptions()
        {
            Console.Clear();
            List<string> adoptionInfo = new List<string>();
            int counter = 1;
            var adoptions = Query.GetPendingAdoptions().ToList();
            if(adoptions.Count > 0)
            {
                foreach(Adoption adoption in adoptions)
                {
                    adoptionInfo.Add($"{counter}. {adoption.Client.FirstName} {adoption.Client.LastName}, {adoption.Animal.Name} {adoption.Animal.Species}");
                    counter++;
                }
                UserInterface.DisplayUserOptions(adoptionInfo);
                UserInterface.DisplayUserOptions("Enter the number of the adoption you would like to approve");
                int input = UserInterface.GetIntegerData();
                ApproveAdoption(adoptions[input - 1]);
            }

        }
        private void ApproveAdoption(Adoption adoption)
        {
            UserInterface.DisplayAnimalInfo(adoption.Animal);
            UserInterface.DisplayClientInfo(adoption.Client);
            UserInterface.DisplayUserOptions("Would you approve this adoption?");
            if ((bool)UserInterface.GetBitData())
            {
                Query.UpdateAdoption(true, adoption);
            }
            else
            {
                Query.UpdateAdoption(false, adoption);
            }
        }

        private void CheckAnimalStatus()
        {
            Console.Clear();
            var animals = SearchForAnimal().ToList();
            if(animals.Count > 1)
            {
                UserInterface.DisplayUserOptions("Several animals found");
                UserInterface.DisplayAnimals(animals);
                UserInterface.DisplayUserOptions("Enter the ID of the animal you would like to check");
                int ID = UserInterface.GetIntegerData();
                CheckAnimalStatus(ID);
                return;
            }
            if(animals.Count == 0)
            {
                UserInterface.DisplayUserOptions("Animal not found please use different search criteria");
                return;
            }
            RunCheckMenu(animals[0]);
        }

        private void RunCheckMenu(Animal animal)
        {
            bool isFinished = false;
            Console.Clear();
            while(!isFinished){
                List<string> options = new List<string>() { "Animal found:", animal.Name, animal.Species.Name, "Would you like to:", "1. Get Info", "2. Update Info", "3. Check shots", "4. Return" };
                UserInterface.DisplayUserOptions(options);
                int input = UserInterface.GetIntegerData();
                if (input == 4)
                {
                    isFinished = true;
                    continue;
                }
                RunCheckMenuInput(input, animal);
            }
        }

        private void RunCheckMenuInput(int input, Animal animal)
        {
            
            switch (input)
            {
                case 1:
                    UserInterface.DisplayAnimalInfo(animal);
                    Console.Clear();
                    return;
                case 2:
                    UpdateAnimal(animal);
                    Console.Clear();
                    return;
                case 3:
                    CheckShots(animal);
                    Console.Clear();
                    return;
                default:
                    UserInterface.DisplayUserOptions("Input not accepted please select a menu choice");
                    return;
            }
        }

        private void CheckShots(Animal animal)
        {
            List<string> shotInfo = new List<string>();
            var shots = Query.GetShots(animal);
            foreach(AnimalShot shot in shots)
            {
                shotInfo.Add($"{shot.Shot.Name} Date: {shot.DateReceived}");
            }
            if(shotInfo.Count > 0)
            {
                UserInterface.DisplayUserOptions(shotInfo);
                if(UserInterface.GetBitData("Would you like to Update shots?"))
                {
                    Query.UpdateShot("booster", animal);
                }
            }
            else
            {
                if (UserInterface.GetBitData("Would you like to Update shots?"))
                {
                    Query.UpdateShot("booster", animal);
                }
            }
            
        }

        private void UpdateAnimal(Animal animal)
        {
            Dictionary<int, string> updates = new Dictionary<int, string>();
            List<string> options = new List<string>() { "Select Updates: (Enter number and choose finished when finished)", "1. Category", "2. Breed", "3. Name", "4. Age", "5. Demeanor", "6. Kid friendly", "7. Pet friendly", "8. Weight", "9. Finished" };
            UserInterface.DisplayUserOptions(options);
            string input = UserInterface.GetUserInput();
            if(input.ToLower() == "9" ||input.ToLower() == "finished")
            {
                Query.EnterUpdate(animal, updates);
            }
            else
            {
                updates = UserInterface.EnterSearchCriteria(updates, input);
            }
        }

        private void CheckAnimalStatus(int iD)
        {
            Console.Clear();
            var animals = SearchForAnimal(iD).ToList();
            if (animals.Count == 0)
            {
                UserInterface.DisplayUserOptions("Animal not found please use different search criteria");
                return;
            }
            RunCheckMenu(animals[0]);
        }

        private IQueryable<Animal> SearchForAnimal(int iD)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
             var animals = (from animal in context.Animals where animal.AnimalId == iD select animal);
            return animals;
        }

        private void RemoveAnimal()
        {

            var animals = SearchForAnimal().ToList();
            if (animals.Count > 1)
            {
                UserInterface.DisplayUserOptions("Several animals found please refine your search.");
                UserInterface.DisplayAnimals(animals);
                UserInterface.DisplayUserOptions("Press enter to continue");
                Console.ReadLine();
                return;
            }
            else if (animals.Count < 1)
            {
                UserInterface.DisplayUserOptions("Animal not found please use different search criteria");
                return;
            }
            var animal = animals[0];
            List<string> options = new List<string>() { "Animal found:", animal.Name, animal.Species.Name, "would you like to delete?" };
            if ((bool)UserInterface.GetBitData(options))
            {
                Query.RemoveAnimal(animal);
            }
        }
        private void AddAnimal()
        {
            Console.Clear();
            Animal animal = new Animal();
            string species = UserInterface.GetStringData("species", "the animal's");
            species = CheckSpecies(species);
            animal.Species = Query.GetSpecies(species);
            animal.Name = UserInterface.GetStringData("name", "the animal's");
            animal.Age = UserInterface.GetIntegerData("age", "the animal's");
            animal.Demeanor = UserInterface.GetStringData("demeanor", "the animal's");
            animal.KidFriendly = UserInterface.GetBitData("the animal", "child friendly");
            animal.PetFriendly = UserInterface.GetBitData("the animal", "pet friendly");
            animal.Weight = UserInterface.GetIntegerData("the animal", "the weight of the");
            string dietPlan = UserInterface.GetStringData("diet plan", "the animal's");
            dietPlan = CheckDietPlan(dietPlan);
            animal.DietPlan = Query.GetDietPlan(dietPlan);
            Query.AddAnimal(animal);
        }
        public string CheckDietPlan(string dietPlan)
        {
            if (!Query.CheckDietPlan(dietPlan))
            {
                DietPlan newDietPlan = new DietPlan();
                newDietPlan.Name = dietPlan;
                newDietPlan.FoodType = UserInterface.GetUserInput("Please input the type of food: ");
                newDietPlan.FoodAmountInCups = int.Parse(UserInterface.GetUserInput("Amount of food in Cups: "));
                Query.AddDietPlan(newDietPlan);
            }
            return dietPlan;
        }
        public string CheckSpecies(string species)
        {
            if(!Query.CheckSpecies(species))
            {
                Species newSpecies = new Species();
                newSpecies.Name = species;
                Query.AddSpecies(newSpecies);
            }  
            return species;
        }
        protected override void LogInPreExistingUser()
        {
            List<string> options = new List<string>() { "Please log in", "Enter your username (CaSe SeNsItIvE)" };
            UserInterface.DisplayUserOptions(options);
            userName = UserInterface.GetUserInput();
            UserInterface.DisplayUserOptions("Enter your password (CaSe SeNsItIvE)");
            string password = UserInterface.GetUserInput();
            try
            {
                Console.Clear();
                employee = Query.EmployeeLogin(userName, password);
                UserInterface.DisplayUserOptions("Login successfull. Welcome.");
            }
            catch
            {
                Console.Clear();
                UserInterface.DisplayUserOptions("Employee not found, please try again, create a new user or contact your administrator");
                LogIn();
            }
            
        }
        private void CreateNewEmployee()
        {
            Console.Clear();
            string email = UserInterface.GetStringData("email", "your");
            int employeeNumber = int.Parse(UserInterface.GetStringData("employee number", "your"));
            try
            {
                employee = Query.RetrieveEmployeeUser(email, employeeNumber);
            }
            catch
            {
                UserInterface.DisplayUserOptions("Employee not found please contact your administrator");
                PointOfEntry.Run();
            }
            if (employee.Password != null)
            {
                UserInterface.DisplayUserOptions("User already in use please log in or contact your administrator");
                LogIn();
                return;
            }
            else
            {
                UpdateEmployeeInfo();
            }
        }

        private void UpdateEmployeeInfo()
        {
            GetUserName();
            GetPassword();
            Query.AddUsernameAndPassword(employee);
        }

        private void GetPassword()
        {
            UserInterface.DisplayUserOptions("Please enter your password: (CaSe SeNsItIvE)");
            employee.Password = UserInterface.GetUserInput();
        }

        private void GetUserName()
        {
            Console.Clear();
            string username = UserInterface.GetStringData("username", "your");
            if (Query.CheckEmployeeUserNameExist(username))
            {
                UserInterface.DisplayUserOptions("Username already in use please try another username.");
                GetUserName();
            }
            else
            {
                employee.UserName = username;
                UserInterface.DisplayUserOptions("Username successful");
            }
        }
    }
}
