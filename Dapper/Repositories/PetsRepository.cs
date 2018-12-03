using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;


namespace EjemploBestDay
{
    class PetsRepository
    {
        public List<Models.Pet> ReadAll()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (IDbConnection db = new SqlConnection(cadenaConexion))
            {
                return db.Query<Models.Pet>("SELECT * FROM Pet").ToList();
            }
        }

        public Models.Pet Find(int id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                return db.Query<Models.Pet>("SELECT * FROM Pet WHERE Id = @Id", new { Id = id }).SingleOrDefault();
            }
        }

        public List<Models.Pet> ReadSp1()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                string readSp = "GetAllPets";
                return db.Query<Models.Pet>(readSp, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public int ReadSp2()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (IDbConnection db = new SqlConnection(cadenaConexion))
            {
                var affectedRows = db.Execute("GetAllPets", commandType: CommandType.StoredProcedure);
                return affectedRows;
            }
        }

        public int ReadSp2WithParam(int id)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (IDbConnection db = new SqlConnection(cadenaConexion))
            {
                var p = new DynamicParameters();
                p.Add("Id", id);

                var affectedRows = db.Execute("GetPetById", p, commandType: CommandType.StoredProcedure);
                return affectedRows;
            }

        }


        public int Insert1(string petName, DateTime birthDate)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                string sql = "INSERT INTO Pet (Name, BirthDate,TypeId, OwnerId) Values (@name, @birth_Date,1,1);";

                var affectedRows = connection.Execute(sql, new { name = petName, birth_date = birthDate });

                return affectedRows;
            }
        }

        public int InsertMany()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                string sql = "INSERT INTO Pet (Name, BirthDate,TypeId, OwnerId) Values (@name, @birth_Date,1,1);";

                var affectedRows = connection.Execute(sql, 
                    new[]{
                        new { name = "InsertManySuke", birth_date = new DateTime(2014, 01, 08) },
                        new { name = "InsertManySuke2", birth_date = new DateTime(2015, 01, 08) }
                        }
                );

                return affectedRows;
            }
        }

        public int Update(Models.Pet pet, int id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                string sqlQuery = "UPDATE Pet SET Name = @name, BirthDate = @birthDate WHERE Id = @id";
                int rowsAffected = db.Execute(sqlQuery, new { @name = pet.Name, @birthDate = pet.BirthDate, @id=id });
                return rowsAffected;
            }
        }

        public Models.PetsTiposDTO DevolucionDTOconINNER(int id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                return db.Query<Models.PetsTiposDTO>("SELECT p.Id, p.Name, p.BirthDate, t.Id AS TypeId, t.Name AS TipoName " + 
                                                    "FROM Pet p " +
                                                    "INNER JOIN Type t ON p.TypeId = t.Id " + 
                                                    "WHERE p.Id = @Id", new { Id = id }).First();
            }
        }

        public int multiplesQueries_unrelatedEntities()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                var query = "SELECT * FROM Pet; SELECT * FROM Type";

                var results = db.QueryMultiple(query);
                // retrieve the results into the respective models
                var animales = results.Read<Models.Pet>().ToList();
                var tipos = results.Read<Models.Type>().ToList();               
                //chequear si abre dos conexiones o solo uno
                return animales.Count + tipos.Count;
            }                
        }

        public Models.TiposRelated multiplesQuerias_1ToMany(int id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                var query = "SELECT * FROM Type WHERE Id = @id; SELECT * FROM Pet WHERE TypeId = @id; ";

                var results = db.QueryMultiple(query, new { @id = id });
                // retrieve the results into the respective models
                var tipos = results.ReadSingle<Models.TiposRelated>();
                tipos.Animales = results.Read<Models.Pet>().ToList();

                return tipos;
            }
        }








    } //end class
} //end namespace
