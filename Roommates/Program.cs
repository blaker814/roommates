﻿using System;
using System.Collections.Generic;
using Roommates.Repositories;
using Roommates.Models;
using System.Linq;

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
                    case ("Search for roommate"):
                        SearchForRoommate(roommateRepo);
                        break;
                    case ("Show all rooms"):
                        ShowAllRooms(roomRepo);
                        break;
                    case ("Search for room"):
                        SearchForRoom(roomRepo);
                        break;
                    case ("Add a room"):
                        AddRoom(roomRepo);
                        break;
                    case ("Update a room"):
                        UpdateRoom(roomRepo);
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
                    case ("Update a chore"):
                        UpdateChore(choreRepo);
                        break;
                    case ("Delete a chore"):
                        DeleteChore(choreRepo);
                        break;
                    case ("Show unassigned chores"):
                        ShowUnassignedChores(choreRepo);
                        break;
                    case ("Assign chore to roommate"):
                        AssignChoreToRoommate(choreRepo, roommateRepo);
                        break;
                    case ("Reassign chore"):
                        ReassignChore(choreRepo, roommateRepo);
                        break;
                    case ("List chore counts"):
                        ListChoreCounts(choreRepo);
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }
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
        private static void UpdateRoom(RoomRepository roomRepo)
        {
            List<Room> roomOptions = roomRepo.GetAll();
            foreach (Room r in roomOptions)
            {
                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
            }

            Console.Write("Which room would you like to update? ");
            int selectedRoomId = int.Parse(Console.ReadLine());
            Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

            Console.Write("New Name: ");
            selectedRoom.Name = Console.ReadLine();

            Console.Write("New Max Occupancy: ");
            selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

            roomRepo.Update(selectedRoom);

            Console.WriteLine($"Room has been successfully updated");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        private static void DeleteRoom(RoomRepository roomRepo)
        {
            List<Room> rooms = roomRepo.GetAll();
            foreach (Room r in rooms)
            {
                Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
            }

            Console.Write("Which room would you lke to delete? ");
            int id = int.Parse(Console.ReadLine());
            roomRepo.Delete(id);

            Console.WriteLine($"Room with id {id} has been deleted");
            Console.WriteLine("Press any key to continue");
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
        private static void UpdateChore(ChoreRepository choreRepo)
        {
            List<Chore> choreOptions = choreRepo.GetAll();
            foreach (Chore c in choreOptions)
            {
                Console.WriteLine($"{c.Id} - {c.Name}");
            }

            Console.Write("Which chore would you like to update? ");
            int selectedChoreId = int.Parse(Console.ReadLine());
            Chore selectedChore = choreOptions.FirstOrDefault(c => c.Id == selectedChoreId);

            Console.Write("New Name: ");
            selectedChore.Name = Console.ReadLine();

            choreRepo.Update(selectedChore);

            Console.WriteLine($"Chore has been successfully updated");
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        private static void DeleteChore(ChoreRepository choreRepo)
        {
            List<Chore> choreOptions = choreRepo.GetAll();
            foreach (Chore c in choreOptions)
            {
                Console.WriteLine($"{c.Id} - {c.Name}");
            }

            Console.Write("Which chore would you like to delete? ");
            int id = int.Parse(Console.ReadLine());

            choreRepo.Delete(id);

            Console.WriteLine($"Chore with id {id} has been deleted");
            Console.WriteLine("Press any key to continue");
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
        private static void ReassignChore(ChoreRepository choreRepo, RoommateRepository roommateRepo)
        {
            List<Chore> assignedChores = choreRepo.GetAssignedChores();
            foreach (Chore chore in assignedChores)
            {
                Console.WriteLine($"{chore.Id} - {chore.Name}");
            }
            Console.Write("Chore to reassign: ");
            int choreSelection = int.Parse(Console.ReadLine());

            Roommate roommateChore = roommateRepo.GetRoommateChoreById(choreSelection);
            Console.WriteLine($"This chore is currently assigned to {roommateChore.FirstName}. Who would you like to assign it to?");

            List<Roommate> roommates = roommateRepo.GetAll();
            foreach (Roommate roommate in roommates)
            {
                Console.WriteLine($"{roommate.Id} - {roommate.FirstName}");
            }
            Console.Write("Roommate to be assigned chore: ");
            int roommateSelection = int.Parse(Console.ReadLine());

            choreRepo.ReassignChore(choreSelection, roommateSelection, roommateChore.Id);

            Roommate roommateAssigned = roommateRepo.GetById(roommateSelection);

            Console.WriteLine($"Chore successfully reassigned to {roommateAssigned.FirstName}");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        private static void ListChoreCounts(ChoreRepository choreRepo)
        {
            List<ChoreCount> choreCounts = choreRepo.GetChoreCounts();
            foreach (ChoreCount choreCount in choreCounts)
            {
                Console.WriteLine($"{choreCount.RoommateName}: {choreCount.Count}");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
        {
            "Search for roommate",
            "Show all rooms",
            "Search for room",
            "Add a room",
            "Delete a room",
            "Show all chores",
            "Search for chore",
            "Add a chore",
            "Update a chore",
            "Delete a chore",
            "Show unassigned chores",
            "Assign chore to roommate",
            "Reassign chore",
            "List chore counts",
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
