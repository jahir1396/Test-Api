using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Test.EntityFramework.TestDbContext;

namespace Sistema.Proyecto
{
    public interface IUnitOfWork : IDisposable
    {
        TestDbContext Context { get; }
        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public TestDbContext Context { get; }

        public UnitOfWork(TestDbContext context)
        {
            Context = context;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
