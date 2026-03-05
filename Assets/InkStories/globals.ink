//globals.ink
// Global state (shared across ALL NPCs)

// --- NPC tracking ---
LIST NPC = maerlon, child, kase, erixa, drayk, eira, marrek
VAR met_npcs = ()              // list of NPCs the player has met

EXTERNAL giveScrap(amount)

VAR npcs_talked = 0
VAR Dialogue_scrap = 0

// --- Story flags ---
VAR met_child = false
VAR maerlon_gave_scrap = false
VAR drayk_gave_scrap = false



=== function register_npc(who) ===
{
- met_npcs ? who:        
    ~ return false
- else:
    ~ met_npcs += who
    ~ npcs_talked += 1
    ~ return true
}