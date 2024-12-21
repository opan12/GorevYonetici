using GörevYönet.Domain.Common;
using GörevYönet.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GörevYönet.Domain.Entitites
{
    public class TaskItem:EntityBase
    {
        public string Description { get; set; }           // Görev açıklaması
        public DateTime DueDate { get; set; }             // Görev bitiş tarihi
        public string UserId { get; set; }                // Kullanıcı kimliği
        public bool IsCompleted { get; set; }             // Tamamlandı mı?
        public double Progress { get; set; }              // İlerleme yüzdesi
      
        public DateTime? CompletedDate { get; set; }      // Tamamlanma tarihi (isteğe bağlı)
        public bool IsDeleted { get; set; }               // Silinmiş mi?
        public TaskPriority Priority { get; set; }        // Öncelik
        public TaskGroupingKey GroupingKey { get; set; }  // Gruplama anahtarı



    }
}
