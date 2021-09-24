using System;
using TinyRacing;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Tiny;
using Unity.Tiny.Text;
using Unity.Tiny.UI;
namespace AVOlight
{

  public class leader_board_system : SystemBase
  {

    protected override void OnCreate()
    {
      Console.WriteLine(nameof(leader_board_system));

    }
    bool leader_board_user_interface_updated;
    protected override void OnUpdate()
    {
      var user = GetSingleton<user_profile_data>();
      var entity = GetSingletonEntity<leader_board_table>();
      var table_rect = EntityManager.GetComponentData<RectTransform>(entity);
      if ((user.connected && table_rect.Hidden) || (!user.connected && !table_rect.Hidden))
      {
        table_rect.Hidden = !user.connected;
        EntityManager.SetComponentData<RectTransform>(entity, table_rect);
        Console.WriteLine($"{nameof(leader_board_table)} hidden {table_rect.Hidden}");
        if (table_rect.Hidden) { return; }
      }

      if (!HasSingleton<Race>())
      {
        return;
      }
      var race =
        GetSingleton<Race>();
      // new Race { RaceTimer = 22.2f };
      if (race.IsInProgress()) { leader_board_user_interface_updated = false; }
      if (leader_board_user_interface_updated
        || !race.IsFinished()
        ) { return; }
      #region trying to figure out why user interface text isn't updating....
#if true
      Entities.ForEach((Entity e, ref TextRenderer tr, ref RectTransform rect, ref UIName uiName) =>
  {

    if (
      uiName.Name != "row0"
    //!HasComponent<leader_board_table_row>(e)
    )
    {
      return;
    }
    TextLayout.SetEntityTextRendererString(EntityManager, e, $"{leader_board_table_row.time_string(race.RaceTimer)}");

  }).WithStructuralChanges().WithReadOnly(race).Run();
#endif
      #endregion
      Entities.ForEach((Entity e, ref leader_board_table_row row) =>
      {
        Console.WriteLine($"{nameof(leader_board_table_row)} {row.index} update");
        //TextLayout.SetEntityTextRendererString(EntityManager, e, $"{row.index}");
        if (row.index == 0)
        {
          row.set_score(race.RaceTimer);
        }
        else
        {
          // return until scores supported...
          return;
        }
        if (row.should_update_text_renderer_string())
          TextLayout.SetEntityTextRendererString(EntityManager, e, row.get_updated_text_renderer_string());
      }).WithStructuralChanges().WithReadOnly(race).Run();
      leader_board_user_interface_updated = true;
    }
  }


}
