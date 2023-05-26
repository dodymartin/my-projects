using MinimalApi.Dal;
using MinimalApi.Repositories;

namespace MinimalApi.Services
{
    public class UnitOfWorkService : IDisposable
    {
        private readonly MinimalApiDbContext _dbContext;

        private readonly IBaseCrudRepo<ApiCallUsage, Guid> _apiCallUsageRepo;
        public IBaseCrudRepo<ApiCallUsage, Guid> ApiCallUsageRepo => _apiCallUsageRepo ?? new BaseCrudRepo<ApiCallUsage, Guid>(_dbContext);

        private readonly ApplicationRepo _applicationRepo;
        public IApplicationRepo ApplicationRepo => _applicationRepo ?? new ApplicationRepo(_dbContext);

        private readonly ApplicationFacilityRepo _applicationFacilityRepo;
        public IApplicationFacilityRepo ApplicationFacilityRepo => _applicationFacilityRepo ?? new ApplicationFacilityRepo(_dbContext);

        private readonly ControllerUriInfoByApplicationRepo _controllerUriInfoByApplicationRepo;
        public IControllerUriInfoByApplicationRepo ControllerUriInfoByApplicationRepo => _controllerUriInfoByApplicationRepo ?? new ControllerUriInfoByApplicationRepo(_dbContext);

        private readonly ControllerUriFacilityInfoByApplicationRepo _controllerUriFacilityInfoByApplicationRepo;
        public IControllerUriFacilityInfoByApplicationRepo ControllerUriFacilityInfoByApplicationRepo => _controllerUriFacilityInfoByApplicationRepo ?? new ControllerUriFacilityInfoByApplicationRepo(_dbContext);

        private readonly DatabaseRepo _databaseRepo;
        public IDatabaseRepo DatabaseRepo => _databaseRepo ?? new DatabaseRepo(_dbContext);

        private readonly PingUriRepo _pingUriRepo;
        public IPingUriRepo PingUriRepo => _pingUriRepo ?? new PingUriRepo(_dbContext);

        private readonly WebApiVersionRepo _webApiVersionRepo;
        public IWebApiVersionRepo WebApiVersionRepo => _webApiVersionRepo ?? new WebApiVersionRepo(_dbContext);

        public UnitOfWorkService(MinimalApiDbContext context)
        {
            _dbContext = context;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _dbContext.Dispose();
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
