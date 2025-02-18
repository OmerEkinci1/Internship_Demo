﻿using Core.DataAccess;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ITranslateRepository : IEntityRepository<Translate>
    {
        Task<List<TranslateDto>> GetTranslateDto();
        Task<Dictionary<string, string>> GetTranslateWordList(string lang);
        Task<string> GetTranslatesByLang(string langCode);
    }
}
