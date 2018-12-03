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

            //Mostrar todos los elementos
            //var animales = petsRepo.ReadAll();
            //foreach (var animal in animales)
            //{
            //    Console.WriteLine(animal.Id + ": " + animal.Name);
            //}
            //Console.ReadKey();

            //buscar un elemento por Id
            //var animal1 = petsRepo.Find(1);
            //Console.WriteLine(animal1.Id + ": " + animal1.Name);
            //Console.ReadKey();

            //insertar simple
            //var filasInsertadas = petsRepo.Insert1("Snoopy", new DateTime(2000, 1, 1));
            //Console.WriteLine("Número de registro insertados: " + filasInsertadas);
            //Console.ReadKey();

            //insercción múltiple
            //var filasInsertadasMany = petsRepo.InsertMany();
            //Console.WriteLine("Número de registro insertados con el InsertMany: " + filasInsertadasMany);
            //Console.ReadKey();

            //updatear un elemento. Borrar de la misma manera
            //var nuevosValores = new Models.Pet();
            //nuevosValores.Name = "1538";
            //nuevosValores.BirthDate = new DateTime(2090, 1, 1);
            //var filasUpdateadas = petsRepo.Update(nuevosValores, 6);
            //Console.WriteLine("Número de registro updateados con el InsertMany: " + filasUpdateadas);
            //Console.ReadKey();

            //update con parameters


            //Select con INNER JOIN
            //var devolucion = petsRepo.DevolucionDTOconINNER(1);
            //Console.WriteLine(devolucion.Name + " " + devolucion.TipoName);
            //Console.ReadKey();


            //relaciones independientes 
            //var numeroDeFilas = petsRepo.multiplesQueries_unrelatedEntities();
            //Console.WriteLine("Número de registro en Pet + Tipos: " + numeroDeFilas);
            //Console.ReadKey();

            //relaciones 1-N
            var objetoDevuelto = petsRepo.multiplesQuerias_1ToMany(1);
            Console.WriteLine("Objeto 1 con sus relaciones 1-N");
            Console.WriteLine(objetoDevuelto.Id + " " + objetoDevuelto.Name);
            Console.WriteLine("");
            Console.WriteLine("Tipos");
            foreach (var tipo in objetoDevuelto.Animales)
            {
                Console.WriteLine(tipo.Name);
            }

            Console.ReadKey();


            //Hacer un INNER JOIN de todos los tipos con sus correspondientes animales



            //Ver como montar UnitTest con Dapper - Ejemplo
        }
    }
}
