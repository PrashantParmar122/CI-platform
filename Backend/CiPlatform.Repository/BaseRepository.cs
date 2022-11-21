using CiPlatform.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Repository
{
    public class BaseRepository
    {
        protected readonly CiPlatformContext _context = new CiPlatformContext();
    }
}
