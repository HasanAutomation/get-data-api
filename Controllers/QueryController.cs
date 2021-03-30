using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BlogPostApi.Controllers
{
    [Route("api/[controller]")]
    public class QueryController : ControllerBase
    {
        public QueryController(AppDb db)
        {
            Db = db;
        }

        // GET api/query
        [HttpGet("queryString")]
        public async Task<IActionResult> GetResults(string request)
        {
            await Db.Connection.OpenAsync();

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = request;
            List<Dictionary<string, string>> jsonLists = new List<Dictionary<string, string>>();
            Dictionary<string, string> vals;
            var reader = await cmd.ExecuteReaderAsync();
            using (reader)
            {

                while (await reader.ReadAsync())
                {
                    vals = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        vals.Add(reader.GetName(i).ToString(), reader.GetValue(i).ToString());


                    }
                    jsonLists.Add(vals);
                }

            }
            string json = JsonConvert.SerializeObject(jsonLists);

            return Ok(json);
        }

        public AppDb Db { get; }
    }
}