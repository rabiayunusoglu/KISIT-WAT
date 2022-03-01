using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
     public interface IChargePersonManager
    {
        ChargePersonDTO CreatePerson(ChargePersonDTO chargePerson);
        ChargePersonDTO GetPersonById(System.Guid id);
        bool DeletePerson(System.Guid id);
        List<ChargePersonDTO> GetAllPersons();
        ChargePersonDTO UpdatePerson(ChargePersonDTO chargePerson);
        
    }
   
}
