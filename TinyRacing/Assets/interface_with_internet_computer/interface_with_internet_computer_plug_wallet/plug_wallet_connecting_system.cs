using System;
using Unity.Entities;
using Unity.Transforms;
namespace AVOlight
{

  [UpdateBefore(typeof(interface_with_internet_computer_plug_wallet_system))]
  public class plug_wallet_connecting_system : SystemBase
  {
    protected override void OnCreate()
    {
      Console.WriteLine(nameof(plug_wallet_connecting_system));

    }
    bool animating;
    protected override void OnUpdate()
    {
    }
  }


}
