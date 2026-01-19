using Business.Libraries.ServiceResult.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class Test : AppControllerBase
    {
        private IServiceResultFactory _resultFact;

        public Test(IServiceResultFactory resultFact)
        {
            _resultFact = resultFact;
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet("testDto")]
        public async Task<object> GetDTO([FromBody]string name)
        {
            if(name == "hitler")
                throw new ApplicationException("------- TEST: Exception in API_Gateway -> TestController ---------------");

            var dto = new TestDTO { 
                Number = 123,
                Name = name,
                Data = new List<string>() { "aaa", "bbb", "ccc" }
            };

            var result = _resultFact.Result<TestDTO>(dto, true, "* result mesage *");

            return result;
        }



    }



    public class TestDTO
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public List<string> Data { get; set; }
    }
}
