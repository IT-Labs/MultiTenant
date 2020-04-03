namespace ItLabs.MultiTenant.Api
{
    /// <summary>
    /// Task Entity
    /// </summary>
    public class Task
    {
        /// <summary>
        /// The task Id (identifier)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The task name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The task description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The task flag for marking it done
        /// </summary>
        public bool IsDone { get; set; }
    }
}
