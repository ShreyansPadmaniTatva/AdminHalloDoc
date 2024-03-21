﻿using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface ISchedulingRepository
    {
        Task<List<Physicians>> PhysicianAll();
        Task<List<Physicians>> PhysicianByRegion(int? region);
        Task<bool> CreateShift(Schedule s, string AdminID);
        Task<List<Schedule>> GetShift(int month);
    }
}
