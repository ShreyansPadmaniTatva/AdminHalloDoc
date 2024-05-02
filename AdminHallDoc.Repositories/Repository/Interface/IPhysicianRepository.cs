using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;

namespace AdminHalloDoc.Repositories.Admin.Repository.Interface
{
    public interface IPhysicianRepository
    {
        Task<List<PhysicianLocation>> FindPhysicianLocation();
        Task<List<Physicians>> PhysicianAll();
        Task<List<Physicians>> PhysicianByRegion(int? region);
        Task<bool> ChangeNotificationPhysician(Dictionary<int, bool> changedValuesDict);
        Task<bool> PhysicianAddEdit(Physicians physiciandata, string AdminId);
        Task<Physicians> GetPhysicianById(int id);
        Task<bool> SavePhysicianInfo(Physicians vm);
        Task<bool> ChangePasswordAsync(string password, int Physicianid);
        Task<bool> EditAdminInfo(Physicians vm);
        Task<bool> DeletePhysician(int PhysicianID, string AdminID);
        Task<bool> EditMailBilling(Physicians vm, string AdminId);
        Task<bool> EditProviderProfile(Physicians vm, string AdminId);
        Task<bool> EditProviderOnbording(Physicians vm, string AdminId);
        Task<bool> GetLocation(int PhysicianId);
        List<Physician> isProviderEmailExist(string Email);
        List<AdminHalloDoc.Entities.Models.Admin> isAdminEmailExist(string Email);
        Task<List<PhysicianPayrate>> PhysicianPayrate(int PhysicianId);
        Task<bool> SavePayrate(int PayrateId, decimal? Payrate, string AdminId);
    }
}
