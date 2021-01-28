using ReactionsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace ReactionsService.Data
{
    public interface ITypeOfReactionRepository
    {
        List<TypeOfReaction> GetAllTypesOfReaction();

        TypeOfReaction GetTypeOfReactionByID(int typeOfReactionID);

        void AddTypeOfReaction(TypeOfReaction typeOfReaction);

        void UpdateTypeOfReaction(TypeOfReaction typeOfReaction);

        void DeleteTypeOfReaction(int typeOfReactionID);
        public bool SaveChanges();


    }
}
