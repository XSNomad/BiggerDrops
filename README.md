# BiggerDrops The Stand Alone Edition. 
For those that want more units without any dependencies. 

Bigger Drops is a mod for HBS BattleTech that adds on the ability to expand your drop size from 4 to 8 or more units

## Settings
Bigger drops is configurable through the following settings. The values shown here are the default values if not present in the settings.json file

*Note: Some settings are only applicable when Custom Units is present or not present in the load order*


```json
{
  "debugLog": false, 
  "additionalLanceName": "AI LANCE",
  "additinalMechSlots": 4,
  "additinalPlayerMechSlots": 4,
  "defaultMaxTonnage": 500,
  "allowUpgrades" : false,
  "showAdditionalArgoUpgrades" : false,
  "argoUpgradeName" : "Command & Control",
  "argoUpgradeCategory1Name" : "Drop Size", 
  "argoUpgradeCategory2Name" : "Mech Control", 
  "argoUpgradeCategory3Name" : "Drop Tonnage",
  "respectFourDropLimit" : false,
  "limitFlashpointDrop" : true,
}
```

### Always Applicable
the following settings are always applicable:

`debugLog`: Add additional logging, unless you are hunting a bug, set this to false

`defaultMaxTonnage` : The max tonnage you can drop by default. Note: when `allowUpgrades` is true, this is the max tonnage you can drop at the start of a career, 
upgrades/events can alter this value by changing the `BiggerDrops_MaxTonnage` stat. Also Note that contracts can override this value in all cases.

`allowUpgrades` : When true allows Argo Upgrades/events to increase or decrease the available drop tonnage and slots, when disabled the settings file setting will always apply.

`showAdditionalArgoUpgrades` : When true, an additional area on the Argo's upgrade screen becomes available for upgrades to appear in with 3 separate categories for upgrades.

`argoUpgradeName` :  When `showAdditionalArgoUpgrades` is true, this is the name of the new upgrades bar in the Argo's upgrade screen.

`argoUpgradeCategory1Name` : When `showAdditionalArgoUpgrades` is true, this is the name of the 1st category in the new upgrade bar

`argoUpgradeCategory2Name` : When `showAdditionalArgoUpgrades` is true, this is the name of the 2nd category in the new upgrade bar

`argoUpgradeCategory3Name` : When `showAdditionalArgoUpgrades` is true, this is the name of the 3rd category in the new upgrade bar

`respectFourDropLimit`: When true, contracts max units must be set to -1 or else the unit count in the contract will be enforced (by default most contracts are set to 4),
contracts that limit you to less than 4 mechs will be restricted regardless of this setting

`limitFlashpointDrop` : when set to true, flashpoint drops will be limited to 4 mechs, setting this to false will allow additional slots to be available, 
provided MissionControl is also configured to allow for this

`additionalLanceName`: the name of the additional lance on the lance configuration screen

### When Running Without CustomUnits or With CustomUnits V1

the following settings are applicable when running without CustomUnits or when CustomUnits is present with V1 of its expanded lance API,
they are not applicable if the version of CustomUnits has V2 of the API available 

`additinalMechSlots`: the number of additional mech slots to add, valid range of:
- `0 - 4`

if upgrades are enabled this is the starting number of slots for a career, events or upgrades can alter this value by changing the `BiggerDrops_AdditionalMechSlots` stat.
Note, if this value is greater than the value of `additinalPlayerMechSlots`, then extra units above that value will be assigned to the control of an allied AI player. 

Example: if `additinalMechSlots` is set to 3, but `additinalPlayerMechSlots` is set to 1, then the 2 extra units will be assigned to the allied AI.

*Note: Yes this setting is misspelled, however it has been left so for compatibility reasons*

`additinalPlayerMechSlots`: the number of additional mechs under the control of the player. valid range of:
- `0 - 4`

this value cannot be greater then `additinalMechSlots`.

if upgrades are enabled, this value acts as the starting value for a career, events or upgrades can alter this value by changing the `BiggerDrops_AdditionalPlayerMechSlots` stat.

*Note: Yes this setting is misspelled, however it has been left so for compatibility reasons*
