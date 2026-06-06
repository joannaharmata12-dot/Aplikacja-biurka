using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooking.SharedKernel;

public class DeskDto
{
    public int Id { get; set; }
    public string DeskNumber { get; set; } = string.Empty;
    public int FloorId { get; set; }

    public int X_Coordinate { get; set; }
    public int Y_Coordinate { get; set; }

    public bool IsAvailable { get; set; }
}