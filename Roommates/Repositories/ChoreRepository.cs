﻿using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Roommates.Models;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> chores = new List<Chore>();
                    while(reader.Read())
                    {
                        int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                        string nameValue = reader.GetString(reader.GetOrdinal("Name"));
                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };
                        chores.Add(chore);
                    }
                    reader.Close();

                    return chores;
                }
            }
        }
        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;

                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }
                    reader.Close();

                    return chore;
                }
            }
        }
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                                OUTPUT INSERTED.Id
                                                VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }
        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, Name, RoommateId
                                        FROM Chore c
                                        LEFT JOIN RoommateChore ON ChoreId = c.Id
                                        WHERE RoommateId IS NULL";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Chore> unassignedChores = new List<Chore>();

                    while(reader.Read())
                    {
                        int choreId = reader.GetInt32(reader.GetOrdinal("Id"));
                        string choreName = reader.GetString(reader.GetOrdinal("Name"));

                        Chore chore = new Chore
                        {
                            Id = choreId,
                            Name = choreName
                        };

                        unassignedChores.Add(chore);
                    }
                    reader.Close();

                    return unassignedChores;
                }
            }
        }
        public void AssignChore(int choreId, int roommateId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (ChoreId, RoommateId)
                                                VALUES (@choreId, @roommateId)";
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                        SET Name = @name,
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM RoommateChore WHERE ChoreId = @id;
                                        DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<ChoreCount> GetChoreCounts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT r.FirstName, COUNT(j.RoommateId) AS ChoreCount
                                        FROM RoommateChore j 
                                        JOIN Roommate r ON j.RoommateId = r.Id
                                        GROUP BY r.FirstName";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ChoreCount> choreCounts = new List<ChoreCount>();
                    while(reader.Read())
                    {
                        string roommateName = reader.GetString(reader.GetOrdinal("FirstName"));
                        int count = reader.GetInt32(reader.GetOrdinal("ChoreCount"));
                        ChoreCount choreChount = new ChoreCount
                        {
                            RoommateName = roommateName,
                            Count = count
                        };
                        choreCounts.Add(choreChount);
                    }
                    reader.Close();

                    return choreCounts;
                }
            }
        }
        public List<Chore> GetAssignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Chore.Id, Name
                                        FROM Chore
                                        JOIN RoommateChore ON Chore.Id = ChoreId
                                        ORDER BY Chore.Id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Chore> assignedChores = new List<Chore>();
                    while (reader.Read())
                    {
                        int choreId = reader.GetInt32(reader.GetOrdinal("Id"));
                        string choreName = reader.GetString(reader.GetOrdinal("Name"));
                        Chore chore = new Chore
                        {
                            Id = choreId,
                            Name = choreName
                        };
                        assignedChores.Add(chore);
                    }
                    reader.Close();

                    return assignedChores;
                }
            }
        }
        public void ReassignChore(int choreId, int roommateId, int assignedId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE RoommateChore
                                        SET RoommateId = @rmId,
                                            ChoreId = @choreId
                                        WHERE ChoreId = @choreId AND RoommateId = @assignedId";
                    cmd.Parameters.AddWithValue("@rmId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    cmd.Parameters.AddWithValue("@assignedId", assignedId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
