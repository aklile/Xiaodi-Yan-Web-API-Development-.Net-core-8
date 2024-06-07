using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController(IConfiguration configuration) : ControllerBase
    {
        [HttpGet]
        [Route("my-key")]
        public ActionResult GetMyKey()
        {
            var myKey = configuration["MyKey"];
            return Ok(myKey);
        }
        [HttpGet]
        [Route("database-configuration")]
        public ActionResult GetDatabaseConfiguration()
        {
            var type = configuration["Databases:System:Type"];
            var connectionString = configuration["Databases:System:ConnectionString"];
            var type1 = configuration["Database:Type"];
            var connectionString1 = configuration["Database:ConnectionString"];
            return Ok(new
            {
                TypeNested = type,
                ConnectionStringNested = connectionString,
                Type = type1,
                ConnectionString = connectionString1
            });
        }
        [HttpGet]
        [Route("database-configuration-with-bind")]
        public ActionResult GetDatabaseConfigurationWithBind()
        {
            var databaseOption = new DatabaseOptions();
            // The `SectionName` is defined in the `DatabaseOption` class, 
            //which shows the section name in the `appsettings.json` file.
            configuration.GetSection($"{DatabaseOptions.SectionName}:{DatabaseOptions.SystemDatabaseSectionName}").
            Bind(databaseOption);
            // You can also use the code below to achieve the same result
            // configuration.Bind(DatabaseOption.SectionName, databaseOption);
            return Ok(new
            {
                databaseOption.Type,
                databaseOption.
            ConnectionString
            });
        }
        [HttpGet]
        [Route("database-configuration-with-generic-type")]
        public ActionResult GetDatabaseConfigurationWithGenericType()
        {
            var databaseOption = configuration.GetSection($"{DatabaseOptions.
         SectionName}:{DatabaseOptions.SystemDatabaseSectionName}").Get<DatabaseOptions>();
            return Ok(new
            {
                databaseOption.Type,
                databaseOption.
         ConnectionString
            });
        }
        /*[HttpGet]
        [Route("database-configuration-with-ioptions")]
        public ActionResult GetDatabaseConfigurationWithIOptions([FromServices] IOptions<DatabaseOptions> options)
        {
            *//*var databaseOption = options.Value;
            return Ok(new
            {
                databaseOption.Type,
                databaseOption.
            ConnectionString
            });*//*
           
        }*/

        [HttpGet]
        [Route("database-configuration-with-ioptions-snapshot")]
        public ActionResult GetDatabaseConfigurationWithIOptionsSnapshot([FromServices] IOptionsSnapshot<DatabaseOptions> options)
        {
            /* var databaseOption = options.Value;
             return Ok(new
             {
                 databaseOption.Type,
                 databaseOption.
             ConnectionString
             });*/
            var systemDatabaseOption = options.Get(DatabaseOptions.SystemDatabaseSectionName);
            var businessDatabaseOption = options.Get(DatabaseOptions.BusinessDatabaseSectionName);
            return Ok(new
            {
                SystemDatabaseOption = systemDatabaseOption,
                BusinessDatabaseOption = businessDatabaseOption
            });
        }
        [HttpGet]
        [Route("database-configuration-with-ioptions-monitor")]
        public ActionResult
GetDatabaseConfigurationWithIOptionsMonitor([FromServices]
IOptionsMonitor<DatabaseOptions> options)
        {
            var systemDatabaseOption = options.Get(DatabaseOptions.SystemDatabaseSectionName);
            var businessDatabaseOption = options.Get(DatabaseOptions.BusinessDatabaseSectionName);
            return Ok(new
            {
                SystemDatabaseOption = systemDatabaseOption,
                BusinessDatabaseOption = businessDatabaseOption
            });
            /*  var databaseOption = options.CurrentValue;
              return Ok(new
              {
                  databaseOption.Type,
                  databaseOption.
           ConnectionString
              });*/
        }
    }

}
