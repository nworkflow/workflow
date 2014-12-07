using Infrastructure.Data.MainBoundedContext.UnitOfWork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Nsun.Workflow.Intall.InitInfo
{
    public class TestUnit : MainWorkflowUnitOfWork
    {
        public TestUnit()
            : base("Server=.;Initial Catalog=NsunDB_Test;Integrated Security=true;MultipleActiveResultSets=True")
        {
        }
    }

    public class MainWorkflowUnitOfWorkInitializer : DropCreateDatabaseAlways<MainWorkflowUnitOfWork>
    {
        public MainWorkflowUnitOfWorkInitializer()
        {
            Seed(new MainWorkflowUnitOfWork());
        }

    
    protected override void Seed(MainWorkflowUnitOfWork unitOfWork)
        {

            
            
            // {"列名 'Id' 无效。\r\n列名 'TemplteName' 无效。\r\n列名 'TemplateType' 无效。"}
            var book = new NSTemplateInfo(Guid.NewGuid(),"测试","<xml ?>");
            unitOfWork.NSTemplateInfos.Add(book);
            unitOfWork.Database.CompatibleWithModel(false);
            unitOfWork.NSTemplateTypes.Add(new NSTemplateType() { Id=Guid.NewGuid(), TemplateType="Test", TemplateDes="Test Des" ,Version="1.0.0.1"});
            unitOfWork.NSNodeInfos.Add(new NSNodeInfo() {Id=Guid.NewGuid() });
            unitOfWork.SaveChanges();
        }
    }
}
