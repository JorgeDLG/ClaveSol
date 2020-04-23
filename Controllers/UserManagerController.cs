using Microsoft.AspNetCore.Mvc;

namespace ClaveSol.Controllers
{
    public class UserManagerController : Controller
    {
        private AppDbContext db = null;

        public UserManagerController(AppDbContext db)
        {
            this.db = db;
        }
    }
}