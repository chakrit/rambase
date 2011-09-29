
namespace RAMBase.Tests
{
  public class Dud
  {
    public class X
    {
      public string Hello { get; set; }
      public string K { get; set; }
    }

    public void a()
    {
      var x = RAM.Configure<X>();

    }
  }
}
