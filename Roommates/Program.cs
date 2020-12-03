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
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }
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
        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Show all rooms",
            "Search for room",
            "Add a room",
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
