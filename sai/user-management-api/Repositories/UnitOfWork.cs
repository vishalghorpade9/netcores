using System;
using user_management_api.Data;
using user_management_api.Model;
namespace user_management_api.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private UserManagementDbContext context;
        private GenericRepository<User> userRepository;

        public UnitOfWork()
        {
        }

        public UnitOfWork(UserManagementDbContext context)
        {
            this.context = context;
        }

        public GenericRepository<User> UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(context);
                }
                return userRepository;
            }
        }        

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

       

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
