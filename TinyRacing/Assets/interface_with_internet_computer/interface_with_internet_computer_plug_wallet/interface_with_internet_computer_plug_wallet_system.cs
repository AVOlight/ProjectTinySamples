using System;
using System.Runtime.InteropServices;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Tiny.UI;
using Unity.Transforms;
namespace AVOlight
{

  public class interface_with_internet_computer_plug_wallet_system : SystemBase
  {

    #region MonoPInvokeCallback attribute for JavaScript interop
    internal class MonoPInvokeCallbackAttribute : Attribute
    {
      public MonoPInvokeCallbackAttribute() { }
      public MonoPInvokeCallbackAttribute(Type t) { }
    }
    [DllImport("__Internal")]
    private static extern void plug_wallet_request_connect(in_string_out_void_delegate call_back_result_1_principal_identifier);
    [MonoPInvokeCallback(typeof(in_string_out_void_delegate))]
    #endregion
    private static void request_connect_call_back(string principal_identifier)
    {
      Console.WriteLine($"[C#] {nameof(principal_identifier)} {principal_identifier}");
      user_profile.connected = !string.IsNullOrEmpty(principal_identifier);
      connect_state = user_profile.connected ? state.connected : state.connect;
#if unity_entities_component_data_strings_working
      user_profile.principal_identifier = principal_identifier; 
#endif
    }
    static user_profile_data user_profile;
    static state connect_state;
    protected override void OnCreate()
    {
      Console.WriteLine(nameof(interface_with_internet_computer_plug_wallet_system));
    }
    enum state
    {
      connect,
      connecting,
      connected
    }
    bool animating_icon;
    bool plug_wallet_button_entity_enabled = true;
    protected override void OnUpdate()
    {
      if (connect_state == state.connect ||
        (plug_wallet_button_entity_enabled && connect_state == state.connected))
      {
        var plug_wallet_button_entity = GetSingletonEntity<interface_with_internet_computer_plug_wallet>();
        #region disabled plug wallet button after connected
        if (connect_state == state.connected)
        {
          SetSingleton(user_profile);
          #region AddComponent<Disabled> making GetSingletonEntity throw exception ?
#if false
          // no nested game objects being disabled by parent
          if (!EntityManager.HasComponent<Disabled>(plug_wallet_button_entity))
          {
            EntityManager.AddComponent<Disabled>(plug_wallet_button_entity);
          }
          plug_wallet_button_entity_enabled = false;
#endif
          #endregion
          var rect = EntityManager.GetComponentData<RectTransform>(plug_wallet_button_entity);
          rect.Hidden = true;
          EntityManager.SetComponentData<RectTransform>(plug_wallet_button_entity, rect);
          return;
        }
        #endregion
        var plug_wallet_component = GetSingleton<interface_with_internet_computer_plug_wallet>();
        #region set clicked_entity if clicked
        var clicked_entity = Entity.Null;
        Entities.ForEach((Entity e, in UIState state) =>
        {
          if (state.IsClicked)
          {
            clicked_entity = e;
          }
        }).Run();
        #endregion
        if (clicked_entity != null && plug_wallet_button_entity == clicked_entity)
        {
          plug_wallet_request_connect(request_connect_call_back);
          connect_state = state.connecting;
        }
      }
      #region try rotating the plug icon while waiting on connection
      var animate_icon = connect_state == state.connecting;
      if (animating_icon || animate_icon)
      {
        var delta_time = Time.DeltaTime;
        var plug_wallet_button_icon_entity = GetSingletonEntity<plug_wallet_button_icon>();
        var rotation = animate_icon ?
          math.mul(
            EntityManager.GetComponentData<Rotation>(plug_wallet_button_icon_entity).Value,
            quaternion.AxisAngle(math.forward(), delta_time))
          : quaternion.identity;
        EntityManager.SetComponentData<Rotation>(plug_wallet_button_icon_entity, new Rotation
        {
          Value = rotation,
        });
        animating_icon = animate_icon;
      }
      #endregion
    }
  }


}
