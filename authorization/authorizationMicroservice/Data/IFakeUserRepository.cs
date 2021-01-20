using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorizationMicroservice.Data
{
    public interface IFakeUserRepository
    {
        public void FillData();
    }
}
