using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploBestDay
{
    class Program
    {
        static void Main(string[] args)
        {
            var petsRepo = new PetsRepository();

            //--------------------Muestra todos los elementos
            //var animales = petsRepo.ReadAll();
            //foreach (var animal in animales)
            //{
            //    Console.WriteLine(animal.Id + ": " + animal.Name);
            //}



            //--------------------busca un elemento por Id
            //var animal1 = petsRepo.Find(1);
            //Console.WriteLine(animal1.Id + ": " + animal1.Name);



            //--------------------insercción simple
            //var filasInsertadas = petsRepo.Insert1("Snoopy", new DateTime(2000, 1, 1));
            //Console.WriteLine("Número de registro insertados: " + filasInsertadas);



            //--------------------llamada a un SP que devuelve una lista (db.Query)
            //var animales = petsRepo.ReadAnimalesFromSpUsingDbQuery();
            //foreach (var animal in animales)
            //{
            //    Console.WriteLine(animal.Id + ": " + animal.Name);
            //}

            //-------------------llamada a un SP que devuelve un unico animal usando su id
            //var animal = petsRepo.GetAnimalByIdFromSP_UsingDynamicParams(1);
            //Console.WriteLine(animal.Id + ": " + animal.Name);



            //--------------------insercción múltiple
            //var filasInsertadasMany = petsRepo.InsertMany();    //La pena es que esto ejecuta 1 insert x valor (enseñar capturas de Profiler 16/01 a las 00:18 aprox)
            //Console.WriteLine("Número de registro insertados con el InsertMany: " + filasInsertadasMany);



            //--------------------updatear un elemento. El borrar es de la misma manera
            //var nuevosValores = new Models.Pet();
            //nuevosValores.Name = "1538";
            //nuevosValores.BirthDate = new DateTime(2090, 1, 1);
            //var filasUpdateadas = petsRepo.Update(nuevosValores, 6);
            //Console.WriteLine("Número de registro updateados: " + filasUpdateadas);



            //--------------------update con parameters
            //var nuevosValores = new Models.Pet
            //{
            //    Id = 1,
            //    Name = "16Enero2019-TechSession_0947",
            //    BirthDate = new DateTime(2090, 1, 1)
            //};
            //var filasUpdateadas = petsRepo.UpdateSpWithParameters(nuevosValores);
            //var resultado = filasUpdateadas > 0 ? "Fila updateada" : "Fila NO updateada";
            //Console.WriteLine(resultado);



            //--------------------Select con INNER JOIN
            //var devolucion = petsRepo.DevolucionDTOconINNER(1);
            //Console.WriteLine(devolucion.Name + " " + devolucion.TipoName);



            //--------------------Multi-queries: relaciones independientes 
            //var numeroDeFilas = petsRepo.MultiplesQueries_unrelatedEntities();
            //Console.WriteLine("Número de registro en Pet + Tipos: " + numeroDeFilas);



            //--------------------Multi-queries: relaciones 1-N
            //var objetoDevuelto = petsRepo.MultiplesQuerias_1ToMany(1);
            //Console.WriteLine("Objeto 1 con sus relaciones 1-N");
            //Console.WriteLine(objetoDevuelto.Id + " " + objetoDevuelto.Name);
            //Console.WriteLine("");
            //Console.WriteLine("Animales");
            //foreach (var tipo in objetoDevuelto.Animales)
            //{
            //    Console.WriteLine(tipo.Name);
            //}



            //--------------------Resultados: Multi-mapping
            var lista = petsRepo.MultiMappingOneToMany();
            foreach (var animal in lista)
            {
                Console.WriteLine(animal.Id + ": " + animal.Name);
            }


            Console.ReadKey();
        }
    }
}
