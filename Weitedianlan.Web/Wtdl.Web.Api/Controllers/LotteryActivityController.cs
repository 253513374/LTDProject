using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wtdl.Web.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LotteryActivityController : ControllerBase
    {
        // GET: api/<LotteryActivityController>
        [HttpGet]
        public IEnumerable<string> GetLotteryActivity()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/<LotteryActivityController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<LotteryActivityController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<LotteryActivityController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<LotteryActivityController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}