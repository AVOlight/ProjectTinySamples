using Unity.Collections;
using Unity.Entities;

namespace AVOlight
{
  [GenerateAuthoringComponent]
  public struct user_profile_data : IComponentData
  {
    #if unity_entities_component_data_strings_working
    public FixedString64 principal_identifier;
    #endif
    public bool connected;
  }
}