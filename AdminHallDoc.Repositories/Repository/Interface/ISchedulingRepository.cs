using AdminHalloDoc.Entities.Models;
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
        Task<List<Schedule>> PhysicianAll();
        Task<List<Schedule>> PhysicianByRegion(int? region);
        Task<bool> CreateShift(Schedule s, string AdminID);
        Task<List<Schedule>> GetShift(int month, int? region);
        Task<Schedule> GetShiftByShiftdetailId(int Shiftdetailid);
        Task<bool> EditShift(Schedule s, string AdminID);
        Task<bool> UpdateStatusShift(string s, string AdminID);
        Task<bool> DeleteShift(string s, string AdminID);
        Task<List<Schedule>> GetAllNotApprovedShift(int? regionId);
        Task<List<Physicians>> PhysicianOnCall(int? region);
    }
}
