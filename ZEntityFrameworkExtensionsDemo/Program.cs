using System;

namespace ZEntityFrameworkExtensionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {



            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
            };
        }
    }
}
