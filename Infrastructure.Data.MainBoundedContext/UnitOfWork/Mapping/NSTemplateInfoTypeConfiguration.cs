using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.UnitOfWork.Mapping
{
    class NSTemplateInfoTypeConfiguration : EntityTypeConfiguration<NSTemplateInfo>
    {
        public NSTemplateInfoTypeConfiguration()
        {
            //this.Property(b => b.TemplateId).IsRequired();
            //this.Property(b => b.TemplteName).IsRequired();
            //this.Property(b => b.TemplateText).IsRequired();
            this.ToTable("NSTemplateInfo");
        }
    }


    class NSTemplateTypeTypeConfiguration : EntityTypeConfiguration<NSTemplateType>
    {
        public NSTemplateTypeTypeConfiguration()
        {
            this.ToTable("NSTemplateType");
        }
    }

    class NSNodeInfoTypeConfiguration : EntityTypeConfiguration<NSNodeInfo>
    {
        public NSNodeInfoTypeConfiguration()
        {
            this.ToTable("NSNodeInfo");
        }
    }

    class NSTaskInfoTypeConfiguration : EntityTypeConfiguration<NSTaskInfo>
    {
        public NSTaskInfoTypeConfiguration()
        {
            this.ToTable("NSTaskInfo");
        }
    }

    class NSInstanceInfoTypeConfiguration : EntityTypeConfiguration<NSInstanceInfo>
    {
        public NSInstanceInfoTypeConfiguration()
        {
            this.ToTable("NSInstanceInfo");
        }
    }

    class NSNodeGroupTypeConfiguration : EntityTypeConfiguration<NSNodeGroup>
    {
        public NSNodeGroupTypeConfiguration()
        {
            this.ToTable("NSNodeGroup");
        }
    }

    class NSRoutingInfoTypeConfiguration : EntityTypeConfiguration<NSRoutingInfo>
    {
        public NSRoutingInfoTypeConfiguration()
        {
            this.ToTable("NSRoutingInfo");
        }
    }

    class NSRoutingDataTypeConfiguation : EntityTypeConfiguration<NSRoutingData>
    {
        public NSRoutingDataTypeConfiguation()
        {
            this.ToTable("NSRoutingData");
        }
    }
}
