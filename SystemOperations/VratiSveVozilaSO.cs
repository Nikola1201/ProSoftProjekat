using Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SystemOperations
{
    public class VratiSveVozilaSO : SystemOperationBase
    {
        public List<Vozilo> Result { get; private set; }

        protected override void ExecuteConcreteOperation()
        {
            Result = _broker.GetAll(new Vozilo())
                .Cast<Vozilo>()
                .Where(v => v.Aktivno)
                .ToList();
        }
    }
}
