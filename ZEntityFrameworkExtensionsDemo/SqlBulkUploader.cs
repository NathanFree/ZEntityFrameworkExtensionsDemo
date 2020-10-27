using System;
using System.Collections.Generic;
using System.Text;
using Z.BulkOperations;
using Z.EntityFramework.Extensions;
using ZEntityFrameworkExtensionsDemo.Models;

namespace ZEntityFrameworkExtensionsDemo
{
    public class SqlBulkUploader
    {
        private readonly MyDbContext _context;

        public SqlBulkUploader(
            MyDbContext context)
        {
            _context = context;

            EntityFrameworkManager.ContextFactory = context =>
                new MyDbContext();
        }

        public void BulkInsert<T>(IEnumerable<T> items, bool includeGraph = true)
            where T : class =>
            _context.BulkInsert(items, options =>
            {
                if (includeGraph)
                {
                    options.IncludeGraph = true;
                    options.IncludeGraphOperationBuilder = operation =>
                    {
                        /** InsertKeepIdentity value false means that if you define a PK for a row, 
                         * your PK is ignored. true means your defined PK is respected, but every
                         * record you insert is required to have a user-defined PK. by grabbing the
                         * bulk operation, we can turn it on only for those records where we want to
                         * define a PK during import
                         */
                        if (operation is BulkOperation<Owner> ownerBulk)
                        {
                            ownerBulk.InsertKeepIdentity = true;
                        }
                    };
                }
            });

        public void BulkUpdate<T>(IEnumerable<T> items, bool includeGraph = false)
            where T : class =>
            _context.BulkUpdate(items, options => options.IncludeGraph = includeGraph);

        public void BulkDelete<T>(IEnumerable<T> items, bool includeGraph = false)
            where T : class =>
            _context.BulkDelete(items, options => options.IncludeGraph = includeGraph);
    }
}
