// Global state (shared across ALL NPCs)
LIST NPC = npc_maerlon, npc_child

VAR met_npcs = ()               // InkList of NPC items
VAR npcs_talked = 0             // This is what Unity can read

VAR met_child = false
VAR maerlon_gave_scrap = false
VAR Dialogue_scrap = 0

// Call this from an NPC the FIRST time you want to count them.
// It will only increment once per NPC, ever.
=== function RegisterNPC(npcId)
{ met_npcs ? npcId:
    // already registered -> do nothing
- else:
    ~ met_npcs = met_npcs + npcId
    ~ npcs_talked = npcs_talked + 1
}
