﻿using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

            //modelBuilder.Entity<TrainerClients>()
            //    .HasOne(p => p.Client)
            //    .WithMany(b => b.TrainerClients)
            //    .HasForeignKey(p => p.ClientId);

            //modelBuilder.Entity<TrainerClients>()
            //    .HasOne(p => p.Trainer)
            //    .WithMany(b => b.TrainerClients)
            //    .HasForeignKey(p => p.TrainerId);
        }
        
        public DbSet<Trainer> Trainer { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<TrainerClients> TrainerClients { get; set; }

        public T ExecuteProcedure<T>(string procedureName, params object[] parameters) where T : new()
        {
            T res = new T();

            using (var command = this.Database.GetDbConnection().CreateCommand())
            {
                command.Connection.Open();
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = string.Format("tt.\"{0}\"", procedureName);
                    command.Parameters.AddRange(this.GenerateParameters(parameters));
                    
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

        private NpgsqlParameter[] GenerateParameters(object[] parameters)
        {
            IList<NpgsqlParameter> postgreSqlParams = new List<NpgsqlParameter>();

            foreach(var parameter in parameters)
            {
                postgreSqlParams.Add(new NpgsqlParameter() { Value = parameter });
            }

            return postgreSqlParams.ToArray();
        }

    }
}
