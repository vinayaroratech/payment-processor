using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Payments.API.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        // GET: api/<PaymentController>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<PaymentController>/5
        [Route("{id}")]
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        // POST api/<PaymentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/<PaymentController>/5
        [Route("{id}")]
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<PaymentController>/5
        [Route("{id}")]
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
