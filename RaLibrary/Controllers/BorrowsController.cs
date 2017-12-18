using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace RaLibrary.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/borrows")]
    [RaAuthentication]
    [RaLibraryAuthentication]
    [RaLibraryAuthorize(Roles = RoleTypes.Administrators + "," + RoleTypes.ServiceAccount)]
    public class BorrowsController : RaLibraryController
    {
        #region Fields
        
        private IBorrowManager _borrows = new BorrowManager();

        #endregion Fields

        [Route("")]
        [HttpGet]
        [ResponseType(typeof(IQueryable<BorrowDto>))]
        public async Task<IHttpActionResult> ListBorrows()
        {
            IQueryable<BorrowDto> borrows = await _borrows.ListAsync();

            return Ok(borrows);
        }
    }
}
