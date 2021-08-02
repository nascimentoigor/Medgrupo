using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teste.Domain.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}