using System.Data.Entity;

namespace Webdiyer.MvcPagerDemo.Models
{
    public class DataContext:DbContext
    {
        public DbSet<Order> Orders { get; set; }
    }
}