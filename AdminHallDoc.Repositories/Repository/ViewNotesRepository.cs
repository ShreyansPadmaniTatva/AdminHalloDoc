using AdminHalloDoc.Entities.Data;
using AdminHalloDoc.Entities.Models;
using AdminHalloDoc.Entities.ViewModel;
using AdminHalloDoc.Entities.ViewModel.AdminViewModel;
using AdminHalloDoc.Entities.ViewModel.PatientViewModel;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            try
            {
                ViewNotesModel notes = await _context.Requestnotes
                    .Where(r => r.Requestid == id)
                    .Select(r => new ViewNotesModel
                    {
                        Requestid = r.Requestid,
                        Administrativenotes = r.Administrativenotes,
                        Adminnotes = r.Adminnotes,
                        Createdby = r.Createdby,
                        Createddate = r.Createddate,
                        Intdate = r.Intdate,
                        Intyear = r.Intyear,
                        Ip = r.Ip,
                        Modifiedby = r.Modifiedby,
                        Modifieddate = r.Modifieddate,
                        Physiciannotes = r.Physiciannotes,
                        Requestnotesid = r.Requestnotesid,
                    })
                    .FirstOrDefaultAsync();

                if (notes != null)
                {
                    var requestlog = await _context.Requeststatuslogs
                        .Where(E => E.Requestid == id && E.Transtophysician != null)
                        .ToListAsync();

                    List<TransfernotesModel> transferlist = requestlog.Select(e => new TransfernotesModel
                    {
                        Requestid = e.Requestid,
                        Notes = e.Notes,
                        Physicianid = e.Physicianid,
                        Createddate = e.Createddate,
                        Requeststatuslogid = e.Requeststatuslogid,
                        Transtoadmin = e.Transtoadmin,
                        Transtophysicianid = e.Transtophysicianid
                    }).ToList();

                    notes.transfernotes = transferlist;
                }
                else
                {
                     notes = new ViewNotesModel();
                    notes.Requestid = id;
                }
                

                return notes;
            }
            catch (Exception e)
            {
                throw; // You might want to handle the exception more gracefully here
            }

        }
        #endregion

        #region PutNotes 

        public bool PutNotes(string? adminnotes, string? physiciannotes, int? RequestID)
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

                        _context.Requestnotes.Update(notes);
                        _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (adminnotes != null)
                {
                    if (notes != null)
                    {

                        notes.Adminnotes = adminnotes;
                        notes.Modifieddate = DateTime.Now;

                        _context.Requestnotes.Update(notes);
                        _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
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
