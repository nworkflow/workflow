using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Infrastructure.Data.MainBoundedContext.WorkflowModule.Repositories;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Nsun.Workflow.Common
{
    public partial interface IService1
    {
        [OperationContract]
        string NewTemplate(NSTemplateInfo nsTemplateInfo);


        [OperationContract]
        string NewTemplateType(NSTemplateType nsTemplateInfo);


        [OperationContract]
        string DelTemplateTypes(List<NSTemplateType> delItems);


        [OperationContract]
        string NewTemplateTypes(List<NSTemplateType> nsTemplateInfos);

        [OperationContract]
        List<NSTemplateType> GetAllTemplateType();

        [OperationContract]
        List<NSTemplateInfo> GetAllTemplateInfoByType(string type);
    }

    public partial class Service1
    {
        #region 模板名称

        public string NewTemplate(NSTemplateInfo nsTemplateInfo)
        {
            if (string.IsNullOrEmpty(nsTemplateInfo.TemplateName)
                || string.IsNullOrWhiteSpace(nsTemplateInfo.TemplateText)
                || string.IsNullOrWhiteSpace(nsTemplateInfo.TemplateType))
            {
                return "模板名称/模板内容/模板类型不能为空！";
            }


            TemplateRepository templateRespository = new TemplateRepository(Context);


            string version = templateRespository.GetTemplateVersion(nsTemplateInfo.TemplateName, nsTemplateInfo.TemplateType);
            int tempateVersion = -1;
            int.TryParse(version, out tempateVersion);
            nsTemplateInfo.Version = "1.0.0.1";//  (++tempateVersion).ToString();


            if (templateRespository != null)
            {
                templateRespository.Add(nsTemplateInfo);
                templateRespository.SaveChage();

            }

            return string.Empty;
        }


        public List<NSTemplateInfo> GetAllTemplateInfoByType(string templateType)
        {
            TemplateRepository templateRespository = new TemplateRepository(Context);
            List<NSTemplateInfo> nsTempleateInfos = new List<NSTemplateInfo>();
            var templateNames = templateRespository.GetAll().Where(p => p.TemplateType == templateType).OrderByDescending(p => p.CreateTime).GroupBy(p => p.TemplateName);

            templateNames.ToList().ForEach(p =>
            {
                nsTempleateInfos.Add(p.First());
            });
            return nsTempleateInfos;
        }
        #endregion（模板名称）

        #region 模板类型

        public string NewTemplateType(NSTemplateType nsTemplateInfo)
        {
            if (nsTemplateInfo == null)
                return "模板类型不能为空!";

            try
            {
                NSTemplateTypeRepository t = new NSTemplateTypeRepository(Context);
                t.Add(nsTemplateInfo);
                t.SaveChage();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            return string.Empty;
        }


        public string NewTemplateTypes(List<NSTemplateType> nsTemplateInfos)
        {
            try
            {
                NSTemplateTypeRepository t = new NSTemplateTypeRepository(Context);
                foreach (var item in nsTemplateInfos)
                {
                    t.Add(item);
                }
                t.SaveChage();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }


        public string DelTemplateTypes(List<NSTemplateType> delItems)
        {
            try
            {
                NSTemplateTypeRepository t = new NSTemplateTypeRepository(Context);
                t.RemoveAll(delItems);
                t.SaveChage();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }


        public List<NSTemplateType> GetAllTemplateType()
        {
            NSTemplateTypeRepository t = new NSTemplateTypeRepository(Context);
            return t.GetAll().ToList();
        }


        public NSTemplateInfo GetTemplateById(Guid id)
        {
            TemplateRepository t = new TemplateRepository(Context);
            return t.Get(id);
        }

        #endregion (模板类型)
    }
}
