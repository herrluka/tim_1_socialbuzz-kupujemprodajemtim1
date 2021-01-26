using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Models;

namespace ReactionsService.Data
{
    public class TypeOfReactionRepository : ITypeOfReactionRepository
    {

        private readonly ContextDB contextDB;

        public TypeOfReactionRepository(ContextDB contextDB)
        {
            this.contextDB = contextDB;
        }

        public List<TypeOfReaction> GetAllTypesOfReaction()
        {
            return contextDB.Type_Of_Reaction.ToList();
        }

        public TypeOfReaction GetTypeOfReactionByID(int typeOfReactionID)
        {
            return contextDB.Type_Of_Reaction.FirstOrDefault(e => e.TypeOfReactionID == typeOfReactionID);

        }

        public void AddTypeOfReaction(TypeOfReaction typeOfReaction)
        {
            contextDB.Add(typeOfReaction);
        }

        public void UpdateTypeOfReaction(TypeOfReaction typeOfReaction)
        {
            
        }

        public void DeleteTypeOfReaction(int typeOfReactionID)
        {
            var type = GetTypeOfReactionByID(typeOfReactionID);
            contextDB.Remove(type);
        }

        public bool SaveChanges()
        {
            return contextDB.SaveChanges() > 0;
        }


    }
}
