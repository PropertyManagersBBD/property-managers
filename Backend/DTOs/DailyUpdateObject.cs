using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
  public record Marriages(long partner_a, long partner_b);
  public record Children(long parent, long child);
  public record Deaths(long deceased, long next_of_kin);
  public class DailyUpdateModel
  {
    public Marriages[] marriages { get; set; }
    public Children[] children { get; set; }

    public long[] adults { get; set; }
    
    public Deaths[] deaths { get; set; }

    public DailyUpdateModel(Marriages[] marriages, Children[] children, long[] adults, Deaths[] deaths)
    {
      this.marriages = marriages;
      this.children = children;
      this.adults = adults;
      this.deaths = deaths;
    }
  }
}