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

        public List<Models.Pet> ReadAnimalesFromSpUsingDbQuery()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                return db.Query<Models.Pet>("GetAllPets", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Models.Pet GetAnimalByIdFromSP_UsingDynamicParams(int id)
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (IDbConnection db = new SqlConnection(cadenaConexion))
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);

                var pet = db.Query<Models.Pet>("GetPetById", p, commandType: CommandType.StoredProcedure).First();
                return pet;
            }

        }

        public int UpdateSpWithParameters(Models.Pet pet)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@Id", pet.Id, DbType.Int32, ParameterDirection.Input);
                parameter.Add("@Name", pet.Name, DbType.String, ParameterDirection.Input);
                parameter.Add("@NumRowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

                db.Execute("UpdateNameOfPetById", parameter, commandType: CommandType.StoredProcedure);

                int rowCount = parameter.Get<int>("@NumRowsAffected");
                return rowCount;
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

        public int MultiplesQueries_unrelatedEntities()
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

        public Models.TiposRelated MultiplesQuerias_1ToMany(int id)
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

        public List<Models.PetMapping> MultiMappingOneToMany()
        {
            string sql = "SELECT * FROM [PetClinic].[dbo].[Pet] AS p " +
                "INNER JOIN [PetClinic].[dbo].[Type] AS t ON p.TypeId = t.Id;";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
            {
                var petsDictionary = new Dictionary<int, Models.PetMapping>();

                var lista = db.Query<Models.PetMapping, Models.Type, Models.PetMapping>(
                    sql,
                    (pet, tipo) =>
                    {
                        Models.PetMapping entrada;

                        if (!petsDictionary.TryGetValue(pet.Id, out entrada))
                        {
                            entrada = pet;
                            entrada.Tipos = new List<Models.Type>();
                            petsDictionary.Add(entrada.Id, entrada);
                        }

                        entrada.Tipos.Add(tipo);
                        return entrada;
                    },
                    splitOn: "Id")
                    .Distinct()
                    .ToList();

                return lista;
            }
        }








    } //end class
} //end namespace
    