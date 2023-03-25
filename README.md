# Cordite's Chore Challenge!

Challenge Details can be found [here](https://docs.google.com/document/d/1BgYWu5N-_5U_qfNfUSLKdJscn_HbSp2FsdPj79YfAGQ/edit)

Cordite's [Discord](https://discord.gg/pFwZpv8)

Coverage Tracker [Doc](https://docs.google.com/spreadsheets/d/1eT_f9JHEmKZOs4Xj1IsX7IrUrrQVZyH1DUhV8QXVVxk/edit?usp=sharing)

## Needs
* Help implementing the remaining 16 achievements
* Ability to determine if other mods are loaded
    * Either display a warning or block them?
* A way better score panel, this is just the fastest thing I could come up with

## Adding an achievement
* Clone the repo/get it building on your machine
* Create a file in the Framework/Achievements folder (can group multiple if they make sense, see `UberEats` for example)
* Copy the `TemplateAchievement` class defined into `TemplateAchievement.cs` into the created class, rename it to the class name you want
* Implement whatever logic is needed for the achievement
    * You can log events to the smapi console with
        ```cs
        instance.Monitor.Log($"{some_data}", LogLevel.Alert)
        ```
    * When an achievement should be unlocked, call
        ```cs
        instance.HasSeen = true;
        ```
* Attach it in `ModEntry.cs` by adding it to the `Achievements` list.

## More complicated achievements
There are some helpers for things like
* Collect N of a thing (`CumulativeAchievement`)
    * In the constructor, set `MaxValue=#` to define the max threshold
    * In your logic, if a new item should be included call `instance.CurrentValue++`
    * This will trigger an info box with your completion amount and will spawn the `HasSeen` check automatically once `CurrentValue == MaxValue`
* Trigger all achievement (`AchievementCollection`)
    * In the constructor just create a new list of all the achievements that should be associated. 
    * NOTE: THESE DO NOT NEED TO BE MANUALLY REGISTERED TO `ModEntry.cs`
    * They'll automatically pop on their own, and once all have popped it'll pop this collective achievement.
