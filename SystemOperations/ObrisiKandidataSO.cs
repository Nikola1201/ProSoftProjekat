using Common.Domain;
using DBBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemOperations
{
    /// <summary>
    /// Sistemska operacija za brisanje kandidata iz sistema.
    /// Pre brisanja kandidata uklanja sve njegove upise kako bi se izbeglo kršenje referencijalnog integriteta.
    /// </summary>
    public class ObrisiKandidataSO : SystemOperationBase
    {
        private Kandidat argument;

        /// <summary>Konstruktor za produkcijsku upotrebu (podrazumevani broker).</summary>
        /// <param name="argument">Kandidat za brisanje.</param>
        public ObrisiKandidataSO(Kandidat argument) : this(argument, null) { }

        /// <summary>Konstruktor sa injektovanim brokerom (test-friendly).</summary>
        /// <param name="argument">Kandidat za brisanje.</param>
        /// <param name="broker">Broker za pristup bazi.</param>
        public ObrisiKandidataSO(Kandidat argument, IBroker? broker) : base(broker)
        {
            this.argument = argument;
        }

        /// <inheritdoc/>
        protected override void ExecuteConcreteOperation()
        {
            List<Upis> upisiKandidata = _broker.GetEntitiesByQuery(new Upis())
                                        .Cast<Upis>()
                                        .ToList();
            foreach(Upis up in upisiKandidata)
            {
                _broker.Delete(up);
            }
            _broker.Delete(argument);
            
        }
    }
}
