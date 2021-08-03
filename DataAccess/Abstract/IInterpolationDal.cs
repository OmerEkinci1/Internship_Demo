﻿using Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IInterpolationDal : IEntityRepository<Interpolation>
    {
        Task<Interpolation> GetByID(int id);
    }
}
