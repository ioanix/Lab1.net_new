using System;
using System.Collections.Generic;

namespace Lab1_new.ViewModels.Reservations
{
    public class NewReservationRequest
    {
        public List<int> ReservedMovieIds { get; set; }

        public DateTime? ReservationDateTime { get; set; }
    }
}
