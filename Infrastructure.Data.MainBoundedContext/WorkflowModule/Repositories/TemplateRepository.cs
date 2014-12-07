using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories
{
    public class TemplateRepository : Repository<NSTemplateInfo>, INSTemplateInfoRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="unitOfWork">Associated unit of work</param>
        public TemplateRepository(MainWorkflowUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }


        public override NSTemplateInfo Get(Guid id)
        {
            if (id != Guid.Empty)
            {
                var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;

                var set = currentUnitOfWork.CreateSet<NSTemplateInfo>();

                return set.Where(c => c.Id == id)
                          .SingleOrDefault();
            }
            else
                return null;
        }


        public string GetTemplateVersion(string templateName,string templateType)
        {
            if (string.IsNullOrWhiteSpace(templateName) || string.IsNullOrWhiteSpace(templateType))
                return "-1";
            var currentUnitOfWorkf = this.UnitOfWork as MainWorkflowUnitOfWork;
            var set = currentUnitOfWorkf.CreateSet<NSTemplateInfo>();

            var version =  set.Where(p => p.TemplateName == templateName && p.TemplateType == templateType).OrderByDescending(p => p.CreateTime).FirstOrDefault();
            if (version != null)
            {
                return version.Version?? "0";
            }

            return "0";
        }

        public IEnumerable<NSTemplateInfo> GetEnabled(int pageIndex, int pageCount)
        {
            var currentUnitOfWork = this.UnitOfWork as MainWorkflowUnitOfWork;

            return currentUnitOfWork.NSTemplateInfos.OrderBy(c => c.CreateTime)
                                     .Skip(pageIndex * pageCount)
                                     .Take(pageCount);
        }

        #endregion
    }
}
