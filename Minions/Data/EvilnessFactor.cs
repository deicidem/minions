using System;
using System.Collections.Generic;

#nullable disable

namespace Minions.Data
{
    public partial class EvilnessFactor
    {
        public EvilnessFactor()
        {
            Villains = new HashSet<Villain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Villain> Villains { get; set; }
    }
}
