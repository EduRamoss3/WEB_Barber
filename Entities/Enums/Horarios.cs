using System;
using System.Collections.Generic;

namespace Barber.UI.Entities.Enums
{
    public class Horarios
    {
        // Propriedade para armazenar a lista de horários
        public List<DateTime> HorariosList { get; set; } = new List<DateTime>
        {
            // Horários definidos para um dia fictício (01/01/0001)
            DateTime.Parse("0001-01-01 07:30:00"),
            DateTime.Parse("0001-01-01 08:00:00"),
            DateTime.Parse("0001-01-01 08:30:00"),
            DateTime.Parse("0001-01-01 09:00:00"),
            DateTime.Parse("0001-01-01 09:30:00"),
            DateTime.Parse("0001-01-01 10:00:00"),
            DateTime.Parse("0001-01-01 10:30:00"),
            DateTime.Parse("0001-01-01 11:00:00"),
            DateTime.Parse("0001-01-01 11:30:00"),
            DateTime.Parse("0001-01-01 12:00:00"),
            DateTime.Parse("0001-01-01 12:30:00"),
            DateTime.Parse("0001-01-01 13:00:00"),
            DateTime.Parse("0001-01-01 13:30:00"),
            DateTime.Parse("0001-01-01 14:00:00"),
            DateTime.Parse("0001-01-01 14:30:00"),
            DateTime.Parse("0001-01-01 15:00:00"),
            DateTime.Parse("0001-01-01 15:30:00"),
            DateTime.Parse("0001-01-01 16:00:00"),
            DateTime.Parse("0001-01-01 16:30:00"),
            DateTime.Parse("0001-01-01 17:00:00"),
            DateTime.Parse("0001-01-01 17:30:00"),
            DateTime.Parse("0001-01-01 18:00:00"),
            DateTime.Parse("0001-01-01 18:30:00"),
            DateTime.Parse("0001-01-01 19:00:00"),
            DateTime.Parse("0001-01-01 19:30:00"),
            DateTime.Parse("0001-01-01 20:00:00"),
            DateTime.Parse("0001-01-01 20:30:00"),
            DateTime.Parse("0001-01-01 21:00:00")
        };
    }
}
