using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminHalloDoc.Repositories.Admin.Repository
{
    public class ViewNotesRepository : IViewNotesRepository
    {
        #region Constructor 
        private readonly EmailConfiguration _emailConfig;
        private readonly ApplicationDbContext _context;
        public ViewNotesRepository(ApplicationDbContext context, EmailConfiguration emailConfig)
        {
            _context = context;
            _emailConfig = emailConfig;
        }
        #endregion

        #region GetDocumentByRequest
        public async Task<ViewNotesModel> GetNotesByRequest(int id)
        {
            var req = _context.Requests.FirstOrDefault(W => W.Requestid == id);

            var rsa = (from rs in _context.Requeststatuslogs
                       join py in _context.Physicians on rs.Physicianid equals py.Physicianid into pyGroup
                       from py in pyGroup.DefaultIfEmpty()
                       join p in _context.Physicians on rs.Transtophysicianid equals p.Physicianid into pGroup
                       from p in pGroup.DefaultIfEmpty()
                       join a in _context.Admins on rs.Adminid equals a.Adminid into aGroup
                       from a in aGroup.DefaultIfEmpty()
                       where rs.Requestid == id
                       select new TransfernotesModel
                       {
                           TransPhysician = p.Firstname,
                           Admin = a.Firstname,
                           Physician = py.Firstname,
                           Requestid = rs.Requestid,
                           Notes = rs.Notes,
                           Status = rs.Status,
                           Physicianid = rs.Physicianid,
                           Createddate = rs.Createddate,
                           Requeststatuslogid = rs.Requeststatuslogid,
                           Transtoadmin = rs.Transtoadmin,
                           Transtophysicianid = rs.Transtophysicianid

                       }).ToList();

            var requestlog = _context.Requeststatuslogs.Where(E => E.Requestid == id && E.Physicianid != null);
            var cancelnotes = _context.Requeststatuslogs.Where(E => E.Requestid == id && ((E.Status == 3) || (E.Status == 7) || (E.Status == 8)));
            var model = _context.Requestnotes.FirstOrDefault(E => E.Requestid == id);
            ViewNotesModel allData = new ViewNotesModel();
            if (model == null)
            {
                allData.Requestid = id;

                allData.Physiciannotes = "-";
                allData.Administrativenotes = "-";
                allData.Adminnotes = "-";
            }
            else
            {
                allData.Status = req.Status;
                allData.Requestid = id;
                allData.Requestnotesid = model.Requestnotesid;
                allData.Physiciannotes = model.Physiciannotes;
                allData.Administrativenotes = model.Administrativenotes;
                allData.Adminnotes = model.Adminnotes;
            }

            List<TransfernotesModel> list = new List<TransfernotesModel>();
            foreach (var e in cancelnotes)
            {
                list.Add(new TransfernotesModel
                {

                    Requestid = e.Requestid,
                    Notes = e.Notes,
                    Status = e.Status,
                    Physicianid = e.Physicianid,
                    Createddate = e.Createddate,
                    Requeststatuslogid = e.Requeststatuslogid,
                    Transtoadmin = e.Transtoadmin,
                    Transtophysicianid = e.Transtophysicianid

                });
            }
            allData.cancelnotes = list;

            List<TransfernotesModel> md = new List<TransfernotesModel>();
            foreach (var e in rsa)
            {
                md.Add(new TransfernotesModel
                {
                    TransPhysician = e.TransPhysician,
                    Admin = e.Admin,
                    Physician = e.Physician,
                    Requestid = e.Requestid,
                    Notes = e.Notes,
                    Status = e.Status,
                    Physicianid = e.Physicianid,
                    Createddate = e.Createddate,
                    Requeststatuslogid = e.Requeststatuslogid,
                    Transtoadmin = e.Transtoadmin,
                    Transtophysicianid = e.Transtophysicianid
                });
            }
            allData.transfernotes = md;
            return allData;

        }
        #endregion

        #region PutNotes 

        public bool PutNotes(string? adminnotes, string? physiciannotes, int? RequestID, string Id)
        {
            try
            {
                Requestnote notes = _context.Requestnotes.FirstOrDefault(E => E.Requestid == RequestID);
                if (physiciannotes != null)
                {
                    if (notes != null)
                    {

                        notes.Physiciannotes = physiciannotes;
                        notes.Modifieddate = DateTime.Now;
                        notes.Modifiedby = Id;

                        _context.Requestnotes.Update(notes);
                        _context.SaveChangesAsync();
                        return true;
                    }
                    else

                    {
                        Requestnote rn = new Requestnote
                        {
                            Requestid = (int)RequestID,
                            Physiciannotes = physiciannotes,
                            Createddate = DateTime.Now,
                            Createdby = Id
                        };
                        _context.Requestnotes.Add(rn);
                        _context.SaveChangesAsync();
                        return true;
                    }
                }
                else if (adminnotes != null)
                {
                    if (notes != null)
                    {

                        notes.Adminnotes = adminnotes;
                        notes.Modifieddate = DateTime.Now;
                        notes.Modifiedby = Id;

                        _context.Requestnotes.Update(notes);
                        _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        Requestnote rn = new Requestnote
                        {
                            Requestid = (int)RequestID,
                            Adminnotes = adminnotes,
                            Createddate = DateTime.Now,
                            Createdby = Id
                        };
                        _context.Requestnotes.Add(rn);
                        _context.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region FindVenderByVenderType
        public async Task<List<VenderComboBox>> FindVenderByVenderType(int? id)
        {
            List<VenderComboBox> Vender = await _context.Healthprofessionals.Where(E => E.Profession == id)
                .Select(r => new VenderComboBox
                {
                   VenderId = r.Vendorid,
                   VenderName = r.Vendorname
                })
                .ToListAsync();

            return Vender;
        }
        #endregion

        #region FindVenderByVenderID
        public async Task<ViewOrder> FindVenderByVenderID(int? id)
        {
            ViewOrder Vender = await _context.Healthprofessionals.Where(E => E.Vendorid == id)
                .Select(r => new ViewOrder
                {
                    VenderId = r.Vendorid,
                    BusinessContact = r.Businesscontact,
                    Email = r.Email,
                    FaxNumber = r.Faxnumber
                })
                .FirstAsync();

            return Vender;
        }
        #endregion

        #region SaveViewOrder
        public async Task<bool> SaveViewOrder(ViewOrder viewOrder)
        {
            try
            {
                var Orderdetail = new Orderdetail();
                // Aspnetuser
                Orderdetail.Vendorid = viewOrder.VenderId;
                Orderdetail.Requestid = viewOrder.RequestId;
                Orderdetail.Faxnumber = viewOrder.FaxNumber;
                Orderdetail.Email = viewOrder.Email;
                Orderdetail.Createddate = DateTime.Now;
                Orderdetail.Noofrefill = viewOrder.Refills;
                Orderdetail.Prescription = viewOrder.Prescription;
                Orderdetail.Businesscontact = viewOrder.BusinessContact;
                _context.Orderdetails.Add(Orderdetail);
                await _context.SaveChangesAsync();

                var req = await _context.Requests.Where(e => e.Requestid == viewOrder.RequestId).FirstOrDefaultAsync();
                _emailConfig.SendMail(viewOrder.Email, "New Order arrived", viewOrder.Prescription + "Request name" + req.Firstname);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
           
        }
        #endregion

        #region GetPartnersByProfession
        public async Task<List<ViewVendorList>> GetPartnersByProfession(int? regionId,string? searchvender)
        {


            List<ViewVendorList> pl = await (from r in _context.Healthprofessionals
                                                 join t in _context.Healthprofessionaltypes on r.Profession equals t.Healthprofessionalid
                                                 where r.Isdeleted == new BitArray(1) &&
                                                       (!regionId.HasValue || r.Profession == regionId) &&
                                                       (searchvender == null || r.Vendorname.ToLower().Contains(searchvender.ToLower()))
                                                 select new ViewVendorList
                                                 {
                                                     VendorID = r.Vendorid,
                                                     Profession = t.Professionname,
                                                     BusinessContact = r.Businesscontact ?? "",
                                                     Email = r.Email,
                                                     FaxNumber = r.Faxnumber,
                                                     PhoneNumber = r.Phonenumber,
                                                     VendorName = r.Vendorname,

                                                 })
                                     .ToListAsync();


            return pl;

        }
        #endregion

        #region SavePartner
        public async Task<bool> SavePartner(Healthprofessional SavePartner)
        {
            try
            {
                if(SavePartner.Vendorid != null)
                {
                    var UpdateHealthprofessional = await _context.Healthprofessionals.FirstAsync(r=> r.Vendorid == SavePartner.Vendorid);
                    UpdateHealthprofessional.Vendorname = SavePartner.Vendorname;
                    UpdateHealthprofessional.Faxnumber = SavePartner.Faxnumber;
                    UpdateHealthprofessional.Email = SavePartner.Email;
                    UpdateHealthprofessional.Createddate = DateTime.Now;
                    UpdateHealthprofessional.Phonenumber = SavePartner.Phonenumber;
                    UpdateHealthprofessional.Profession = SavePartner.Profession;
                    UpdateHealthprofessional.State = SavePartner.State;
                    UpdateHealthprofessional.City = SavePartner.City;
                    UpdateHealthprofessional.Regionid = SavePartner.Regionid;
                    UpdateHealthprofessional.Zip = SavePartner.Zip;
                    UpdateHealthprofessional.Businesscontact = SavePartner.Businesscontact;
                    UpdateHealthprofessional.Isdeleted = new BitArray(1);
                    UpdateHealthprofessional.Isdeleted[0] = false;
                    _context.Healthprofessionals.Update(UpdateHealthprofessional);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var Healthprofessional = new Healthprofessional();
                    // Aspnetuser
                    Healthprofessional.Vendorname = SavePartner.Vendorname;
                    Healthprofessional.Faxnumber = SavePartner.Faxnumber;
                    Healthprofessional.Email = SavePartner.Email;
                    Healthprofessional.Createddate = DateTime.Now;
                    Healthprofessional.Phonenumber = SavePartner.Phonenumber;
                    Healthprofessional.Profession = SavePartner.Profession;
                    Healthprofessional.State = SavePartner.State;
                    Healthprofessional.City = SavePartner.City;
                    Healthprofessional.Regionid = SavePartner.Regionid;
                    Healthprofessional.Zip = SavePartner.Zip;
                    Healthprofessional.Businesscontact = SavePartner.Businesscontact;
                    Healthprofessional.Isdeleted = new BitArray(1);
                    Healthprofessional.Isdeleted[0] = false;

                    _context.Healthprofessionals.Add(Healthprofessional);
                    await _context.SaveChangesAsync();
                }
                

               
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        #endregion

        #region GetPartnerById
        public async Task<Healthprofessional> GetPartnerById(int? venderId)
        {


            Healthprofessional pl = await _context.Healthprofessionals.FirstOrDefaultAsync(r => r.Vendorid == venderId);

            return pl;

        }
        #endregion

        #region DeletePartnerById
        public async Task<bool> DeletePartnerById(int? venderId)
        {
            try
            {
                var UpdateHealthprofessional = await _context.Healthprofessionals.FirstAsync(r => r.Vendorid == venderId);
                UpdateHealthprofessional.Isdeleted = new BitArray(1);
                UpdateHealthprofessional.Isdeleted[0] = true;
                _context.Healthprofessionals.Update(UpdateHealthprofessional);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
          

        }
        #endregion
    }
}
