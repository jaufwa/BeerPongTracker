using BeerPongTracker.Api.Models;
using BeerPongTracker.BusinessLogic.Cup;
using BeerPongTracker.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeerPongTracker.Api.Controllers
{
    [RoutePrefix("api/Cup")]
    public class CupController : ApiController
    {
        private readonly ICupLogic _cupLogic;

        public CupController()
        {
            _cupLogic = new CupLogic(new BeerPongFederationEntities());
        }
    }
}
