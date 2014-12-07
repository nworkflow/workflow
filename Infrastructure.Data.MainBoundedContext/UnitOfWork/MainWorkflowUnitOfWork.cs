using Infrastructure.Data.MainBoundedContext.UnitOfWork.Mapping;
using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;
using Nsun.Domain.MainBoundedContext.WorkflowModule.Aggregates.WorkflowAgg;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.MainBoundedContext.UnitOfWork
{
    public class MainWorkflowUnitOfWork : DbContext, IQueryableUnitOfWork
    {

        public MainWorkflowUnitOfWork()
            : base("Server=.;Initial Catalog=NsunDB1;Integrated Security=true;MultipleActiveResultSets=True")
        {
        }

        public MainWorkflowUnitOfWork(string connectionStr):base(connectionStr)
        {
           // Database.SetInitializer<MainWorkflowUnitOfWork>(new DropCreateDatabaseIfModelChanges<MainWorkflowUnitOfWork>());
            //bool isChange =  Database.CompatibleWithModel(false);
            //if(isChange)
              //  Database.Initialize(true);
        }

        #region IDbSet Members

        IDbSet<NSTemplateInfo> _nsTemplateInfos;
        public IDbSet<NSTemplateInfo> NSTemplateInfos
        {
            get
            {
                if (_nsTemplateInfos == null)
                    _nsTemplateInfos = base.Set<NSTemplateInfo>();

                return _nsTemplateInfos;
            }
        }

        IDbSet<NSTemplateType> _nsTemplateTypes;
        public IDbSet<NSTemplateType> NSTemplateTypes
        {
            get
            {
                if (_nsTemplateTypes == null)
                    _nsTemplateTypes = base.Set<NSTemplateType>();
                return _nsTemplateTypes;
            }
        }

        IDbSet<NSTaskInfo> _nsTaskInfos;
        public IDbSet<NSTaskInfo> NsTaskInfos
        {
            get
            {
                if (_nsTaskInfos == null)
                    _nsTaskInfos = base.Set<NSTaskInfo>();
                return _nsTaskInfos;
            }
        }

        IDbSet<NSInstanceInfo> _nsInstanceInfos;
        public IDbSet<NSInstanceInfo> NsInstanceInfos
        {
            get
            {
                if (_nsInstanceInfos == null)
                    _nsInstanceInfos = base.Set<NSInstanceInfo>();
                return _nsInstanceInfos;
            }
        }

        IDbSet<NSNodeInfo> _nsNodeInfos;
        public IDbSet<NSNodeInfo> NSNodeInfos
        {
            get {
                if (_nsNodeInfos == null)
                    _nsNodeInfos = base.Set<NSNodeInfo>();
                return _nsNodeInfos; }
        }


        IDbSet<NSNodeGroup> _nsNodeGroups;

        public IDbSet<NSNodeGroup> NsNodeGroups
        {
            get
            {
                if (_nsNodeGroups == null)
                    _nsNodeGroups = base.Set<NSNodeGroup>();
                return _nsNodeGroups;
            }
        }


        IDbSet<NSRoutingInfo> _nsRoutingInfos;
        public IDbSet<NSRoutingInfo> NsRoutingInfos
        {
            get
            {
                if (_nsRoutingInfos == null)
                    _nsRoutingInfos = base.Set<NSRoutingInfo>();
                return _nsRoutingInfos;
            }
        }

        IDbSet<NSRoutingData> _nsRoutingDatas;
        public IDbSet<NSRoutingData> NSRoutingDatas
        {
            get
            {
                if (_nsRoutingDatas == null)
                    _nsRoutingDatas = base.Set<NSRoutingData>();
                return _nsRoutingDatas;
            }
        }

        #endregion

        #region IQueryableUnitOfWork Members

        public DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            //attach and set as unchanged
            base.Entry<TEntity>(item).State = System.Data.EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry<TEntity>(item).State = System.Data.EntityState.Modified;
        }
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if it is not attached, attach original and set current values
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }


        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });

                }
            } while (saveFailed);

        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = System.Data.EntityState.Unchanged);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #endregion

        #region DbContext Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Remove unused conventions
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //Add entity configurations in a structured way using 'TypeConfiguration’ classes
            modelBuilder.Configurations.Add(new NSTemplateInfoTypeConfiguration());
            modelBuilder.Configurations.Add(new NSTemplateTypeTypeConfiguration());
            modelBuilder.Configurations.Add(new NSTaskInfoTypeConfiguration());
            modelBuilder.Configurations.Add(new NSInstanceInfoTypeConfiguration());
            modelBuilder.Configurations.Add(new NSNodeInfoTypeConfiguration());
            modelBuilder.Configurations.Add(new NSNodeGroupTypeConfiguration());
            modelBuilder.Configurations.Add(new NSRoutingInfoTypeConfiguration());
            modelBuilder.Configurations.Add(new NSRoutingDataTypeConfiguation());
        }

        #endregion

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
