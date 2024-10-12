using DAL.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FacultyService
    {
        public List<Faculty> GeAll()
        {
            StudentContextDB context = new StudentContextDB();
            return context.Faculties.ToList();
        }
    }
}
