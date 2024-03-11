using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IPhysicianRepository
    {
        Task<List<PhysicianLocation>> FindPhysicianLocation();
        Task<List<Physicians>> PhysicianAll();
        Task<List<Physicians>> PhysicianByRegion(int? region);
        Task<bool> ChangeNotificationPhysician(Dictionary<int, bool> changedValuesDict);
    }
}
