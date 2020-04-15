using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ItLabs.MultiTenant.Api.Controllers
{
    /// <summary>
    /// Controller used to showcase the different tasks per tenant
    /// </summary>
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

        [HttpPost]
        public int Post(Task task)
        {
            _dbContext.Tasks.Add(task);
            var taskId = _dbContext.SaveChanges();
            return taskId;
        }
    }
}
