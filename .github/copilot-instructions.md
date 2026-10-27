# GitHub Copilot Instructions for [XND] Profitable Weapons (Continued)

## Mod Overview and Purpose
[XND] Profitable Weapons (Continued) is an update to the original Profitable Weapons mod. It enhances the game by increasing the sell price multiplier of weapons to 100%, making it profitable for players to produce and sell weapons. This mod also introduces a unique tagging system for weapons to maintain balance:

- **Looted Weapons**: These weapons are marked with a (U) tag to distinguish them from non-looted weapons. They have a sell price multiplier of 20% and can still be sold to traders. There is no mood penalty associated with equipping them.
- **Weapon Tagging Criteria**: Weapons gain the (U) tag if dropped by raiders or friendlies, if used 20 times, or if dropped from trader inventories. The latter two criteria are configurable through mod settings.

## Key Features and Systems
- **Sell Price Multiplier**: Configurable multiplier for weapon sell prices.
- **Looted Weapon Tagging**: Unique tagging system for looted weapons.
- **Stockpile Filters**: Two new stockpile filters to manage looted and non-looted weapons separately.
- **Compatibility**: Works with Combat Extended, MendAndRecycle, and Nano Repair Tech.

## Coding Patterns and Conventions
- **C# Files**: The mod primarily uses C# for its logic and behavior.
- **XML Files**: XML is used for defining new game elements such as stat parts, special thing filters, and stats.
- **Harmony Patching**: The mod uses the Harmony Patch Library to modify vanilla game behavior.

### C# File Structure
- **CompLootedWeapon.cs**: Contains the component for looted weapons, including methods for checking and modifying their state.
- **SpecialThingFilterWorker_NonLootedWeapon.cs**: Defines a special thing filter for non-looted weapons.
- **StaticConstructorClass.cs**: A static constructor class for initializing the mod.
- **StatPart_IsLootedWeapon.cs**: A stat part that modifies stats based on whether a weapon is looted.
- **Pawn_EquipmentTracker_TryDropEquipment.cs**: A harmony patch for equipment tracking to handle looted weapons.

### XML File Structure
- **Stats_Basics_General.xml**: Defines basic stats for the game.
- **SpecialThingFilters.xml**: Defines special thing filters for looted and non-looted weapons.

## XML Integration
XML files are crucial for defining new game elements and modifying existing ones. Key XML files include:

- **Stats_Basics_General.xml**: Used to define basic stats and modify existing ones.
- **SpecialThingFilters.xml**: Defines special thing filters for stockpiles, allowing for better management of looted and non-looted weapons.

## Harmony Patching
Harmony patching is used to modify vanilla game behavior without altering the original codebase. Key harmony patches include:

- **Pawn_EquipmentTracker_TryDropEquipment.cs**: Postfix patch to handle looted weapons when they are dropped.

## Suggestions for Copilot
To ensure Copilot generates code that aligns with the mod's conventions and requirements, consider the following suggestions:

- **Context**: Provide context for the code being generated, such as the purpose of the component or the specific behavior being modified.
- **Harmony Patching**: When generating harmony patches, ensure that the patch follows the correct syntax and targets the correct method.
- **XML Definitions**: When generating XML definitions, ensure that the new elements are properly integrated into the game's data structure.

By following these guidelines, Copilot can help you generate high-quality code for your RimWorld mod.

## Additional Resources
- **Discord Channel**: For error reporting and support.
- **GitHub Repository**: For contributing solutions and reporting issues.



This .github/copilot-instructions.md file provides detailed guidance for using GitHub Copilot in the context of the [XND] Profitable Weapons (Continued) RimWorld mod. It covers the mod's purpose, key features, coding conventions, XML integration, harmony patching, and provides suggestions for Copilot usage.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

