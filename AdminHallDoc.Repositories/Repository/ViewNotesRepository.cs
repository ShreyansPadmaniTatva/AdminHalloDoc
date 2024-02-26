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
        public async Task<ViewNotesModel> GetNotesByRequest(int? id)
        {
            var requestlog = _context.Requeststatuslogs.Where(E => E.Requestid == id && E.Transtophysician != null );

            ViewNotesModel notes = await _context.Requestnotes.Where(r => r.Requestid == id).Select(r => new ViewNotesModel
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
            }).FirstAsync();

            List<TransfernotesModel> transferlist = new List<TransfernotesModel>();
            foreach (var e in requestlog)
            {
                transferlist.Add(new TransfernotesModel
                {
                    Requestid = e.Requestid,
                    Notes = e.Notes,
                    Physicianid = e.Physicianid,
                    Createddate = e.Createddate,
                    Requeststatuslogid = e.Requeststatuslogid,
                    Transtoadmin = e.Transtoadmin,
                    Transtophysicianid = e.Transtophysicianid
                });
            }
            notes.transfernotes = transferlist;

            return notes;
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

    }
}
