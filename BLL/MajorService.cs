﻿using DAL.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MajorService
    {
        public List<Major> GetAllByfaculty(int facultyId)
        {
            StudentContextDB context = new StudentContextDB();
            return context.Majors.Where(p => p.FacultyID == facultyId).ToList();
        }
    }
}
