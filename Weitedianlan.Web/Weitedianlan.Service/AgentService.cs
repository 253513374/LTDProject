using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Response;

namespace Weitedianlan.Service
{
    public class AgentService
    {
        private Db _db;

        public AgentService(Db db)
        {
            this._db = db;
        }

        public ResponseModel GetAgentList()
        {
            //var agents = _db.Agents.Select(s=> new { s.AID,s.AName,s.APeople,s.ABelong,s.AType}).ToList().OrderByDescending(c => c.AID);

            var agents = _db.Agent.AsNoTracking().ToList().OrderByDescending(c => c.AID);
            // var agents = _db.W_LabelStorages.ToList();
            var response = new ResponseModel();
            response.Code = 200;
            response.Status = "Agent集合获取成功";
            response.Data = new List<Agent>();

            response.Data = agents;
            //foreach (var banner in banners)
            //{
            //    response.Data.Add(new tAgent
            //    {
            //        Id = banner.Id,
            //        Image = banner.Image,
            //        Url = banner.Url,
            //        Remark = banner.Remark
            //    });
            //}
            return response;
        }
    }
}