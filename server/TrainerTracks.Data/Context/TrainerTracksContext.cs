﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using TrainerTracks.Data.Model.Entity;

namespace TrainerTracks.Data.Context
{
    public class TrainerTracksContext : DbContext
    {
        public TrainerTracksContext(DbContextOptions<TrainerTracksContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("tt");
        }



        public DbSet<Trainer> Trainer { get; set; }


        public T ExecuteProcedure<T>(string procedureName, Npgsql.NpgsqlParameter[] parameters) where T : new()
        {
            T res = new T();

            using (var command = this.Database.GetDbConnection().CreateCommand())
            {
                command.Connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = string.Format("tt.\"{0}\"", procedureName);
                    command.Parameters.AddRange(parameters);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        // Completely stolen from https://www.codeproject.com/Articles/827984/Generically-Populate-List-of-Objects-from-SqlDataR
                        while (reader.Read())
                        {
                            foreach(var prop in typeof(T).GetProperties())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                                {
                                    Type convertTo = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                    prop.SetValue(res, Convert.ChangeType(reader[prop.Name], convertTo), null);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            
            return res;
        }

    }
}
