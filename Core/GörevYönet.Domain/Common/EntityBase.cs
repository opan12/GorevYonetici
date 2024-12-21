using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GörevYönet.Domain.Common
{
    public class EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Id'nin her yeni TaskItem için benzersiz olması için
        public DateTimeOffset CreatedDate { get; set; }         // Görev oluşturulma tarihi
        public DateTimeOffset? UpdatedDate { get; set; }         // Görev oluşturulma tarihi

    }
}
