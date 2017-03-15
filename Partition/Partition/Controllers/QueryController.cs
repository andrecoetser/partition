using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Partition.Config;
using Partition.Entities;
using Partition.Model;

namespace Partition.Controllers
{
    [Route("api/[controller]")]
    public class QueryController : Controller
    {
        private readonly DataContext _dataContext;

        public QueryController(IOptions<AppSettings> settings)
        {
            AppSettings appSettings = settings.Value;
            _dataContext = new DataContext(appSettings.DefaultConnection);
        }

        [HttpGet]
        [Route("TimeRange")]
        public async Task<IActionResult> GetTimeRange(TimeRangeRequest request)
        {
            return request.IsValid() ? (IActionResult) Json(await _dataContext.TimeRangeQuery(request)) : BadRequest();
        }

        [HttpGet]
        [Route("ProductStore")]
        public async Task<IActionResult> GetProductStore(ProductStoreRequest request)
        {
            return request.IsValid() ? (IActionResult)Json(await _dataContext.ProductStoreQuery(request)) : BadRequest();
        }
    }
}
