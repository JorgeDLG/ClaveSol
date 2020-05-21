using System.Collections.Generic;
using ClaveSol.Models;

namespace ClaveSol.Controllers
{
    public interface ICategoryService
    {
         List<Category> GetAllCategories();
    }
}