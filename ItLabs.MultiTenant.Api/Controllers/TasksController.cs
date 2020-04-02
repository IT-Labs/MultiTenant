using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MultiTenant.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TasksDbContext _dbContext;

        public TasksController(TasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Task> Get()
        {
            var tasks = _dbContext.Tasks.ToList();
            return tasks;
        }
    }
}
