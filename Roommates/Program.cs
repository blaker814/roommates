using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;

namespace Roommates
{
    class Program
    {
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        ShowAllRooms(roomRepo);
                        break;
                    case ("Search for room"):
                        SearchForRoom(roomRepo);
                        break;
                    case ("Add a room"):
                        AddRoom(roomRepo);
                        break;
                    case ("Delete a room"):
                        DeleteRoom(roomRepo);
                        break;
                    case ("Show all chores"):
                        ShowAllChores(choreRepo);
                        break;
                    case ("Search for chore"):
                        SearchForChore(choreRepo);
                        break;
                    case ("Add a chore"):
                        AddChore(choreRepo);
                        break;
                    case ("Search for roommate"):
                        SearchForRoommate(roommateRepo);
                        break;
                    case ("Show unassigned chores"):
                        ShowUnassignedChores(choreRepo);
                        break;
                    case ("Assign chore to roommate"):
                        AssignChoreToRoommate(choreRepo, roommateRepo);
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }
        }

        private static void DeleteRoom(RoomRepository roomRepo)
        {
            List<Room> rooms = roomRepo.GetAll();
            foreach (Room r in rooms)
            {
                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
            }

            Console.Write("Room Id: ");
            int id = int.Parse(Console.ReadLine());
            roomRepo.Delete(id);
            Console.WriteLine($"Room with id {id} has been deleted");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static void ShowAllRooms(RoomRepository roomRepo)
        {
            List<Room> rooms = roomRepo.GetAll();
            foreach (Room r in rooms)
            {
                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        static void SearchForRoom(RoomRepository roomRepo)
        {
            Console.Write("Room Id: ");
            int id = int.Parse(Console.ReadLine());

            Room room = roomRepo.GetById(id);

            Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        static void AddRoom(RoomRepository roomRepo)
        {
            Console.Write("Room name: ");
            string name = Console.ReadLine();

            Console.Write("Max occupancy: ");
            int max = int.Parse(Console.ReadLine());

            Room roomToAdd = new Room()
            {
                Name = name,
                MaxOccupancy = max
            };

            roomRepo.Insert(roomToAdd);

            Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        static void ShowAllChores(ChoreRepository choreRepo)
        {
            List<Chore> chores = choreRepo.GetAll();
            foreach (Chore c in chores)
            {
                Console.WriteLine($"{c.Id} - {c.Name}");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        static void SearchForChore(ChoreRepository choreRepo)
        {
            Console.Write("Chore id: ");
            int id = int.Parse(Console.ReadLine());

            Chore chore = choreRepo.GetById(id);

            Console.WriteLine($"{chore.Id} - {chore.Name}");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        static void AddChore(ChoreRepository choreRepo)
        {
            Console.Write("Chore name: ");
            string name = Console.ReadLine();

            Chore choreToAdd = new Chore()
            {
                Name = name
            };

            choreRepo.Insert(choreToAdd);

            Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        static void SearchForRoommate(RoommateRepository roommateRepo)
        {
            Console.Write("Roommate id: ");
            int id = int.Parse(Console.ReadLine());

            Roommate roommate = roommateRepo.GetById(id);

            Console.WriteLine($"{roommate.Id} - {roommate.FirstName} Rent Portion({roommate.RentPortion}) Room Name({roommate.Room.Name})");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        static void ShowUnassignedChores(ChoreRepository choreRepo)
        {
            List<Chore> unassignedChores = choreRepo.GetUnassignedChores();
            foreach (Chore chore in unassignedChores)
            {
                Console.WriteLine($"{chore.Id} - {chore.Name}");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        static void AssignChoreToRoommate(ChoreRepository choreRepo, RoommateRepository roommateRepo)
        {
            List<Chore> chores = choreRepo.GetAll();
            List<Roommate> roommates = roommateRepo.GetAll();

            foreach (Chore c in chores)
            {
                Console.WriteLine($"{c.Id} - {c.Name}");
            }
            Console.Write("Select chore: ");
            int choreChoice = int.Parse(Console.ReadLine());

            foreach (Roommate r in roommates)
            {
                Console.WriteLine($"{r.Id} - {r.FirstName}");
            }
            Console.Write("Select roommate: ");
            int roommateChoice = int.Parse(Console.ReadLine());

            choreRepo.AssignChore(choreChoice, roommateChoice);

            Chore chore = choreRepo.GetById(choreChoice);
            Roommate roommate = roommateRepo.GetById(roommateChoice);

            Console.WriteLine($"{chore.Name} has been assigned to {roommate.FirstName}");
            Console.Write("Press any key to continue");
            Console.ReadKey();

        }
        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Delete a room",
            "Show all chores",
            "Search for chore",
            "Add a chore",
            "Search for roommate",
            "Show unassigned chores",
            "Assign chore to roommate",
            "Exit"
        };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }

        }
    }
}
