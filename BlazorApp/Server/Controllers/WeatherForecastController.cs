using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        [HttpGet]
        public IEnumerable<PersonasModel> Get()
        {

            var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .Build();
            var connectionString = configuration.GetSection("MiConn").Value;

            using (SqlConnection bdSql = new SqlConnection("Server=MARCELO-NB\\OCTUBRE; Database=Contactos; Integrated Security=true"))

            {

                using (SqlCommand bdComando = new SqlCommand("sp_ObtenerPersonas", bdSql))

                {

                    bdComando.CommandType = CommandType.StoredProcedure;
                    //bdComando.Parameters.Add(new SqlParameter("@vIdCliente", idCliente));
                    var Contactos = new List<PersonasModel>();
                    bdSql.Open();

                    using (var recordset = bdComando.ExecuteReader())
                    {

                        if (recordset.HasRows)
                        {
                            using (var dt = new DataTable())
                            {
                                dt.Load(recordset);

                                List<PersonasModel> target = dt.AsEnumerable()
                                    .Select(row => new PersonasModel
                                    {
                                        Id = row.Field<int?>(0).GetValueOrDefault(),

                                        Nombre = row.Field<string>(1),
                                        Apellido = row.Field<string>(2)



                                    }).ToList();
                                return target;
                            }
                        }
                        else
                        {
                            throw new Exception("Error: ");
                        }

                        //while (await recordset.ReadAsync())
                        //{
                        //    // asignamos los valores del recordset mediante un
                        //    // método en el que formateamos los valores recibidos
                        //    Contactos.Add(recordset.GetInt16("Id"));
                        //}
                    }



                }

            }



            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new PersonasModel
            //{
            //    Id = 1,
            //    Nombre = "AAA",
            //    Apellido = "UUU"
            //})
            //.ToArray();
        }
    }

    //[HttpGet("servicioscontratadosADO/{idCliente}")]
    //public async Task GetConADO()
    //{
    //    var configuration = new ConfigurationBuilder()
    //    .SetBasePath(Directory.GetCurrentDirectory())
    //    .AddJsonFile("appsettings.json", false)
    //    .Build();
    //    var connectionString = configuration.GetSection("MiConn").Value;

    //    using (SqlConnection bdSql = new SqlConnection(connectionString))

    //    {

    //        using (SqlCommand bdComando = new SqlCommand("sp_ObtenerPersonas", bdSql))

    //        {

    //            bdComando.CommandType = CommandType.StoredProcedure;
    //            //bdComando.Parameters.Add(new SqlParameter("@vIdCliente", idCliente));
    //            var Contactos = new List<PersonasModel>();
    //            await bdSql.OpenAsync();

    //            using (var recordset = await bdComando.ExecuteReaderAsync())
    //            {

    //                if (recordset.HasRows)
    //                {
    //                    using (var dt = new DataTable())
    //                    {
    //                        dt.Load(recordset);

    //                        List<PersonasModel> target = dt.AsEnumerable()
    //                            .Select(row => new PersonasModel
    //                            {
    //                                Id = row.Field<int?>(0).GetValueOrDefault(),

    //                                Nombre = row.Field<string>(1),
    //                                Apellido = row.Field<string>(2)
                                    


    //                            }).ToList();
    //                        return target;
    //                    }
    //                }
    //                else
    //                {
    //                    throw new Exception("Error: ");
    //                }

    //                //while (await recordset.ReadAsync())
    //                //{
    //                //    // asignamos los valores del recordset mediante un
    //                //    // método en el que formateamos los valores recibidos
    //                //    Contactos.Add(recordset.GetInt16("Id"));
    //                //}
    //            }

                

    //        }

    //    }

    //}

}
