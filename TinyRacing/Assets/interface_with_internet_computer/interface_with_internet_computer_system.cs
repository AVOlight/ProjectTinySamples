using System;
using System.Runtime.InteropServices;
using Unity.Entities;
namespace AVOlight
{

  public class interface_with_internet_computer_system : SystemBase
  {

    protected override void OnCreate()
    {
      Console.WriteLine(nameof(interface_with_internet_computer_system));
    }

    protected override void OnUpdate()
    {
    }
  }


}
