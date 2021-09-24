using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
namespace AVOlight
{
  [GenerateAuthoringComponent]
  public struct leader_board_table_row : IComponentData
  {
    public int index;
    public float score;
#if unity_entities_component_data_strings_working
    public FixedString64 name; 
#endif
    bool should_update_text_renderer_string_bool;
    public bool should_update_text_renderer_string() { return should_update_text_renderer_string_bool; }
    public void set_score(float score)
    {
#if unity_entities_component_data_strings_working
      set(name, score); 
#else
      set(null, score);
#endif
    }
    public void set(FixedString64 name, float score)
    {
#if unity_entities_component_data_strings_working
      this.name = name; 
#endif
      this.score = score;
      should_update_text_renderer_string_bool = true;
    }
    public string get_updated_text_renderer_string()
    {
      should_update_text_renderer_string_bool = false;
      var time = time_string(score);
#if unity_entities_component_data_strings_working
      return index == 0 ? $"{name} {time}" : $"#{index} {name} {time}"; 
#else
      return index == 0 ? time : $"#{index} {time}";
#endif
    }
    public static string time_string(float seconds)
    {
      var seconds_remainder = math.floor((seconds % 60) * 100) / 100.0f;
      var minutes = ((int)(seconds / 60)) % 60;
      return $"{minutes:00}:{seconds_remainder:00.00}";
    }
  }
}
